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
                map[i, j].Pos = new Vector2Int(j, i);
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

    private int[,] dir = {{-1, 0}, {1, 0}, {0, 1}, {0, -1}};
    private void PathFind()
    {
        ClearState();
        Queue<Grid> queue = new Queue<Grid>();
        HashSet<Grid> visited = new HashSet<Grid>();
        queue.Enqueue(map[startPos.y,startPos.x]);
        visited.Add(map[startPos.y, startPos.x]);
        //查找路径
        while (queue.Count != 0)
        {
            Grid preGrid = queue.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                var pos = preGrid.Pos;
                int x = pos.x + dir[i,0];
                int y = pos.y + dir[i,1];
                if (x>=0 && x<width && y>=0 && y<height)
                {
                    var grid = map[y, x];
                    if (grid.type != GridType.Block && !visited.Contains(grid) ) {
                        queue.Enqueue(grid);
                        visited.Add(grid);
                        //设置来路 用于路径查找
                        grid.PreGrid = preGrid;
                    }
                }
            }
        }

        //从后往前寻找路径
        var lastGrid = map[endPos.y, endPos.x];
        while (lastGrid.PreGrid)
        {
            lastGrid.SetVisited(true);
            lastGrid = lastGrid.PreGrid;
        }

    }
    
    private void ClearState()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i,j].ClearState();
            }
        }
    }

    

    private void OnGUI()
    {
        if (GUILayout.Button("重置地图"))
        {
            CreateMap();
        }

        if (GUILayout.Button("BFS"))
        {
            PathFind();   
        }
    }
}
