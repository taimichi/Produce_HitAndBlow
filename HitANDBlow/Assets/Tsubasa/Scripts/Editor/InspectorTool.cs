using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Button))]
public class InspectorTool : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty typeProp = serializedObject.FindProperty("button");
        EditorGUILayout.PropertyField(typeProp);

        var type = (Button.ButtonProperty)typeProp.enumValueIndex;

        /* 共通表示 */
        // 選択中の表示オブジェクト
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nowSelectObj"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anim"));

        // enumの値によって特定の表示を行う
        if (type == Button.ButtonProperty.OpenTutorial)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("test"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
