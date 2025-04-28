using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

public class NumberInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // �{�^���̎�ނ�ݒ�
    public enum ButtonProperty
    {
        None,         // �Ȃ��̏��
        Number,
        Enter,
        Cancel,

    }
    // ��{�I�ɂ�None�Őݒ肵�Ďg���Ƃ��ɕύX���ނ݂̂ɐݒ肷�邱��
    [Header("�{�^���̎�ޑI��"), SerializeField] ButtonProperty numButton = ButtonProperty.None;

    // �G�ꂽ�Ƃ��ɕ�����₷�����邽�߂ɕ\������
    [Header("�I�𒆂ɕ\������I�u�W�F�N�g"), SerializeField]
    GameObject nowSelectObj;

    // �A�j���[�V�����̐ݒ�
    [SerializeField] Animator anim;

    //�{�^���ɐݒ肷�鐔���̒l
    [SerializeField, Range(0, 9)] private int number = 0;

    #region UIOnOff
    public void OnPointerEnter(PointerEventData eventData)
    {
        // �\��
        nowSelectObj.SetActive(true);
        Debug.Log("UI�ɐG�ꂽ");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ��\��
        nowSelectObj.SetActive(false);
        Debug.Log("UI���痣�ꂽ");
    }
    #endregion

    private void Start()
    {
        //�����{�^���̎�
        if(numButton == ButtonProperty.Number)
        {
            //�����{�^����Image���擾���A�摜��ύX
            Image buttonImage = this.GetComponent<Image>();
            buttonImage.sprite = SpriteData.SpriteEntity.NumberSprite[number];
        }

        NumberManager numberManager = GameObject.Find("GameCanvas").GetComponent<NumberManager>();
        numberManager.GetNumberInputScript(this);
    }

    public void NumberButtonInput()
    {
        // �G�ꂽ��Ԃō��N���b�N���������珈��
        if (nowSelectObj.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (numButton != ButtonProperty.None)
                {
                    //anim.Play("UI_OnTheTouchMove");
                    // Enum�̒l���ƂɑΉ�����֐������s
                    InvokeMatchingMethod(numButton);
                }
            }
        }
    }

    private void InvokeMatchingMethod(ButtonProperty kButton)
    {
        string methodName = kButton.ToString(); // Enum�̖��O�𕶎���

        // ���\�b�h�����擾
        MethodInfo method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (method != null)
        {
            method.Invoke(this, null);
        }
        else
        {
            Debug.LogWarning($"���\�b�h '{methodName}' ��������܂���B");
        }
    }

    #region Function
    private void Number()
    {
        Debug.Log(number + " �����͂���܂���");
    }

    private void Enter()
    {
        Debug.Log("���肵�܂�");
    }

    private void Cancel()
    {
        
        Debug.Log("1���������܂�");
    }
    #endregion

}
