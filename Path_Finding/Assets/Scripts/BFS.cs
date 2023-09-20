using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class BFS : MonoBehaviour
{
    public Grid prefab;

    //地图宽高
    public int width = 5;
    public int height = 5;

    //起点和结束点
    public Vector2Int startPos;
    public Vector2Int endPos;

    private const int maxSize = 40;
    private void Start()
    {
        //初始化地图
        for (int y = 0; y < maxSize; y++)
        {
            for (int x = 0; x < maxSize; x++)
            {
                Grid grid = Instantiate<Grid>(prefab, transform); 
                grid.transform.localPosition = new Vector3(x * 55, y * 55);
                grid.BindClick( OnGridClick(y,x));
                grid.gameObject.SetActive(false);
                map[y, x] = grid;
            }
        }   
    }


    private Grid[,] map = new Grid[40,40];
    private void CreateMap()
    {
        ClearMap();
        //生成地图
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i,j].gameObject.SetActive(true); 
                map[i,j].ChangeType(GridType.Normal);
            }
        }
        map[startPos.y,startPos.x].ChangeType(GridType.Start);
        map[endPos.y,endPos.x].ChangeType(GridType.End);
    }

    private UnityAction OnGridClick(int y,int x)
    {
        return () =>
        {
            Grid grid = map[y, x];
            if (map[y, x].type == GridType.Normal)
            {
                map[y, x].ChangeType(GridType.Block);
            }
            else if (grid.type == GridType.Block)
            {
                map[y, x].ChangeType(GridType.Normal);
            }

        };

    }

    private void ClearMap()
    {
        for (int i = 0; i < maxSize; i++)
        {
            for (int j = 0; j < maxSize; j++)
            {
                map[i,j].gameObject.SetActive(false);
            }
        }
    }

    

    private void OnGUI()
    {
        if (GUILayout.Button("重置地图"))
        {
            CreateMap();
        }
    }
}
