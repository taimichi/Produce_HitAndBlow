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

    //���͂��鐔�l�̍ŏ���
    private int minInputNumber = 0;
    //���͂��鐔�l�̍ő吔
    private int maxInputNumber = 9;

    //�����I�u�W�F�N�g��ۑ�����z��
    private GameObject[] Historys;
    //�����悤�I�u�W�F�N�g��ۑ�����ϐ�
    private GameObject AnswerObj;

    //���l���͊֘A�{�^���̃X�N���v�g�����郊�X�g
    private List<NumberInput> numberInputs = new List<NumberInput>();

    //�����̐��l�����p���X�g
    private List<int> answerGenerate = new List<int>();

    private void Start()
    {
        //���́A�����̐��l������
        NumberReset();

        //���𐶐�
        CreateHistoryArea();

        //��������
        AnswerGenerate();

    }

    /// <summary>
    /// ���l�֘A�̍X�V�p�֐�
    /// </summary>
    public void NumberUpdate()
    {
        for(int i= 0; i < numberInputs.Count; i++)
        {
            numberInputs[i].NumberButtonInput();
        }
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
                maxEffot = 5;
                break;

            case DifficultyData.Difficult.normal:
                maxEffot = 7;
                break;

            case DifficultyData.Difficult.hard:
                maxEffot = 10;
                break;
        }
        //�ő�l�A�ŏ��l��ݒ�
        maxInputNumber = maxEffot;
        minInputNumber = 0;

        Historys = new GameObject[maxEffot];
        for(int i = 0; i < maxEffot; i++)
        {
            Historys[i] = Instantiate(HistoryPre, HistoryParent);
        }
        AnswerObj = Instantiate(AnswerPre, HistoryParent);
    }

    /// <summary>
    /// ���́A�����̐��l��������
    /// </summary>
    private void NumberReset()
    {
        for(int i = 0; i < NumberData.ELEMNT_NUM; i++)
        {
            NumberData.InputNumberEntity.inputNumbers[i] = -1;
            NumberData.InputNumberEntity.answerNumbers[i] = -1;
        }
    }

    /// <summary>
    /// �����̐��l�𐶐�
    /// </summary>
    private void AnswerGenerate()
    {
        for(int i = minInputNumber; i < maxInputNumber; i++)
        {
            answerGenerate.Add(i);
        }

        int count = 0;
        while(count < NumberData.ELEMNT_NUM)
        {
            int indexNumber = Random.Range(minInputNumber, answerGenerate.Count);

            NumberData.InputNumberEntity.answerNumbers[count] = answerGenerate[indexNumber];
            answerGenerate.Remove(indexNumber);

            count++;
        }

        Debug.Log("�����\��" + string.Join(" , " , NumberData.InputNumberEntity.answerNumbers));
    }

    public void GetNumberInputScript(NumberInput _numberInput)
    {
        numberInputs.Add(_numberInput);
    }
}
