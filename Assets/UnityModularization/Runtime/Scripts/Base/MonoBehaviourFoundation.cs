using setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourFoundation : MonoBehaviour, IScriptFoundation
{
    protected SettingModuleManager SettingModuleManager => ModuleManager.Instance?.GetModule("SettingModuleManager") as SettingModuleManager;

    protected virtual void Awake()
    {
        SettingModuleManager?.AcceptScriptFoundation(this);
    }
}
