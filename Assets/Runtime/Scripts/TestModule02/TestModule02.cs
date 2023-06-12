using System;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public class TestModule02 : ModuleFoundation
{
    [Int(name: "InteagerValue1", tag: "ExampleA", 10)] int intValue;
    [Int(name: "InteagerValue2", tag: "ExampleA", 11)] int int2Value;
    [Int(name: "InteagerValue3", tag: "ExampleA", 12)] int int3Value;
    [Int(name: "InteagerValue4", tag: "ExampleA", 13)] int int4Value;
    [Int(name: "InteagerValue5", tag: "ExampleB", 14)] int int5Value;
    [Int(name: "InteagerValue6", tag: "ExampleB", 15)] int int6Value;
    [Int(name: "InteagerValue7", tag: "ExampleB", 16)] int int7Value;
    [Int(name: "InteagerValue8", tag: "ExampleB", 17)] int int8Value;
    [Float(name: "FloatValue1", tag: "ExampleA", 10.0f)] float floatValue;
    [Float(name: "FloatValue2", tag: "ExampleA", 11.0f)] float float2Value;
    [Float(name: "FloatValue3", tag: "ExampleA", 12.0f)] float float3Value;
    [Float(name: "FloatValue4", tag: "ExampleA", 13.0f)] float float4Value;
    [Float(name: "FloatValue5", tag: "ExampleB", 14.0f)] float float5Value;
    [Float(name: "FloatValue6", tag: "ExampleB", 15.0f)] float float6Value;
    [Float(name: "FloatValue7", tag: "ExampleB", 16.0f)] float float7Value;
    [Float(name: "FloatValue8", tag: "ExampleB", 17.0f)] float float8Value;
    [String(name: "StringValue", tag: "ExampleA", "10 string")] string stringValue;
    [String(name: "StringValue2", tag: "ExampleA", "11 string")] string string2Value;
    [String(name: "StringValue3", tag: "ExampleB", "12 string")] string string3Value;
    [String(name: "StringValue4", tag: "ExampleB", "13 string")] string string4Value;
    [String(name: "StringValue5", tag: "ExampleC", "14 string")] string string5Value;
    [String(name: "StringValue6", tag: "ExampleC", "15 string")] string string6Value;
    [String(name: "StringValue7", tag: "ExampleD", "16 string")] string string7Value;
    [String(name: "StringValue8", tag: "ExampleD", "17 string")] string string8Value;
    [Bool(name: "BoolValue", tag: "ExampleE", true)] bool boolValue;
    [Bool(name: "BoolValue2", tag: "ExampleE", true)] bool bool2Value;
    [Bool(name: "BoolValue3", tag: "ExampleF", true)] bool bool3Value;
    [Bool(name: "BoolValue4", tag: "ExampleF", true)] bool bool4Value;
    [Bool(name: "BoolValue5", tag: "ExampleG", true)] bool bool5Value;
    [Bool(name: "BoolValue6", tag: "ExampleG", true)] bool bool6Value;
    [Bool(name: "BoolValue7", tag: "ExampleH", true)] bool bool7Value;
    [Bool(name: "BoolValue8", tag: "ExampleH", true)] bool bool8Value;
    [Int(name: "OutsiderInt", tag: "Outsider", 1)] int outsiderInt;
    [Float(name: "OutsiderFloat", tag: "Outsider", 1.0f)] float outsiderFloat;
    [String(name: "OutsiderString", tag: "Outsider", "1 string")] string outsiderString;
    [Bool(name: "OutsiderBool", tag: "Outsider", false)] bool outsiderBool;
}
