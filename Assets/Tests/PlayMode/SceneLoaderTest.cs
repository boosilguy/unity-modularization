using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using setting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneLoaderTest
{
    private GameObject moduleManagerGameObject;
    private ModuleManager moduleManager;
    public static string[] sceneName = { "SceneA" };
    public static string[] moduleFoundation = { "SettingManagerModule" };

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return null;
        moduleManagerGameObject = new GameObject("ModuleManager");
        moduleManager = moduleManagerGameObject.AddComponent<ModuleManager>();
    }

    [UnityTest]
    public IEnumerator SceneChange([ValueSource(nameof(sceneName))] string sceneName)
    {
        //ModuleInitUtil.InitModules(moduleManager);

        Assert.IsTrue(moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject);
        Debug.Log($"InitializeScene => {moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject}");

        yield return null;

        SceneManager.LoadScene(sceneName);
        Assert.IsTrue(moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject);
        Debug.Log($"{sceneName} => {moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject}");
    }
}
