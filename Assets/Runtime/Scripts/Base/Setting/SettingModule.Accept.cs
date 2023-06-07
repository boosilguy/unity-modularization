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
        private void AcceptAttribute(ScriptFoundation scriptFoundation, SettingAttribute attribute, MemberInfo member, object value) => attribute.Accept.Invoke(scriptFoundation, member, value);

        /// <summary>
        /// 대상 클래스에 사용된 SettingAttribute를 인식하여 값을 대입합니다.
        /// </summary>
        /// <param name="scriptFoundation">ScriptFoundation을 구현한 클래스</param>
        public void AcceptScriptFoundation(ScriptFoundation scriptFoundation)
        {
            Initialize();

            List<MemberInfo> members = scriptFoundation.GetType()
                .GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes(typeof(SettingAttribute), true).Any())
                .ToList<MemberInfo>();

            if (members.Count == 0)
                return;

            foreach (MemberInfo member in members)
            {
                var settingAttribute = member.GetCustomAttribute<SettingAttribute>();
                var itemInRuntime = ItemsInRuntime.FirstOrDefault(item => item.Name == settingAttribute.Name);
                AcceptAttribute(scriptFoundation, settingAttribute, member, itemInRuntime.Value);

                if (!itemsInScriptFoundation.ContainsKey(scriptFoundation))
                    itemsInScriptFoundation.Add(scriptFoundation, new List<SettingAttribute>());

                itemsInScriptFoundation[scriptFoundation].Add(settingAttribute);
            }
        }
    }

}
