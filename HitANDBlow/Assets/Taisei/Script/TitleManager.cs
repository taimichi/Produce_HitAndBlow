using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private List<TitleInput> titleInputs = new List<TitleInput>();

    [SerializeField] private GameObject GuideObj;
    
    void Start()
    {
        
    }

    public void TitleUpdate()
    {
        for(int i = 0; i < titleInputs.Count; i++)
        {
            titleInputs[i].TitleButtonInput();
        }
    }

    public void GetTitleInput(TitleInput _titleInput)
    {
        titleInputs.Add(_titleInput);
    }
}
