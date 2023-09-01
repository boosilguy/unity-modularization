using System;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public delegate void Accept(IScriptFoundation scriptFoundation, MemberInfo member, object value);

public class SettingAttribute : Attribute
{
    public string Name { get; set; }
    public string Tag { get; set; } = SettingModuleConstValue.DEFAULT_TAG;
    public object Value { get; set; }

    [JsonIgnore] internal IScriptFoundation ScriptFoundation { get; set; }
    [JsonIgnore] internal MemberInfo Member { get; set; }
    [JsonIgnore] internal Accept Accept { get; set; }

    public SettingAttribute (string name, object value, string tag = SettingModuleConstValue.DEFAULT_TAG)
    {
        Name = name;
        Tag = tag;
        Value = value;
        Accept = AcceptField;
    }

    public virtual bool IsDefaultValue()
    {
        return Value == default;
    }

    internal virtual void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member)
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
                    new ColorfulText("Mismatch data types ", Color.white),
                    new ColorfulText($"{member.Name} ({member.MemberType})", Color.yellow),
                    new ColorfulText(" and ", Color.white),
                    new ColorfulText($"{Value} ({Value.GetType()})", Color.yellow)));
            }
        }
        else
        {
            throw new NullReferenceException(RichTextUtil.GetColorfulText(
                    new ColorfulText("The field is null => ", Color.white),
                    new ColorfulText(member.Name, Color.yellow)));
        }
    }

    internal virtual void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        FieldInfo fieldInfo = member as FieldInfo;
        if (fieldInfo != null)
        {
            Type fieldType = fieldInfo.FieldType;
            if (fieldType == value.GetType())
            {
                fieldInfo.SetValue(scriptFoundation, value);
                ScriptFoundation = scriptFoundation;
                Value = value;
                Member = member;
            }
            else
            {
                throw new ArgumentException(RichTextUtil.GetColorfulText(
                    new ColorfulText("Mismatch data types ", Color.white),
                    new ColorfulText($"{member.Name} ({member.MemberType})", Color.yellow),
                    new ColorfulText(" and ", Color.white),
                    new ColorfulText($"{value} ({value.GetType()})", Color.yellow)));
            }
        }
        else
        {
            throw new NullReferenceException(RichTextUtil.GetColorfulText(
                    new ColorfulText("The field is null => ", Color.white),
                    new ColorfulText(member.Name, Color.yellow)));
        }
    }
}

public class IntAttribute : SettingAttribute
{
    public new int Value { get; set; } = default;

    [JsonConstructor]
    public IntAttribute (string name, string tag, int value) : base(name, value, tag)
    {
        Value = value;
    }

    public override bool IsDefaultValue()
    {
        return base.IsDefaultValue();
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member)
    {
        base.AcceptField(scriptFoundation, member);
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        base.AcceptField(scriptFoundation, member, (int)value);
    }
}

public class FloatAttribute : SettingAttribute
{
    public new float Value { get; set; } = default;

    [JsonConstructor]
    public FloatAttribute(string name, string tag, float value) : base(name, value, tag)
    {
        Value = value;
    }

    public override bool IsDefaultValue()
    {
        return base.IsDefaultValue();
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member)
    {
        base.AcceptField(scriptFoundation, member);
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        base.AcceptField(scriptFoundation, member, (float)value);
    }
}

public class StringAttribute : SettingAttribute
{
    public new string Value { get; set; } = default;

    [JsonConstructor]
    public StringAttribute(string name, string tag, string value) : base(name, value, tag)
    {
        Value = value;
    }

    public override bool IsDefaultValue()
    {
        return base.IsDefaultValue();
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member)
    {
        base.AcceptField(scriptFoundation, member);
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        base.AcceptField(scriptFoundation, member, (string)value);
    }
}

public class BoolAttribute : SettingAttribute
{
    public new bool Value { get; set; } = default;

    [JsonConstructor]
    public BoolAttribute(string name, string tag, bool value) : base(name, value, tag)
    {
        Value = value;
    }

    public override bool IsDefaultValue()
    {
        return base.IsDefaultValue();
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member)
    {
        base.AcceptField(scriptFoundation, member);
    }

    internal override void AcceptField(IScriptFoundation scriptFoundation, MemberInfo member, object value)
    {
        base.AcceptField(scriptFoundation, member, (bool)value);
    }
}