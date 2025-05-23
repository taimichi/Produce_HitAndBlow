using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_GuideTextData", menuName = "ScriptableObjects/GuideTextData")]
public class GuideTextData : ScriptableObject
{
    public Sprite guideSprite;
    [TextArea(1,7)] public string Text = "";
}
