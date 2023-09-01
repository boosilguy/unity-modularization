using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace setting
{
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
            internal void Invoke(object oldValue, object newValue) => Changed?.Invoke(oldValue, newValue);
        }

        /// <summary>
        /// Setting 값을 수정합니다.
        /// </summary>
        /// <param name="settingName">Setting name</param>
        /// <param name="value">새로 주입할 Value</param>
        public void UpdateSetting(string settingName, object value)
        {
            if (value == null)
            {
                Debug.LogWarning(
                    RichTextUtil.GetColorfulText(
                        new ColorfulText("[Null value] ", Color.red),
                        new ColorfulText("The value is null", Color.white)));
                return;
            }
            else
            {
                var itemsInSF = itemsInScriptFoundation.Values.SelectMany(list => list).Where(item => item.Name == settingName).ToList();
                if (itemsInSF != null && itemsInSF.Count() != 0)
                {
                    var itemsInRuntime = ItemsInRuntime.Where(item => item.Name == settingName).FirstOrDefault();
                    if (itemsInRuntime != null)
                    {
                        var prevSettingValue = itemsInRuntime.Value;

                        itemsInRuntime.Value = value;
                        foreach (var item in itemsInSF)
                            item.Accept.Invoke(item.ScriptFoundation, item.Member, value);

                        if (callbacksItemUpdate.Keys.Contains(settingName))
                            callbacksItemUpdate[settingName].Invoke(prevSettingValue, value);
                    }
                    else
                    {
                        Debug.LogWarning(
                            RichTextUtil.GetColorfulText(
                                new ColorfulText("[Not found] ", Color.red),
                                new ColorfulText("There is no setting => ", Color.white),
                                new ColorfulText(settingName, Color.yellow)));
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning(
                        RichTextUtil.GetColorfulText(
                            new ColorfulText("[Not found] ", Color.red),
                            new ColorfulText("There is no setting => ", Color.white),
                            new ColorfulText(settingName, Color.yellow)));
                    return;
                }
            }
        }
    }

}
