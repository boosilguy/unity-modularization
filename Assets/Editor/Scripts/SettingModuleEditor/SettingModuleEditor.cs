using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class SettingModuleEditor : EditorWindow
{
    static private Vector2 windowPosition = new Vector2(Screen.currentResolution.width/2, Screen.currentResolution.height/2);
    static private Vector2 windowSize = new Vector2(1080, 1080);

    private Vector2 scrollPosition_SettingViewer = Vector2.zero;

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

    [MenuItem("UnityModularization/Setting Module Editor")]
    static void OpenWindow()
    {
        EditorWindow.GetWindowWithRect<SettingModuleEditor>(new Rect(windowPosition, windowSize), true, "Setting Module Editor");
    }

    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Label("Setting Module Editor", style_Main_Header);

        #region Setting List
        EditorGUILayout.BeginVertical(style_Main_ViewerContainer);
        {
            scrollPosition_SettingViewer = EditorGUILayout.BeginScrollView(scrollPosition_SettingViewer, style_Scroll_SettingViewer);
            {

            }
        }
        #endregion
    }
}
