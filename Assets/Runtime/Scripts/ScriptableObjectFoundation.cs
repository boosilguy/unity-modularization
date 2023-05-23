using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectFoundation : ScriptableObject, ScriptFoundation
{
    protected SettingModuleManager SettingModuleManager => ModuleManager.Instance?.GetModule<SettingModuleManager>();
    
    protected virtual void Awake() => SettingModuleManager?.AcceptScriptFoundation(this);
}
