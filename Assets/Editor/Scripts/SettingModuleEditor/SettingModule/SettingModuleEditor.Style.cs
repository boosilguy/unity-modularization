using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class SettingModuleEditor
{
    GUIStyle style_Main_Header;
    GUIStyle style_Main_ViewerContainer;

    GUIStyle style_SettingTag_Header;
    GUIStyle style_Item_ViewerContainer;

    GUIStyle style_Item_HeaderContainer;
    GUIStyle style_Item_RegisteredHeader;
    GUIStyle style_Item_NameHeader;
    GUIStyle style_Item_TypeHeader;
    GUIStyle style_Item_ValueHeader;
    GUIStyle style_Item_UpdateBtnHeader;

    GUIStyle style_Item_Registered;
    GUIStyle style_Item_Unregistered;
    GUIStyle style_Item_Name;
    GUIStyle style_Item_Type;
    GUIStyle style_Item_UpdateBtn;

    private void StyleInitialize()
    {
        #region Main component
        style_Main_Header = new GUIStyle(GUI.skin.box)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            fixedWidth = EditorGUIUtility.currentViewWidth - 10f,
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(5, 5, 5, 5),
        };

        style_Main_ViewerContainer = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 10, 10),
            margin = new RectOffset(5, 5, 5, 5),
        };

        style_SettingTag_Header = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            margin = new RectOffset(0, 0, 10, 5),
            fontSize = 15,
            fontStyle = FontStyle.Bold,
        };
        #endregion

        #region Container
        style_Item_ViewerContainer = new GUIStyle(GUI.skin.box)
        {
            margin = new RectOffset(10, 10, 5, 10),
        };
        style_Item_ViewerContainer.normal.background = Texture2D.linearGrayTexture;

        style_Item_HeaderContainer = new GUIStyle(GUI.skin.box)
        {
            margin = new RectOffset(20, 26, 0, 0),
        };
        style_Item_HeaderContainer.normal.background = Texture2D.linearGrayTexture;
        #endregion

        #region Item Header
        style_Item_RegisteredHeader = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_REGISTER_WIDTH,

            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
        };
        style_Item_NameHeader = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_NAME_WIDTH,

            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
        };
        style_Item_TypeHeader = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_TYPE_WIDTH,

            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
        };
        style_Item_ValueHeader = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_VALUE_WIDTH,
            
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
        };
        style_Item_UpdateBtnHeader = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_BUTTON_WIDTH,

            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
        };
        #endregion

        #region Item
        style_Item_Registered = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_REGISTER_WIDTH,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 8,
        };
        Color style_Item_Registered_Color;
        ColorUtility.TryParseHtmlString("#00FF80", out style_Item_Registered_Color);
        style_Item_Registered.normal.textColor = style_Item_Registered_Color;

        style_Item_Unregistered = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_REGISTER_WIDTH,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 8,
        };
        Color style_Item_Unregistered_Color;
        ColorUtility.TryParseHtmlString("#FF4040", out style_Item_Unregistered_Color);
        style_Item_Unregistered.normal.textColor = style_Item_Unregistered_Color;

        style_Item_Name = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_NAME_WIDTH,
        };
        style_Item_Type = new GUIStyle(GUI.skin.label)
        {
            fixedWidth = STYLE_ITEM_TYPE_WIDTH,
        };
        style_Item_UpdateBtn = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = STYLE_ITEM_BUTTON_WIDTH,
        };
        #endregion

    }
}
