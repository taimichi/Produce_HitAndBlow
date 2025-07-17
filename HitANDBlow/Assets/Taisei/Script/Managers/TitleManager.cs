using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private static TitleManager instance;
    public static TitleManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<TitleManager>();
            }
            return instance;
        }
    }

    private List<TitleInput> titleInputs = new List<TitleInput>();

    private void Awake()
    {
        if(this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
    }

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
