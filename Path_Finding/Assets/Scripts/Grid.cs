using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GridType
{
    None,
    Start,  //起点
    End,    //终点
    Block,  //障碍
    Normal  //普通
}

public class Grid : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI num;
    public Button btn;
    public Image visitedImg;

    public Grid PreGrid
    {
        get;
        set;
    }
    public Vector2Int Pos
    {
        get;
        set;
    }
    public GridType type = GridType.None;
    public void ChangeType(GridType gridType)
    {
        this.type = gridType;
        switch (gridType)
        {
            case GridType.Normal:
                image.color = Color.white;
                break;
            case GridType.Block:
                image.color = Color.black;
                break;
            case GridType.Start:
                image.color = Color.green;
                break;
            case GridType.End:
                image.color = Color.red;
                break;
        }
    }
    

    public void BindClick(UnityAction clickFunc)
    {
        btn.onClick.AddListener(clickFunc);
    }

    public void SetVisited(bool value)
    {
        visitedImg.gameObject.SetActive(value);
    }

    public void ClearState()
    {
        visitedImg.gameObject.SetActive(false);
        PreGrid = null;
    }
    
}
