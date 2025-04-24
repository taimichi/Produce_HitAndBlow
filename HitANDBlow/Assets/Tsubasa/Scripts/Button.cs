using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using System;
using UnityEngine.UI;
public partial class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region enumData
    // �{�^���̎�ނ�ݒ�
    // �^�C�g���V�[���ł̃{�^��
    public enum ButtonProperty
    {
        None,         // �Ȃ��̏��
        VolumeUp,     // ���ʏ㏸
        VolumeDown,   // ���ʉ��~
        GoTitle,      // �^�C�g���֖߂�
        OpenTutorial, // �`���[�g���A�����J��(��)
        Yes_Title,    // ���f����ꍇ
        No_Title,     // ���f���Ȃ��ꍇ
    }
    // ��{�I�ɂ�None�Őݒ肵�Ă���
    [Header("�{�^���̎�ޑI��"), SerializeField] ButtonProperty button = ButtonProperty.None;
    #endregion

    // �G�ꂽ�Ƃ��ɕ�����₷�����邽�߂ɕ\������
    [Header("�I�𒆂ɕ\������I�u�W�F�N�g"), SerializeField]
    GameObject nowSelectObj;

    [Header("�Q�[�����f���邩�m�F����UI�p�l��"),SerializeField] GameObject GoTitlePanel;
    // �A�j���[�V�����̐ݒ�
    [SerializeField] Animator anim;

    // UI�ڐG�̃C���^�[�t�F�[�X����
    #region InterFace
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

    public void Update()
    {
        // �G�ꂽ��Ԃō��N���b�N���������珈��
        if (nowSelectObj.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (button != ButtonProperty.None)
                {
                    anim.Play("UI_OnTheTouchMove");
                    // Enum�̒l���ƂɑΉ�����֐������s
                    InvokeMatchingMethod(button);
                }
            }
        }
    }

    // ����͊֐��̃}�b�`���O
    // enum�Ɠ������O�̊֐������s����
    #region MatchingMethod
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

    // ���ʏグ��@�\
    private void VolumeUp()
    {
        Debug.Log("UP");
    }

    // ���ʉ�����@�\
    private void VolumeDown()
    {
        Debug.Log("DOWN");
    }
    #endregion
}
