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

        /* ���ʕ\�� */
        // �I�𒆂̕\���I�u�W�F�N�g
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nowSelectObj"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anim"));

        // enum�̒l�ɂ���ē���̕\�����s��
        if (type == Button.ButtonProperty.OpenTutorial)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("test"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
