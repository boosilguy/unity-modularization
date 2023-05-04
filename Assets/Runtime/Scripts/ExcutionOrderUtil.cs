using System;
using UnityEditor;

[InitializeOnLoad]
public class ExcutionOrderUtil
{
    static ExcutionOrderUtil()
    {
        foreach (MonoScript script in MonoImporter.GetAllRuntimeMonoScripts())
        {
            if (script.GetClass() != null)
            {
                foreach(var asset in Attribute.GetCustomAttributes(script.GetClass(), typeof(ScriptExcutionOrderAttribute)))
                {
                    var assetOrder = MonoImporter.GetExecutionOrder(script);
                    var assetNewOrder = ((ScriptExcutionOrderAttribute)asset).order;
                    if (assetOrder != assetNewOrder)
                    {
                        MonoImporter.SetExecutionOrder(script, assetNewOrder);
                    }
                }
            }
        }
    }
}

public class ScriptExcutionOrderAttribute : Attribute
{
    public int order;

    public ScriptExcutionOrderAttribute(int order)
    {
        this.order = order;
    }
}