using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class SettingModuleEditor
{
    private void OnDrawIntSettingItem(IntAttribute intAttribute, bool onlyInScript)
    {
        if (!inputPlaceholder.ContainsKey(intAttribute.Name))
            inputPlaceholder.Add(intAttribute.Name, intAttribute.Value);
        OnDrawOnlyInScript(onlyInScript);
        GUILayout.Label(intAttribute.Name, style_Item_Name);
        GUILayout.Label("Integer type", style_Item_Type);
        int inputField = (int)inputPlaceholder[intAttribute.Name];
        inputField = EditorGUILayout.IntField(inputField, GUILayout.Width(STYLE_ITEM_VALUE_WIDTH));
        inputPlaceholder[intAttribute.Name] = inputField;

        if (GUILayout.Button("Update", style_Item_UpdateBtn))
            SettingModule.UpdateSetting(intAttribute.Name, inputField);
    }

    private void OnDrawFloatSettingItem(FloatAttribute floatAttribute, bool onlyInScript)
    {
        if (!inputPlaceholder.ContainsKey(floatAttribute.Name))
            inputPlaceholder.Add(floatAttribute.Name, floatAttribute.Value);
        OnDrawOnlyInScript(onlyInScript);
        GUILayout.Label(floatAttribute.Name, style_Item_Name);
        GUILayout.Label("Float type", style_Item_Type);
        float inputField = (float)inputPlaceholder[floatAttribute.Name];
        inputField = EditorGUILayout.FloatField(inputField, GUILayout.Width(STYLE_ITEM_VALUE_WIDTH));
        inputPlaceholder[floatAttribute.Name] = inputField;

        if (GUILayout.Button("Update", style_Item_UpdateBtn))
            SettingModule.UpdateSetting(floatAttribute.Name, inputField);
    }

    private void OnDrawStringSettingItem(StringAttribute stringAttribute, bool onlyInScript)
    {
        if (!inputPlaceholder.ContainsKey(stringAttribute.Name))
            inputPlaceholder.Add(stringAttribute.Name, stringAttribute.Value);
        OnDrawOnlyInScript(onlyInScript);
        GUILayout.Label(stringAttribute.Name, style_Item_Name);
        GUILayout.Label("String type", style_Item_Type);
        string inputField = (string)inputPlaceholder[stringAttribute.Name];
        inputField = EditorGUILayout.TextField(inputField, GUILayout.Width(STYLE_ITEM_VALUE_WIDTH));
        inputPlaceholder[stringAttribute.Name] = inputField;

        if (GUILayout.Button("Update", style_Item_UpdateBtn))
            SettingModule.UpdateSetting(stringAttribute.Name, inputField);
    }

    private void OnDrawBoolSettingItem(BoolAttribute boolAttribute, bool onlyInScript)
    {
        if (!inputPlaceholder.ContainsKey(boolAttribute.Name))
            inputPlaceholder.Add(boolAttribute.Name, boolAttribute.Value);
        OnDrawOnlyInScript(onlyInScript);
        GUILayout.Label(boolAttribute.Name, style_Item_Name);
        GUILayout.Label("Boolean type", style_Item_Type);
        bool inputField = (bool)inputPlaceholder[boolAttribute.Name];
        inputField = EditorGUILayout.Toggle(inputField, GUILayout.Width(STYLE_ITEM_VALUE_WIDTH));
        inputPlaceholder[boolAttribute.Name] = inputField;

        if (GUILayout.Button("Update", style_Item_UpdateBtn))
            SettingModule.UpdateSetting(boolAttribute.Name, inputField);
    }

    private void OnDrawOnlyInScript(bool onlyInScript)
    {
        if (!onlyInScript)
            GUILayout.Label("Registered", style_Item_Registered);
        else
            GUILayout.Label("Unregistered", style_Item_Unregistered);
    }
}
