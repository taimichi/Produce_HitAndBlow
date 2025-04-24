using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //インベントリの
    public int width = 10;      //幅
    public int height = 10;     //高さ  

    public GameObject CellPre;      //マスのプレハブ
    public Transform gridTransform; //グリッド(大本)

    private GameObject[,] GridCells;//マスの2次元配列

    void Start()
    {
        CreateGrid();   //グリッド生成
    }

    void Update()
    {
        
    }

    /// <summary>
    /// グリッドを生成関数
    /// </summary>
    private void CreateGrid()
    {
        GridCells = new GameObject[width, height];  //配列の生成

        //インベントリのマス生成
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                //マス生成+配列に入れていく
                GridCells[w, h] = Instantiate(CellPre, gridTransform);
            }
        }
    }
}
