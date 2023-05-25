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

    // callbacksItemUpdate[settingName] += updateEvent 구현에 대해 고민해 볼 것
    public void AddUpdateEvent(string settingName, Action<object, object> updateEvent)
    {
        if (!callbacksItemUpdate.ContainsKey(settingName))
        {
            if (!itemsInScriptFoundation.Any(item => item.Value.Any(setting => setting.Name == settingName)))
            {
                throw new NullReferenceException(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Invalid assignment] ", Color.red),
                        new ColorfulText("The setting name is not found => ", Color.white),
                        new ColorfulText(settingName, Color.yellow)));
            }
            callbacksItemUpdate.Add(settingName, new SettingCallbackContainer());
        }
        callbacksItemUpdate[settingName].Changed += updateEvent;
    }

    public void RemoveUpdateEvent(string settingName, Action<object, object> updateEvent) 
    {
        if (!callbacksItemUpdate.ContainsKey(settingName))
        {
            Debug.LogWarning(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("[Invalid assignment] ", Color.red),
                    new ColorfulText("The setting name is not found => ", Color.white),
                    new ColorfulText(settingName, Color.yellow)));
        }
        callbacksItemUpdate[settingName].Changed -= updateEvent;
    }
}
