using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private NumberManager NumManager;


    void Start()
    {
        NumManager = GameObject.Find("GameCanvas").GetComponent<NumberManager>();   
    }

    void Update()
    {
        NumManager.NumberUpdate();
    }
}
