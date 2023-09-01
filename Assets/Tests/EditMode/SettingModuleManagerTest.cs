using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using setting;
using UnityEngine;
using UnityEngine.TestTools;

public class SettingModuleManagerTest
{
    SettingModuleManager settingModuleManager;

    [SetUp]
    public void SetUp()
    {
        settingModuleManager = ModuleManager.Instance.GetModule<SettingModuleManager>();
    }

    [Test]
    public void SettingListSimplePasses()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("========Runtime Setting List========");
        foreach (var item in settingModuleManager.ItemsInRuntime)
            sb.AppendLine($"{item.Name}\t({item.Tag})\t{item.Value}");
        sb.AppendLine("==============================");
        Debug.Log(sb.ToString());
    }

    [Test]
    [TestCase("FloatValue1", 11f)]
    [TestCase("FloatValue1", 12f)]
    [TestCase("FloatValue1", 13f)]
    public void SettingUpdateSimplePasses(string name, float value)
    {
        settingModuleManager.AddUpdateEvent(name, SettingExampleEvent);
        settingModuleManager.UpdateSetting(name, value);
        settingModuleManager.RemoveUpdateEvent(name, SettingExampleEvent);
    }

    private void SettingExampleEvent(object prev, object post)
    {
        Debug.Log($"{prev} (prev value) => {post} (post value)");
    }
}
