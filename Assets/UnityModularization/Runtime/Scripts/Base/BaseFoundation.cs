using setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFoundation : IScriptFoundation
{
    protected SettingModuleManager SettingModuleManager => ModuleManager.Instance?.GetModule("SettingModuleManager") as SettingModuleManager;

    public BaseFoundation()
    {
        SettingModuleManager?.AcceptScriptFoundation(this);
    }
}
