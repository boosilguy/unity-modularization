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
    private ModuleManager moduleManager;
    public static string[] sceneName = { "SceneA" };
    public static string[] moduleFoundation = { "SettingModuleManager" };

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        moduleManager = ModuleManager.Instance;
        yield return null;
    }

    [UnityTest]
    public IEnumerator SceneChange([ValueSource(nameof(sceneName))] string scene, [ValueSource(nameof(moduleFoundation))] string module)
    {
        ModuleInitUtil.InitModules(moduleManager);

        Assert.IsTrue(moduleManager.gameObject);
        Debug.Log($"[SceneLoaderTest] InitializeScene => {moduleManager.gameObject.name}");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        moduleManager = ModuleManager.Instance;
        Assert.IsTrue(moduleManager.gameObject);
        Debug.Log($"[SceneLoaderTest] {scene} => {moduleManager.gameObject.name}");

        Debug.Log($"[SceneLoaderTest] Module count => {ModuleManager.Instance.modules.Count()}");
        foreach(var item in ModuleManager.Instance.modules)
        {
            if (item == null)
                Debug.Log($"[SceneLoaderTest] Module name is {item.GetType().Name} and, this is null");
            else
                Debug.Log($"[SceneLoaderTest] Module name is {item.GetType().Name} and, this is not null");
        }

        Assert.IsNotNull(ModuleManager.Instance.GetModule(module));
        Debug.Log($"[SceneLoaderTest] Module name => {moduleManager.GetModule(module).GetType().Name}");

        yield return null;
    }
}
