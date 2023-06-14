using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using setting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneLoaderTest
{
    private GameObject moduleManager;
    public static string[] sceneName = { "SceneA" };

    [UnityTest]
    public IEnumerator SceneChange([ValueSource(nameof(sceneName))] string sceneName)
    {
        moduleManager = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        moduleManager.AddComponent<ModuleManager>();

        Assert.IsTrue(moduleManager == GameObject.FindObjectOfType<ModuleManager>().gameObject);
        Debug.Log($"InitializeScene => {moduleManager == GameObject.FindObjectOfType<ModuleManager>().gameObject}");

        yield return null;

        SceneManager.LoadScene(sceneName);
        Assert.IsTrue(moduleManager == GameObject.FindObjectOfType<ModuleManager>().gameObject);
        Debug.Log($"{sceneName} => {moduleManager == GameObject.FindObjectOfType<ModuleManager>().gameObject}");
    }
}
