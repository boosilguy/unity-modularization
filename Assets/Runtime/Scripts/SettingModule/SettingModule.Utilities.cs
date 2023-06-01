using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class SettingModuleManager : ModuleFoundation
{
    /// <summary>
    /// Setting 값이 수정시 발행할 이벤트의 델레게이트입니다.
    /// </summary>
    /// <typeparam name="arg1">Type1</typeparam>
    /// <typeparam name="arg2">Type2</typeparam>
    /// <param name="oldValue">변경 전 값</param>
    /// <param name="newValue">변경 후 값</param>
    public delegate void SettingUpdateEvent<arg1, arg2>(arg1 oldValue, arg2 newValue);
    
    class SettingCallbackContainer
    {
        internal event SettingUpdateEvent<object, object> Changed;
    }
}
