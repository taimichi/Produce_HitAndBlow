using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //�C���x���g����
    public int width = 10;      //��
    public int height = 10;     //����  

    public GameObject CellPre;      //�}�X�̃v���n�u
    public Transform gridTransform; //�O���b�h(��{)

    private GameObject[,] GridCells;//�}�X��2�����z��

    void Start()
    {
        CreateGrid();   //�O���b�h����
    }

    void Update()
    {
        
    }

    /// <summary>
    /// �O���b�h�𐶐��֐�
    /// </summary>
    private void CreateGrid()
    {
        GridCells = new GameObject[width, height];  //�z��̐���

        //�C���x���g���̃}�X����
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                //�}�X����+�z��ɓ���Ă���
                GridCells[w, h] = Instantiate(CellPre, gridTransform);
            }
        }
    }
}
