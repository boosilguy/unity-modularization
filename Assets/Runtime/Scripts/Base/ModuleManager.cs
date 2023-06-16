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

            return null;
        }
    }

    public T GetModule<T>() where T : ModuleFoundation
    {
        foreach(var module in modules)
        {
            if (module.GetType().Equals(typeof(T))) 
                return GetModule(module.GetType().Name) as T;
        }

        return null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
