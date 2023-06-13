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
    public void SettingUpdateSimplePasses()
    {
        settingModuleManager.AddUpdateEvent("FloatValue1", SettingExampleEvent);
        settingModuleManager.UpdateSetting("FloatValue1", 11f);
    }

    [UnityTest]
    public IEnumerator SettingModuleManagerTestWithEnumeratorPasses()
    {
        yield return null;
    }

    private void SettingExampleEvent(object prev, object post)
    {
        Debug.Log($"{prev} (prev value) => {post} (post value)");
    }
}
