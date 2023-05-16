using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public partial class SettingModuleManager : ModuleFoundation
{
    Dictionary<ScriptFoundation, List<SettingAttribute>> SettingDic { get; set; } = new Dictionary<ScriptFoundation, List<SettingAttribute>>();

    public void Initialize()
    {

    }

    
}
