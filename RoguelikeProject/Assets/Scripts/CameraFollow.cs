using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂在Camera上，GameManager是一个不会销毁的物体，掌控全局
public class CameraFollow : MonoBehaviour
{
    //分辨率1024*768,正交视口为3时
    private float upBorder;
    private float downBorder = 2.5f;
    private float leftBorder = 3.5f;
    private float rightBorder;

    public GameObject GameManager;

    public void Awake()
    {
        //如果没有生成单例
        if (!AudioManager.Instance)
            Instantiate(GameManager);
    }

    public void SetBorder(int line,int colums)
    {
        upBorder = line - 3.5f;
        rightBorder = colums - 4.5f;
    }

    /// <summary>
    /// 每次主角移动时候调用
    /// </summary>
    public void Follow()
    {
        if (Player.Instance)
        {
            Vector3 pos = Player.Instance.transform.position;
            pos.z = -10;
            if (pos.x >= rightBorder)
                pos.x = rightBorder;
            if (pos.x <= leftBorder)
                pos.x = leftBorder;
            if (pos.y >= upBorder)
                pos.y = upBorder;
            if (pos.y <= downBorder)
                pos.y = downBorder;
            transform.position = pos;
        }
        else
            Debug.Log("没有发现主角");
    }
    private void Update()
    {
        Follow();
    }
}
