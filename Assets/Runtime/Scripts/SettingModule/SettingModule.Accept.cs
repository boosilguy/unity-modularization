﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public partial class SettingModuleManager : ModuleFoundation
{
    private void AcceptAttribute(ScriptFoundation scriptFoundation, SettingAttribute attribute, MemberInfo member) => attribute.Accept.Invoke(scriptFoundation, member);

    public void AcceptScriptFoundation(ScriptFoundation scriptFoundation)
    {
        List<MemberInfo> members = scriptFoundation.GetType()
            .GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.GetCustomAttributes(typeof(SettingAttribute), true).Any())
            .ToList<MemberInfo>();

        if (members.Count == 0)
            return;

        foreach (MemberInfo member in members)
        {
            var settingAttribute = member.GetCustomAttribute<SettingAttribute>();
            AcceptAttribute(scriptFoundation, settingAttribute, member);

            if (!ItemsInScriptFoundation.ContainsKey(scriptFoundation))
                ItemsInScriptFoundation.Add(scriptFoundation, new List<SettingAttribute>());

            ItemsInScriptFoundation[scriptFoundation].Add(settingAttribute);
        }
    }
}
