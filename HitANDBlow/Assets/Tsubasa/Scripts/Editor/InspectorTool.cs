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
        if (type == Button.ButtonProperty.GoTitle   ||
            type == Button.ButtonProperty.Yes_Title ||
            type == Button.ButtonProperty.No_Title ) 
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GoTitlePanel"));
        }

        else if(type==Button.ButtonProperty.Settings)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MenuCanvas"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
