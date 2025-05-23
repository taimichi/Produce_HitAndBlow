using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuideTextList", menuName = "ScriptableObjects/GuideTextList")]
public class GuideTextList : ScriptableObject
{
    public const string PATH = "GuideTextList";
    private static GuideTextList _guideTextEntity;
    public static GuideTextList GuideTextEntity
    {
        get
        {
            if (_guideTextEntity == null)
            {
                _guideTextEntity = Resources.Load<GuideTextList>(PATH);
                if (_guideTextEntity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _guideTextEntity;
        }
    }

    public List<GuideTextData> guideTextList = new List<GuideTextData>();
}
