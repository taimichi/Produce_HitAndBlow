using UnityEditor;

[CustomEditor(typeof(NumberInput))]
public class CustomIncpectorTest : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty typeProp = serializedObject.FindProperty("numButton");
        EditorGUILayout.PropertyField(typeProp);

        var type = (NumberInput.ButtonProperty)typeProp.enumValueIndex;

        /* ���ʕ\�� */
        // �I�𒆂̕\���I�u�W�F�N�g
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nowSelectObj"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anim"));

        // enum�̒l�ɂ���ē���̕\�����s��
        if (type == NumberInput.ButtonProperty.Number)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("number"));
        }

        serializedObject.ApplyModifiedProperties();
    }

}
