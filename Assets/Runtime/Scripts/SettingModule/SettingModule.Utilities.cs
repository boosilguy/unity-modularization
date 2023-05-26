using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class SettingModuleManager : ModuleFoundation
{
    public delegate void SettingUpdateEvent<arg1, arg2>(arg1 oldValue, arg2 newValue);
    class SettingCallbackContainer
    {
        internal event SettingUpdateEvent<object, object> Changed;
    }
}
