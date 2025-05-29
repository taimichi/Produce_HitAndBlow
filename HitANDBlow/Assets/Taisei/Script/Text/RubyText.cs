using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RubyText : Text, IMeshModifier
{
    [TextArea(3, 10)] [SerializeField] protected string m_RubyText = string.Empty;
    [SerializeField] protected float m_RubyFontScale = 0.5f;

    RubyString strCached;

    public override string text
    {
        set
        {
            m_RubyText = value;
            if (m_RubyText is null)
            {
                m_RubyText = "";
            }
            strCached = RubyString.Parse(value);
            base.text = strCached?.BaseString ?? null;
            RequetRubyTexture();
        }
        get => base.text;
    }
    public RubyString rubyText
    {
        set
        {
            m_RubyText = value?.RawString;
            if (m_RubyText is null)
            {
                m_RubyText = "";
            }
            strCached = value;
            base.text = strCached?.BaseString ?? null;
            RequetRubyTexture();
        }
        get => strCached;
    }
    void RequetRubyTexture()
    {
        int baseFontSize = fontSize;
        int rubyFontSize = Mathf.RoundToInt(baseFontSize * m_RubyFontScale);
        for (int di = 0; di < strCached.Data.Count; ++di)
        {
            RubyString.Pair sr = strCached.Data[di];
            if (sr.Ruby != null)
            {
                font.RequestCharactersInTexture(sr.Ruby, rubyFontSize);
            }
            font.RequestCharactersInTexture(sr.Str, baseFontSize);
        }
    }
    public void ModifyMesh(Mesh mesh)
    {
        throw new NotImplementedException();
    }

    public void ModifyMesh(VertexHelper verts)
    {
        if (strCached == null)
        {
            return;
        }
        var stream = ListPool<UIVertex>.Get();
        verts.GetUIVertexStream(stream);

        Modify(ref stream);

        verts.Clear();
        verts.AddUIVertexTriangleStream(stream);

        ListPool<UIVertex>.Release(stream);
    }

    void Modify(ref List<UIVertex> stream)
    {
        string targetString = PureString(strCached.BaseString);

        int textLength = targetString.Length;
        int[] baseIndexToMeshIndex = new int[textLength];
        {
            int charMeshIndex = 0;
            for (int charIndex = 0; charIndex < textLength; ++charIndex)
            {
                char c = targetString[charIndex];
                if (c != '　' && char.IsWhiteSpace(c))
                {
                    continue;
                }
                baseIndexToMeshIndex[charIndex] = charMeshIndex;
                ++charMeshIndex;
            }
        }
        int[] startIndexMap = new int[strCached.Data.Count];
        string[] pureStrMap = new string[strCached.Data.Count];
        {
            int charIndex = 0;
            for (int i = 0; i < strCached.Data.Count; ++i)
            {
                startIndexMap[i] = charIndex;
                string str = PureString(strCached.Data[i].Str);
                pureStrMap[i] = str;
                charIndex += str.Length;
            }
        }

        // ルビの頂点追加
        int baseFontSize = fontSize;
        int rubyFontSize = Mathf.RoundToInt(baseFontSize * m_RubyFontScale);
        for (int di = 0; di < strCached.Data.Count; ++di)
        {
            RubyString.Pair sr = strCached.Data[di];
            if (sr.Ruby is null)
            {
                continue;
            }
            string pureStr = pureStrMap[di];
            float baseRegionWidth = 0;
            float[] baseOffsMap = new float[pureStr.Length];
            float[] baseOffsMapLeft = new float[pureStr.Length];
            for (int i = 0; i < pureStr.Length; ++i)
            {
                if (font.GetCharacterInfo(pureStr[i], out CharacterInfo characterInfoBase, baseFontSize))
                {
                    float advance = characterInfoBase.advance;
                    baseOffsMapLeft[i] = baseRegionWidth;
                    baseRegionWidth += advance;
                    baseOffsMap[i] = baseRegionWidth - advance / 2.0f;
                }
            }
            float rubyRegionWidth = rubyFontSize * sr.Ruby.Length;
            float regionWidthDiff = -(rubyRegionWidth - baseRegionWidth) / 2.0f;
            for (int si = 0; si < sr.Ruby.Length; ++si)
            {
                if (char.IsWhiteSpace(sr.Ruby[si]))
                {
                    continue;
                }
                int baseIndex = 0;
                {
                    float threshold = rubyFontSize * si + rubyFontSize / 2.0f + regionWidthDiff;
                    for (int i = pureStr.Length - 1; i >= 0; --i)
                    {
                        if (i == 0)
                        {
                            baseIndex = startIndexMap[di] + 0;
                            break;
                        }
                        char c = pureStr[i];
                        if (c != '　' && char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        if (baseOffsMapLeft[i] < threshold)
                        {
                            baseIndex = startIndexMap[di] + i;
                            break;
                        }
                    }
                }
                if (baseIndex < textLength)
                {
                    char bc = targetString[baseIndex];
                    if (bc != '　' && char.IsWhiteSpace(bc))
                    {
                        Debug.LogError("空白のみの文字上にルビは表示できません");
                        continue;
                    }
                    int charMeshIndex = baseIndexToMeshIndex[baseIndex];
                    if (font.GetCharacterInfo(sr.Ruby[si], out CharacterInfo characterInfo, rubyFontSize))
                    {
                        float offsetXPos = (rubyFontSize * si + rubyFontSize / 2.0f) - (baseOffsMap[baseIndex - startIndexMap[di]]) + regionWidthDiff;
                        Vector3 pivot = (stream[charMeshIndex * 6 + 0].position + stream[charMeshIndex * 6 + 2].position) / 2.0f;
                        if (font.GetCharacterInfo(bc, out CharacterInfo characterInfoBase, baseFontSize))
                        {
                            pivot.y = stream[charMeshIndex * 6 + 2].position.y - characterInfoBase.minY;
                        }
                        pivot += new Vector3(offsetXPos, baseFontSize, 0);
                        for (int i = 0; i < 6; i++)
                        {
                            UIVertex vert = stream[charMeshIndex * 6 + i];
                            Vector3 position = vert.position;
                            switch (i)
                            {
                                case 0:
                                case 5:
                                    vert.uv0 = characterInfo.uvTopLeft;
                                    position = pivot + new Vector3(-characterInfo.advance / 2.0f, characterInfo.maxY, 0);
                                    break;
                                case 1:
                                    vert.uv0 = characterInfo.uvTopRight;
                                    position = pivot + new Vector3(characterInfo.advance / 2.0f, characterInfo.maxY, 0);
                                    break;
                                case 4:
                                    vert.uv0 = characterInfo.uvBottomLeft;
                                    position = pivot + new Vector3(-characterInfo.advance / 2.0f, characterInfo.minY, 0);
                                    break;
                                case 2:
                                case 3:
                                    vert.uv0 = characterInfo.uvBottomRight;
                                    position = pivot + new Vector3(characterInfo.advance / 2.0f, characterInfo.minY, 0);
                                    break;
                            }
                            vert.position = position;
                            stream.Add(vert);
                        }
                    }
                }
            }

        }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        this.text = m_RubyText;

        base.OnValidate();
    }

#endif
    string PureString(string text)
    {
        if (supportRichText)
        {
            return Regex.Replace(text, "<color=.*?>|</color>|<b>|</b>|<i>|</i>|<size=.*?>|</size>|<material=.*?>|</material>|<quad.*?>", String.Empty);
        }
        return text;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(RubyText))]
    public class CustomTextEditor : UnityEditor.UI.TextEditor
    {
        SerializedProperty m_RubyTextData;
        SerializedProperty m_FontData;
        SerializedProperty m_RubyFontScaleData;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_RubyTextData = serializedObject.FindProperty("m_RubyText");
            m_FontData = serializedObject.FindProperty("m_FontData");
            m_RubyFontScaleData = serializedObject.FindProperty("m_RubyFontScale");
        }

        public override void OnInspectorGUI()
        {
            var component = (RubyText)target;
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_RubyTextData);
            EditorGUILayout.PropertyField(m_RubyFontScaleData);
            EditorGUILayout.PropertyField(m_FontData);

            AppearanceControlsGUI();
            RaycastControlsGUI();
            MaskableControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
