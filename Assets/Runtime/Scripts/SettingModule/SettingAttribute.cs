using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;

public delegate void Accept(ScriptFoundation scriptFoundation, MemberInfo member);

public class SettingAttribute : Attribute
{
    public string Name { get; set; }
    public string Tag { get; set; }
    protected object Value { get; set; }

    [JsonIgnore] internal ScriptFoundation ScriptFoundation { get; set; }
    [JsonIgnore] internal MemberInfo Member { get; set; }
    [JsonIgnore] internal Accept Accept { get; set; }

    public SettingAttribute (string name, object value, string tag = SettingModuleConstValue.DEFAULT_TAG)
    {
        Name = name;
        Tag = tag;
        Value = value;
        Accept = AcceptField;
    }

    internal virtual void AcceptField(ScriptFoundation scriptFoundation, MemberInfo member)
    {
        FieldInfo fieldInfo = member as FieldInfo;
        if (fieldInfo != null)
        {
            Type fieldType = fieldInfo.FieldType;
            if (fieldType == Value.GetType())
            {
                fieldInfo.SetValue(scriptFoundation, Value);
                ScriptFoundation = scriptFoundation;
                Member = member;
            }
            else
            {
                throw new ArgumentException(RichTextUtil.GetColorfulText(
                    new ColorfulText("[Invalid assignment] ", Color.red),
                    new ColorfulText("Mismatch data types ", Color.white),
                    new ColorfulText($"{member.Name} ({member.MemberType})", Color.yellow),
                    new ColorfulText(" and ", Color.white),
                    new ColorfulText($"{Value} ({Value.GetType()})", Color.yellow)));
            }
        }
        else
        {
            throw new NullReferenceException(RichTextUtil.GetColorfulText(
                    new ColorfulText("[Invalid assignment] ", Color.red),
                    new ColorfulText("The field is null => ", Color.white),
                    new ColorfulText(member.Name, Color.yellow)));
        }
    }
}

public class IntAttribute : SettingAttribute
{
    public new int Value { get; set; }

    [JsonConstructor]
    public IntAttribute (string name, string tag, int value) : base(name, value, tag)
    {
        Value = value;
    }

    internal override void AcceptField(ScriptFoundation scriptFoundation, MemberInfo member)
    {
        base.AcceptField(scriptFoundation, member);
    }
}

public class FloatAttribute : SettingAttribute
{
    public new float Value { get; set; }

    [JsonConstructor]
    public FloatAttribute(string name, string tag, float value) : base(name, value, tag)
    {
        Value = value;
    }
}

public class StringAttribute : SettingAttribute
{
    public new string Value { get; set; }

    [JsonConstructor]
    public StringAttribute(string name, string tag, string value) : base(name, value, tag)
    {
        Value = value;
    }
}

public class BoolAttribute : SettingAttribute
{
    public new bool Value { get; set; }

    [JsonConstructor]
    public BoolAttribute(string name, string tag, bool value) : base(name, value, tag)
    {
        Value = value;
    }
}