using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public static string[] moduleFoundation = { "SettingModuleManager" };

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        moduleManagerGameObject = new GameObject("ModuleManager");
        moduleManager = moduleManagerGameObject.AddComponent<ModuleManager>();
        ModuleInitUtil.InitModules(moduleManager);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SceneChange([ValueSource(nameof(sceneName))] string scene, [ValueSource(nameof(moduleFoundation))] string module)
    {
        Assert.IsTrue(moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject);
        Debug.Log($"InitializeScene => {moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject}");

        yield return null;

        SceneManager.LoadScene(scene);
        Assert.IsTrue(moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject);
        Debug.Log($"{scene} => {moduleManagerGameObject == GameObject.FindObjectOfType<ModuleManager>().gameObject}");

        Debug.Log($"Module count => {ModuleManager.Instance.modules.Count()}");
        foreach(var item in ModuleManager.Instance.modules)
        {
            Debug.Log($"Module name is {item.GetType().Name}");
            Debug.Log($"Module is null {item == null}");
        }

        Assert.IsNotNull(ModuleManager.Instance.GetModule(module));
        Debug.Log($"Module name => {ModuleManager.Instance.GetModule(module).GetType().Name}");
    }
}
