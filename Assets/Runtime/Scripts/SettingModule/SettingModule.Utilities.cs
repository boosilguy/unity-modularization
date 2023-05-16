using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class SettingModuleManager : ModuleFoundation
{
    private void InvokeCallbackViaType(SettingAttribute settingAttribute, Action intTypeCallback, Action floatTypeCallback, Action stringTypeCallback, Action boolTypeCallback)
    {
        switch (settingAttribute)
        {
            case IntAttribute intAttribute:
                intTypeCallback.Invoke();
                break;
            case FloatAttribute floatAttribute:
                floatTypeCallback.Invoke();
                break;
            case StringAttribute stringAttribute:
                stringTypeCallback.Invoke();
                break;
            case BoolAttribute boolAttribute:
                boolTypeCallback.Invoke();
                break;
        }
    }
}
