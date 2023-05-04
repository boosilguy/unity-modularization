using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(ModuleManager))]
public class ModuleManagerEditor : Editor
{
    ModuleManager manager = null;
    SerializedProperty modules = null;

    private ReorderableList list;

    private void OnEnable()
    {
        manager = target as ModuleManager;
        modules = serializedObject.FindProperty("modules");
        list = new ReorderableList(serializedObject, modules, true, true, false, true)
        {
            drawHeaderCallback = OnDrawHeader,
            drawElementCallback = OnDrawElement,
            onReorderCallback = OnReorder,
            elementHeightCallback = CalculateCallHeight
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (modules.isExpanded)
        {
            list.DoLayoutList();
        }
        
        
        if (GUILayout.Button("Reload modules"))
        {
            ModuleInitUtil.InitModules(manager);
        }

        serializedObject.ApplyModifiedProperties();

        
        /*
        EditorGUILayout.PropertyField(modules, new GUIContent("Module List"), GUILayout.Height(EditorGUIUtility.singleLineHeight));
        if (modules.isExpanded)
        {
            list.DoLayoutList();

            if (Event.current.type == EventType.MouseDrag && list.index >= 0)
            {
                int selectedIndex = GetSelectedIndexWithFallback(list);
                if (selectedIndex >= 0)
                {
                    list.ele = selectedIndex;
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { serializedObject.targetObject };
                    DragAndDrop.StartDrag("ReorderableList");
                    Event.current.Use();
                }
            }
        }

        if (GUILayout.Button("Reload modules"))
        {
            ModuleInitUtil.InitModules(manager);
        }

        serializedObject.ApplyModifiedProperties();
        */
    }

    void OnDrawHeader(Rect rect)
    {
        string nameField = "Module List";
        EditorGUI.LabelField(rect, nameField);
    }

    void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        string nameField = manager.modules[index].GetType().FullName;

        EditorGUI.LabelField(rect, nameField);
    }

    void OnReorder(ReorderableList list)
    {
        serializedObject.ApplyModifiedProperties();
    }

    float CalculateCallHeight(int idx)
    {
        return EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(idx));
    }
}