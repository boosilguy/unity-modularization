using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public partial class SettingModuleEditor : EditorWindow
{
    static private Vector2 windowPosition = new Vector2(Screen.currentResolution.width/2, Screen.currentResolution.height/2);
    static private Vector2 windowSize = new Vector2(1080, 1080);

    private Vector2 scrollPosition_SettingViewer = Vector2.zero;
    private List<Vector2> scrollPositionList_SettingTagViewer = new List<Vector2>();

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
        EditorWindow.GetWindowWithRect<SettingModuleEditor>(new Rect(windowPosition, windowSize), true, "Setting Module Editor");
    }

    private void OnEnable()
    {

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
                var settingTags = settingModuleManager.ItemsInRuntime.Select(item => item.Tag).Distinct().OrderBy(tag => tag);

                foreach (var tag in settingTags)
                {
                    GUILayout.Label(tag, style_SettingTag_Header);
                    {

                    }
                }
            }
        }
        #endregion
    }
}
