using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

[InitializeOnLoad]
public class HierarchyUtil
{
    static int iconSize = 15;
    static HierarchyUtil()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        
        #region Contour
        string pattern = "^-{2}";

        if (obj == null)
            return;

        if (Regex.IsMatch(obj.name, pattern))
        {
            var displayName = Regex.Replace(obj.name, pattern, "");
            var extendedSelectionRect = new Rect(selectionRect.x - 30, selectionRect.y, selectionRect.width + 50, selectionRect.height);

            DrawCustomRect(extendedSelectionRect, GetHexColor("#2D2D2D"), displayName, true, true);
        }
        #endregion

        #region Manager icon
        var manager = GameObject.FindObjectOfType<ModuleManager>();

        if (manager == null)
            return;

        if (obj.name == manager.name)
        {
            var iconRect = new Rect(selectionRect.x - iconSize, selectionRect.y, selectionRect.width, selectionRect.height);
            DrawIcon(iconRect, "Avatar Icon", iconSize);
        }
        #endregion
    }

    private static void DrawCustomRect(Rect rect, Color color, string dpName, bool bold, bool center)
    {
        EditorGUI.DrawRect(rect, color);
        EditorGUI.LabelField(rect, dpName, new GUIStyle(GUI.skin.label)
        {
            alignment = center ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft,
            fontStyle = bold ? FontStyle.Bold : FontStyle.Normal,
            normal = new GUIStyleState()
            {
                textColor = Color.white,
            }
        });
    }

    private static void DrawIcon(Rect iconRect, string icon, float iconWidth)
    {
        EditorGUIUtility.SetIconSize(new Vector2(iconWidth, iconWidth));
        var iconGUIContent = EditorGUIUtility.IconContent(icon);
        EditorGUI.LabelField(iconRect, iconGUIContent);
        EditorGUIUtility.SetIconSize(Vector2.zero);
    }

    private static Color GetHexColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;
        return Color.magenta;
    }
}
