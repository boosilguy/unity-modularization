using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public partial class SettingModuleManager : ModuleFoundation
{
    private Dictionary<ScriptFoundation, List<SettingAttribute>> itemsInScriptFoundation = new Dictionary<ScriptFoundation, List<SettingAttribute>>();
    private Dictionary<string, SettingCallbackContainer> callbacksItemUpdate = new Dictionary<string, SettingCallbackContainer>();
    
    private List<SettingAttribute> ItemsInMemory { get; set; } = new List<SettingAttribute>();
    
    public void Initialize()
    {
        ItemsInMemory.Clear();
        ItemsInMemory = Load();
    }

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
                Serialize(ItemsInMemory, fs);
            }
            return;
        }

        directory = SettingModuleConstValue.DEFAULT_BUILD_SAVE_DIRECTORY;
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        directory = Path.Combine(directory, SettingModuleConstValue.DEFAULT_FILENAME_EXTENSION);
        using (FileStream fs = new FileStream(directory, FileMode.Create))
        {
            Serialize(ItemsInMemory, fs);
        }
    }
    
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

    private List<SettingAttribute> LoadSettingsFromDirectory(string directory)
    {
        List<SettingAttribute> result = null;
        if (!File.Exists(directory))
        {
            throw new FileNotFoundException(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("The setting file is not found (", Color.white),
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
                Debug.Log(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Fail to load] ", Color.red),
                        new ColorfulText("The setting list is empty (", Color.white),
                        new ColorfulText(directory, Color.yellow),
                        new ColorfulText(")", Color.white)));
                return null;
            }
            else
            {
                Debug.Log(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Initialized] ", Color.green),
                        new ColorfulText("Complete load setting file (", Color.white),
                        new ColorfulText(directory, Color.yellow),
                        new ColorfulText(")", Color.white)));
                return result;
            }
        }
    }

    private void Merge()
    {
        foreach (SettingAttribute item in ItemsInMemory)
        {
            if (!itemsInScriptFoundation.ContainsKey(item.ScriptFoundation))
            {
                itemsInScriptFoundation.Add(item.ScriptFoundation, new List<SettingAttribute>());
            }
            itemsInScriptFoundation[item.ScriptFoundation].Add(item);
        }
    }
}
