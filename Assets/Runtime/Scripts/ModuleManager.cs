using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ScriptExcutionOrder(-100)]
public class ModuleManager : MonoBehaviour
{
    static ModuleManager instance = null;
    public static ModuleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ModuleManager>();
                
                if (instance == null)
                {
                    var moduleManager = GameObject.Find(ModuleConstant.ModuleManagerName);
                    if (moduleManager == null)
                        instance = new GameObject(ModuleConstant.ModuleManagerName).AddComponent<ModuleManager>();
                    else
                        instance = moduleManager.AddComponent<ModuleManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] public List<ModuleFoundation> modules = new List<ModuleFoundation>();
    Dictionary<string, ModuleFoundation> modulesDic = new Dictionary<string, ModuleFoundation>();

    public ModuleFoundation GetModule(string moduleName)
    {
        if (modulesDic.ContainsKey(moduleName))
            return modulesDic[moduleName];
        else
        {
            foreach (var module in modules)
            {
                if (module != null && (module.GetType().FullName == moduleName || module.GetType().Name == moduleName))
                {
                    modulesDic[moduleName] = module;
                    return module;
                }
            }

            Debug.LogError(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("Invalid module name", Color.red),
                    new ColorfulText(": Cannot get module", Color.white),
                    new ColorfulText(moduleName, Color.yellow)));

            return null;
        }
    }

    public T GetModule<T>() where T : ModuleFoundation
    {
        foreach(var module in modules)
        {
            if (module is T genericModule) 
                return genericModule;
        }

        Debug.LogError(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("Invalid module name", Color.red),
                    new ColorfulText(": Cannot get module", Color.white),
                    new ColorfulText(typeof(T).Name, Color.yellow),
                    new ColorfulText(" type", Color.white)));

        return null;
    }

    private void Update()
    {
        foreach(var module in modules)
            module?.Process();
    }

    private void OnDestroy()
    {
        foreach (var module in modules)
            module?.Process(ETransition.Destroy);

        instance = null;
    }
}
