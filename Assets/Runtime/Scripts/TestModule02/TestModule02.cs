using System;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public class TestModule02 : ModuleFoundation
{
    [Int(name: "InteagerValue", tag: "Example", 10)] int value;
}
