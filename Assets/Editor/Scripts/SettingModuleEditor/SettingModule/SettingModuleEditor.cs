using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using setting;

public partial class SettingModuleEditor : EditorWindow
{
    private Vector2 scrollPosition_SettingViewer = Vector2.zero;
    private Dictionary<string, List<SettingAttribute>> settingsFilteredTag = new Dictionary<string, List<SettingAttribute>>();
    private Dictionary<string, object> inputPlaceholder = new Dictionary<string, object>();

    private SettingModuleManager settingModuleManager;
    private SettingModuleManager SettingModule
    {
        get
        {
            if (settingModuleManager == null)
                settingModuleManager = ModuleManager.Instance?.GetModule("SettingModuleManager") as SettingModuleManager;
            return settingModuleManager;
        }
    }

    private List<string> settingTags = new List<string>();

    [MenuItem("UnityModularization/Setting Module Editor")]
    static void OpenWindow()
    {
        Rect windowRect = new Rect(new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2), new Vector2(1080, 1080));
        EditorWindow.GetWindowWithRect<SettingModuleEditor>(windowRect, true, "Setting Module Editor");
    }

    private void OnEnable()
    {
        settingsFilteredTag = SettingModule.ItemsInRuntime.GroupBy(item => item.Tag).ToDictionary(group => group.Key, group => group.ToList());
        settingsFilteredTag = settingsFilteredTag.Keys.OrderBy(tag => tag).ToDictionary(tag => tag, tag => settingsFilteredTag[tag]);
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        StyleInitialize();

        GUILayout.Label("Setting Module Editor", style_Main_Header);

        #region Setting List
        EditorGUILayout.BeginVertical(style_Main_ViewerContainer);
        {
            OnDrawColumnHeader();
            scrollPosition_SettingViewer = EditorGUILayout.BeginScrollView(scrollPosition_SettingViewer);
            {
                foreach (var item in settingsFilteredTag)
                {
                    EditorGUILayout.BeginVertical(style_Item_ViewerContainer);
                    GUILayout.Label(item.Key, style_SettingTag_Header);
                    {
                        foreach (var setting in settingsFilteredTag[item.Key])
                        {
                            OnDrawSettingItem(setting);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

    private void OnDrawColumnHeader()
    {
        EditorGUILayout.BeginHorizontal(style_Item_HeaderContainer);
        GUILayout.FlexibleSpace();
        {
            GUILayout.Label("Register state", style_Item_RegisteredHeader);
            GUILayout.Label("Name", style_Item_NameHeader);
            GUILayout.Label("Type", style_Item_TypeHeader);
            GUILayout.Label("Value", style_Item_ValueHeader);
            GUILayout.Label("Update", style_Item_UpdateBtnHeader);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void OnDrawSettingItem(SettingAttribute setting)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var onlyInScript = SettingModule.ItemsInNew.Contains(setting) ? true : false;

        switch (setting)
        {
            case IntAttribute intAttribute:
                OnDrawIntSettingItem(intAttribute, onlyInScript);
                break;
            case FloatAttribute floatAttribute:
                OnDrawFloatSettingItem(floatAttribute, onlyInScript);
                break;
            case StringAttribute stringAttribute:
                OnDrawStringSettingItem(stringAttribute, onlyInScript);
                break;
            case BoolAttribute boolAttribute:
                OnDrawBoolSettingItem(boolAttribute, onlyInScript);
                break;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    
}
