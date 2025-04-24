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

        /* 共通表示 */
        // 選択中の表示オブジェクト
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nowSelectObj"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anim"));

        // enumの値によって特定の表示を行う
        if (type == NumberInput.ButtonProperty.Number)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("number"));
        }

        serializedObject.ApplyModifiedProperties();
    }

}
