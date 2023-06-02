using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public partial class SettingModuleManager : ModuleFoundation
{
    internal Dictionary<ScriptFoundation, List<SettingAttribute>> itemsInScriptFoundation = new Dictionary<ScriptFoundation, List<SettingAttribute>>();
    private Dictionary<string, SettingCallbackContainer> callbacksItemUpdate = new Dictionary<string, SettingCallbackContainer>();

    public List<SettingAttribute> ItemsInRuntime => ItemsInSaved.Concat(ItemsInNew).OrderBy(item => item.Tag).ThenBy(item => item.Name).ToList();
    private List<SettingAttribute> ItemsInSaved { get; set; } = new List<SettingAttribute>();
    private List<SettingAttribute> ItemsInNew { get; set; } = new List<SettingAttribute>();

    bool init = false;

    /// <summary>
    /// Setting module을 초기화합니다.
    /// </summary>
    public void Initialize()
    {
        if (!init)
        {
            ClearAllContainer();
            ItemsInSaved = Load();
            ItemsInNew = GetNewItems();
            init = true;
        }
    }

    /// <summary>
    /// 현재 메모리에 올라간 Setting들을 저장합니다.
    /// </summary>
    public void Save()
    {
        string directory;
        
        if (Application.isEditor)
        {
            directory = SettingModuleConstValue.DEFAULT_EDITOR_SAVE_DIRECTORY;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            directory = Path.Combine(directory, SettingModuleConstValue.DEFAULT_FILENAME_EXTENSION);
            using (FileStream fs = new FileStream(directory, FileMode.Create))
            {
                Serialize(ItemsInRuntime, fs);
            }
            return;
        }

        directory = SettingModuleConstValue.DEFAULT_BUILD_SAVE_DIRECTORY;
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        directory = Path.Combine(directory, SettingModuleConstValue.DEFAULT_FILENAME_EXTENSION);
        using (FileStream fs = new FileStream(directory, FileMode.Create))
        {
            Serialize(ItemsInRuntime, fs);
        }
    }
    
    /// <summary>
    /// Local file로부터, Setting을 불러들입니다.
    /// </summary>
    /// <returns>Loaded setting list</returns>
    public List<SettingAttribute> Load()
    {
        List<SettingAttribute> result;
        if (Application.isEditor)
            result = LoadSettingsFromDirectory(
                Path.Combine(
                    SettingModuleConstValue.DEFAULT_EDITOR_SAVE_DIRECTORY, 
                    SettingModuleConstValue.DEFAULT_FILENAME_EXTENSION));
        else
            result = LoadSettingsFromDirectory(
                Path.Combine(
                    SettingModuleConstValue.DEFAULT_BUILD_SAVE_DIRECTORY, 
                    SettingModuleConstValue.DEFAULT_FILENAME_EXTENSION));
        return result;
    }

    /// <summary>
    /// 대상 Setting을 구독하며, 이에 대한 이벤트를 추가합니다.
    /// </summary>
    /// <param name="settingName">Setting name</param>
    /// <param name="updateEvent">Event</param>
    public void AddUpdateEvent(string settingName, SettingUpdateEvent<object, object> updateEvent)
    {
        if (!callbacksItemUpdate.ContainsKey(settingName))
        {
            if (!itemsInScriptFoundation.Any(item => item.Value.Any(setting => setting.Name == settingName)))
            {
                Debug.LogWarning(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Not found] ", Color.red),
                        new ColorfulText("The setting name is not found => ", Color.white),
                        new ColorfulText(settingName, Color.yellow)));

                return;
            }
            callbacksItemUpdate.Add(settingName, new SettingCallbackContainer());
        }
        callbacksItemUpdate[settingName].Changed += updateEvent;
    }

    /// <summary>
    /// 대상 Setting에 구독하였던 이벤트를 제거합니다.
    /// </summary>
    /// <param name="settingName">Setting name</param>
    /// <param name="updateEvent">Event</param>
    public void RemoveUpdateEvent(string settingName, SettingUpdateEvent<object, object> updateEvent) 
    {
        if (!callbacksItemUpdate.ContainsKey(settingName))
        {
            Debug.LogWarning(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("[Not found] ", Color.red),
                    new ColorfulText("The setting name is not found => ", Color.white),
                    new ColorfulText(settingName, Color.yellow)));

            return;
        }
        callbacksItemUpdate[settingName].Changed -= updateEvent;
    }

    /// <summary>
    /// Setting 모듈의 Setting 컨테이너들을 초기화합니다.
    /// </summary>
    private void ClearAllContainer()
    {
        ItemsInSaved.Clear();
        ItemsInNew.Clear();
        itemsInScriptFoundation.Clear();
        callbacksItemUpdate.Clear();
    }

    /// <summary>
    /// 디렉토리로부터 Setting 정보를 직렬화합니다.
    /// </summary>
    /// <param name="directory">대상 디렉토리</param>
    /// <returns>직렬화한 Setting list</returns>
    private List<SettingAttribute> LoadSettingsFromDirectory(string directory)
    {
        List<SettingAttribute> result = new List<SettingAttribute>();
        if (!File.Exists(directory))
        {
            Debug.LogError(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("[Fail to load] ", Color.red),
                    new ColorfulText("Not found directory (", Color.white),
                    new ColorfulText(directory, Color.yellow),
                    new ColorfulText(")", Color.white)));
        }
        else
        {
            using (FileStream fs = new FileStream(directory, FileMode.OpenOrCreate))
            {
                result = Deserialize<List<SettingAttribute>>(fs);
            }

            if (result == null)
            {
                Debug.LogError(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Fail to load] ", Color.red),
                        new ColorfulText("The setting list is empty (", Color.white),
                        new ColorfulText(directory, Color.yellow),
                        new ColorfulText(")", Color.white)));
            }
            else
            {
                Debug.Log(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Initialized] ", Color.green),
                        new ColorfulText("Complete load setting file (", Color.white),
                        new ColorfulText(directory, Color.yellow),
                        new ColorfulText(")", Color.white)));
            }
        }
        return result;
    }

    /// <summary>
    /// 스크립트에 작성된 Setting들 중, 새로 추가된 Setting들을 담는 Container를 초기화합니다. 
    /// </summary>
    /// <returns>저장된 Setting외, 스크립트에서만 활용되는 Setting list</returns>
    private List<SettingAttribute> GetNewItems()
    {
        IEnumerable<string> itemNameInSaved = ItemsInSaved.Select(item => item.Name);
        IEnumerable<SettingAttribute> foundNotExistInSaved = GetAllSettingsInScripts().Where(setting => !itemNameInSaved.Contains(setting.Name))
            .GroupBy(item => item.Name)
            .Select(group =>
            {
                var itemWithValue = group.FirstOrDefault(item => item.IsDefaultValue());
                return itemWithValue ?? group.First();
            });
        return foundNotExistInSaved.ToList();
    }

    /// <summary>
    /// 스크립트에 작성된 Setting들을 Distinct 처리하여 반환하되, 기본값이 아닌 Setting에 대해서 우선적으로 반환합니다. 
    /// </summary>
    /// <returns>현재 스크립트에서 활용되는 Setting list</returns>
    private List<SettingAttribute> GetAllSettingsInScripts()
    {
        List<SettingAttribute> result = new List<SettingAttribute>();
        List<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(t => typeof(ScriptFoundation).IsAssignableFrom(t)).ToList();
        
        IEnumerable<MemberInfo> memberContainer;
        foreach (var foundation in types)
        {
            memberContainer = foundation.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                        .Where(m => m.GetCustomAttribute<SettingAttribute>() != null);
            foreach (var member in memberContainer)
            {
                var settingAttribute = member.GetCustomAttribute<SettingAttribute>();
                result.Add(settingAttribute);
            }
        }

        result = result.GroupBy(item => item.Name)
            .Select(group =>
            {
                var itemWithValue = group.FirstOrDefault(item => item.IsDefaultValue());
                return itemWithValue ?? group.First();
            }).ToList();

        return result;
    }
}
