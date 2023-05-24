using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public partial class SettingModuleManager : ModuleFoundation
{
    private Dictionary<ScriptFoundation, List<SettingAttribute>> ItemsInScriptFoundation { get; set; } = new Dictionary<ScriptFoundation, List<SettingAttribute>>();
    private Dictionary<string, List<Action<(object prev, object post)>>> Callback = new Dictionary<string, List<Action<(object prev, object post)>>>();
    
    private List<SettingAttribute> ItemsInMemory { get; set; } = new List<SettingAttribute>();
    
    public void Initialize()
    {

    }

    public void Save()
    {
        string directory = Path.Combine(SettingModuleConstValue.DEFAULT_SAVE_DIRECTORY, "Setting.json");
        using (FileStream fs = new FileStream(directory, FileMode.Create))
        {
            var json = JsonConvert.SerializeObject(ItemsInMemory, Formatting.Indented, serializerSettings);

            StreamWriter writer = new StreamWriter(fs);
            writer.Write(json);
            writer.Flush();
        }
    }
}
