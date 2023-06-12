using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using setting;
using System;

public partial class SettingModuleEditor : EditorWindow
{
    private Vector2 scrollPosition_SettingViewer = Vector2.zero;
    private Dictionary<string, List<SettingAttribute>> settingsFilteredTag = new Dictionary<string, List<SettingAttribute>>();
    private Dictionary<string, object> inputPlaceholder = new Dictionary<string, object>();
    private List<SettingRemoveTarget> removeTargets = new List<SettingRemoveTarget> ();

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

        EditorGUILayout.Space(25);

        GUILayout.Label("Setting Utility", style_Main_Header);

        EditorGUILayout.BeginVertical(style_Utility_Container);
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
            GUILayout.Label("Remove", style_Item_RemoveTarget);
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

        EditorGUI.BeginDisabledGroup(onlyInScript);
        {
            var target = removeTargets.Where(item => item.item == setting).First();
            bool remove = EditorGUILayout.Toggle(target.remove, GUILayout.Width(STYLE_ITEM_REMOVE_WIDTH));
            target.remove = remove;
        }
        EditorGUI.EndDisabledGroup();

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
        EditorGUILayout.BeginHorizontal(style_Item_HeaderContainer);
        GUILayout.FlexibleSpace();
        {
            GUILayout.Label("Type", style_Utility_TypeHeader);
            GUILayout.Label("Tag", style_Utility_TagHeader);
            GUILayout.Label("Name", style_Utility_NameHeader);
            GUILayout.Label("Value", style_Utility_ValueHeader);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void OnDrawUtility()
    {
        EditorGUILayout.BeginHorizontal(style_Item_HeaderContainer);
        GUILayout.FlexibleSpace();
        {
            addSettingForm.SettingType = (ESettingType)EditorGUILayout.EnumPopup(addSettingForm.SettingType, style_Utility_Type);
            addSettingForm.SettingTag = EditorGUILayout.TextField(addSettingForm.SettingTag, style_Utility_Tag);
            addSettingForm.SettingName = EditorGUILayout.TextField(addSettingForm.SettingName, style_Utility_Name);
            
            ActionViaSettingType(addSettingForm.SettingType,
                () => addSettingForm.SettingIntValue = EditorGUILayout.IntField(addSettingForm.SettingIntValue, style_Utility_IntValue),
                () => addSettingForm.SettingFloatValue = EditorGUILayout.FloatField(addSettingForm.SettingFloatValue, style_Utility_FloatValue),
                () => addSettingForm.SettingStringValue = EditorGUILayout.TextField(addSettingForm.SettingStringValue, style_Utility_StringValue),
                () => addSettingForm.SettingBoolValue = EditorGUILayout.Toggle(addSettingForm.SettingBoolValue, style_Utility_BoolValue));
            
            if (GUILayout.Button("Add", style_utility_AddBtn))
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
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        OnDrawSave();
        EditorGUILayout.EndHorizontal();
    }

    private void OnDrawSave()
    {
        if (GUILayout.Button("Save"))
        {
            foreach (var target in removeTargets)
                if (target.remove)
                    SettingModule.RemoveSetting(target.item.Name);

            SettingModule.Save();
            Initialize(true);
        }
    }

    private void Initialize(bool forceForSettingModule = false)
    {
        SettingModule.Initialize(forceForSettingModule);
        inputPlaceholder.Clear();
        
        settingsFilteredTag = SettingModule.ItemsInRuntime.GroupBy(item => item.Tag).ToDictionary(group => group.Key, group => group.ToList());
        settingsFilteredTag = settingsFilteredTag.Keys.OrderBy(tag => tag).ToDictionary(tag => tag, tag => settingsFilteredTag[tag]);
        removeTargets = settingsFilteredTag.Values.SelectMany(item => item).Select(item => new SettingRemoveTarget(item, false)).ToList();

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
            default:
                throw new NotImplementedException(RichTextUtil.GetColorfulText(
                    new ColorfulText("Not implemented type ", Color.white),
                    new ColorfulText($"{s.GetType()} ", Color.yellow),
                    new ColorfulText("is not implemented", Color.white)));
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
            default:
                throw new NotImplementedException(RichTextUtil.GetColorfulText(
                    new ColorfulText("Not implemented type ", Color.white),
                    new ColorfulText($"{type} ", Color.yellow),
                    new ColorfulText("is not implemented", Color.white)));
        }
    }
}
