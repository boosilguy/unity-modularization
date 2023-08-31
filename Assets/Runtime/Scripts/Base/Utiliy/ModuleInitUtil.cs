using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ModuleInitUtil
{
    public static void InitModules(ModuleManager moduleManager)
    {
        moduleManager.modules.Clear();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> types = new List<Type>();
        foreach(var assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ModuleFoundation)) && Attribute.IsDefined(t, typeof(SerializableAttribute))));
        }

        // 작업하다 말았음
        foreach (var type in types)
        {
            var objects = Resources.FindObjectsOfTypeAll(type);

            foreach (var obj in objects)
            {
                Debug.Log($"[Prefix] {obj}");
            }

            var moduleInstance = Activator.CreateInstance(type) as ModuleFoundation;
            moduleManager.modules.Add(moduleInstance);
            Debug.Log(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("Initialize module : ", Color.white),
                    new ColorfulText(moduleInstance.GetType().FullName, Color.yellow)));
        }

        EditorUtility.SetDirty(moduleManager);
        Debug.Log(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("Initialized module count : ", Color.white),
                    new ColorfulText(types.Count.ToString(), Color.yellow)));
    }
}