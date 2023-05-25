using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class SettingModuleManager : ModuleFoundation
{
    class SettingCallbackContainer
    {
        internal event Action<object, object> Changed;
    }
}
