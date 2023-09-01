using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using setting;

public class ScriptableObjectFoundation : ScriptableObject, IScriptFoundation
{
    protected SettingModuleManager SettingModuleManager => ModuleManager.Instance?.GetModule("SettingModuleManager") as SettingModuleManager;
    
    protected virtual void Awake()
    {
        SettingModuleManager?.AcceptScriptFoundation(this);
    }
}
