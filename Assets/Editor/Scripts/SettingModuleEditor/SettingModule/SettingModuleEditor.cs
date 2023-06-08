using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using setting;
using UnityEngine.UI;
using System;
using UnityEditor.EditorTools;

public partial class SettingModuleEditor : EditorWindow
{
    private Vector2 scrollPosition_SettingViewer = Vector2.zero;
    private Dictionary<string, List<SettingAttribute>> settingsFilteredTag = new Dictionary<string, List<SettingAttribute>>();
    private Dictionary<string, object> inputPlaceholder = new Dictionary<string, object>();

    private AddSettingForm addSettingForm = new AddSettingForm();

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
        Rect windowRect = new Rect(new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2), new Vector2(1080, 1080));
        EditorWindow.GetWindowWithRect<SettingModuleEditor>(windowRect, true, "Setting Module Editor");
    }

    private void OnEnable()
    {
        Initialize();
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
            scrollPosition_SettingViewer = EditorGUILayout.BeginScrollView(scrollPosition_SettingViewer, GUIStyle.none, GUI.skin.verticalScrollbar);
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

        EditorGUILayout.BeginVertical(style_Main_ViewerContainer);
        {
            OnDrawUtilityColumnHeader();
            OnDrawUtility();
        }
        EditorGUILayout.EndVertical();
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
        var onlyInScript = SettingModule.ItemsInNew.Where(x => x == setting).Any();

        ActionViaSettingType(setting,
            (intSetting) => OnDrawIntSettingItem(intSetting, onlyInScript),
            (floatSetting) => OnDrawFloatSettingItem(floatSetting, onlyInScript),
            (stringSetting) => OnDrawStringSettingItem(stringSetting, onlyInScript),
            (boolSetting) => OnDrawBoolSettingItem(boolSetting, onlyInScript));

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void OnDrawUtilityColumnHeader()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("Type");
            GUILayout.Label("Tag");
            GUILayout.Label("Name");
            GUILayout.Label("Value");
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnDrawUtility()
    {
        EditorGUILayout.BeginHorizontal();
        {
            addSettingForm.SettingType = (ESettingType)EditorGUILayout.EnumPopup(addSettingForm.SettingType);
            addSettingForm.SettingTag = EditorGUILayout.TextField(addSettingForm.SettingTag);
            addSettingForm.SettingName = EditorGUILayout.TextField(addSettingForm.SettingName);
            
            ActionViaSettingType(addSettingForm.SettingType,
                () => addSettingForm.SettingIntValue = EditorGUILayout.IntField(addSettingForm.SettingIntValue),
                () => addSettingForm.SettingFloatValue = EditorGUILayout.FloatField(addSettingForm.SettingFloatValue),
                () => addSettingForm.SettingStringValue = EditorGUILayout.TextField(addSettingForm.SettingStringValue),
                () => addSettingForm.SettingBoolValue = EditorGUILayout.Toggle(addSettingForm.SettingBoolValue));
            
            if (GUILayout.Button("Add"))
            {
                if (String.IsNullOrEmpty(addSettingForm.SettingName))
                {
                    EditorUtility.DisplayDialog(ADD_ALERT_TITLE, ADD_ALERT_NONAME, DIALOG_CONFIRM);
                }
                else
                {
                    if (SettingModule.ItemsInRuntime.Where(x => x.Name == addSettingForm.SettingName).Count() != 0)
                    {
                        EditorUtility.DisplayDialog(ADD_ALERT_TITLE, ADD_ALERT_MULTIPLENAMES, DIALOG_CONFIRM);
                    }
                    else
                    {
                        ActionViaSettingType(addSettingForm.SettingType,
                            () => SettingModule.ItemsInNew.Add(new IntAttribute(addSettingForm.SettingName, addSettingForm.SettingTag, addSettingForm.SettingIntValue)),
                            () => SettingModule.ItemsInNew.Add(new FloatAttribute(addSettingForm.SettingName, addSettingForm.SettingTag, addSettingForm.SettingFloatValue)),
                            () => SettingModule.ItemsInNew.Add(new StringAttribute(addSettingForm.SettingName, addSettingForm.SettingTag, addSettingForm.SettingStringValue)),
                            () => SettingModule.ItemsInNew.Add(new BoolAttribute(addSettingForm.SettingName, addSettingForm.SettingTag, addSettingForm.SettingBoolValue)));
                        Initialize();
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            SettingModule.Save();
            Initialize(true);
        }
        EditorGUILayout.EndHorizontal();
    }

    private void Initialize(bool forceForSettingModule = false)
    {
        SettingModule.Initialize(forceForSettingModule);
        inputPlaceholder.Clear();

        settingsFilteredTag = SettingModule.ItemsInRuntime.GroupBy(item => item.Tag).ToDictionary(group => group.Key, group => group.ToList());
        settingsFilteredTag = settingsFilteredTag.Keys.OrderBy(tag => tag).ToDictionary(tag => tag, tag => settingsFilteredTag[tag]);

        addSettingForm = new AddSettingForm();
    }

    private void ActionViaSettingType(SettingAttribute s, Action<IntAttribute> @int, Action<FloatAttribute> @float, Action<StringAttribute> @string, Action<BoolAttribute> @bool)
    {
        switch (s)
        {
            case IntAttribute intAttribute:
                @int.Invoke(intAttribute);
                break;
            case FloatAttribute floatAttribute:
                @float.Invoke(floatAttribute); 
                break;
            case StringAttribute stringAttribute:
                @string.Invoke(stringAttribute);
                break;
            case BoolAttribute boolAttribute:
                @bool.Invoke(boolAttribute);
                break;
        }
    }

    private void ActionViaSettingType(ESettingType type, Action @int, Action @float, Action @string, Action @bool)
    {
        switch (type)
        {
            case ESettingType.Int:
                @int.Invoke();
                break;
            case ESettingType.Float:
                @float.Invoke();
                break;
            case ESettingType.String:
                @string.Invoke();
                break;
            case ESettingType.Bool:
                @bool.Invoke();
                break;
        }
    }
}
