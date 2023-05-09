using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAttribute : Attribute
{
    protected string Name { get; set; }
    protected string Tag { get; set; }
}

public class IntAttribute : SettingAttribute
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public int Value { get; set; }
    public IntAttribute(string name, string tag, int value)
    {
        Name = name;
        Tag = tag;
        Value = value;
    }
}