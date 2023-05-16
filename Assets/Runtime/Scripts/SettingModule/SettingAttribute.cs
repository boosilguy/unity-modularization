using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public delegate void Accept(ScriptFoundation scriptFoundation, MemberInfo member, object value);

public class SettingAttribute : Attribute
{
    public string Name { get; set; }
    public string Tag { get; set; }

    [JsonIgnore] internal ScriptFoundation ScriptFoundation { get; set; }
    [JsonIgnore] internal MemberInfo Member { get; set; }
    [JsonIgnore] internal Accept Accept { get; set; }

    public SettingAttribute (string name, string tag = SettingModuleConstValue.DEFAULT_TAG)
    {
        Name = name;
        Tag = tag;
        Accept = AcceptField;
    }

    internal virtual void AcceptField(ScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        FieldInfo fieldInfo = member as FieldInfo;
        if (fieldInfo != null)
        {
            if (fieldInfo.FieldType == value.GetType())
            {
                fieldInfo.SetValue(scriptFoundation, value);
                ScriptFoundation = scriptFoundation;
                Member = member;
            }
            else
            {
                Debug.LogError(RichTextUtil.GetColorfulText(
                    new ColorfulText("Invalid assignment", Color.red),
                    new ColorfulText(": Mismatch data types ", Color.white),
                    new ColorfulText($"{member.Name} ({member.MemberType})", Color.yellow),
                    new ColorfulText(" and ", Color.white),
                    new ColorfulText($"{value} ({value.GetType()})", Color.yellow)));
                throw new ArgumentException();
            }
        }
        else
        {
            Debug.LogError(RichTextUtil.GetColorfulText(
                    new ColorfulText("Invalid assignment", Color.red),
                    new ColorfulText(": The field is null ", Color.white),
                    new ColorfulText(member.Name, Color.yellow)));
            throw new NullReferenceException();
        }
    }
}

public class IntAttribute : SettingAttribute
{
    public int Value { get; set; }

    [JsonConstructor]
    public IntAttribute (string name, string tag, int value) : base(name, tag)
    {
        Value = value;
    }

    internal override void AcceptField(ScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        base.AcceptField(scriptFoundation, member, value);
        Value = (int)value;
    }
}

public class FloatAttribute : SettingAttribute
{
    public float Value { get; set; }

    [JsonConstructor]
    public FloatAttribute(string name, string tag, float value) : base(name, tag)
    {
        Value = value;
    }
}

public class StringAttribute : SettingAttribute
{
    public string Value { get; set; }

    [JsonConstructor]
    public StringAttribute(string name, string tag, string value) : base(name, tag)
    {
        Value = value;
    }
}

public class BoolAttribute : SettingAttribute
{
    public bool Value { get; set; }

    [JsonConstructor]
    public BoolAttribute(string name, string tag, bool value) : base(name, tag)
    {
        Value = value;
    }
}