using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    //����p�̃v���n�u
    [SerializeField] private GameObject HistoryPre;
    //�����\���p�̃v���n�u
    [SerializeField] private GameObject AnswerPre;
    //�����̐e�I�u�W�F�N�g
    [SerializeField] private Transform HistoryParent;

    //�w��̎萔
    private int maxEffot = 0;

    //�����I�u�W�F�N�g��ۑ�����z��
    private GameObject[] Historys;
    //�����悤�I�u�W�F�N�g��ۑ�����ϐ�
    private GameObject AnswerObj;

    private void Start()
    {
        //���𐶐�
        CreateHistoryArea();
    }

    /// <summary>
    /// �����G���A�̐���
    /// �萔���̗����ƁA�����������
    /// </summary>
    public  void CreateHistoryArea()
    {
        switch (DifficultyData.DifficultyEntity.nowDifficlt)
        {
            case DifficultyData.Difficult.easy:
                maxEffot = 4;
                break;

            case DifficultyData.Difficult.normal:
                maxEffot = 6;
                break;

            case DifficultyData.Difficult.hard:
                maxEffot = 10;
                break;
        }

        Historys = new GameObject[maxEffot];
        for(int i = 0; i < maxEffot; i++)
        {
            Historys[i] = Instantiate(HistoryPre, HistoryParent);
        }
        AnswerObj = Instantiate(AnswerPre, HistoryParent);
    }

    /// <summary>
    /// �����̐��l�𐶐�
    /// </summary>
    private void AnswerGenerate()
    {

    }
}
