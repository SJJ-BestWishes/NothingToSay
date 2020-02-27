using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameManager做成预制体，加上各种控制单例脚本
public class StartGame : MonoBehaviour
{
    public void Start()
    {
        UIManager.Instance.PushPanel(UIPanelType.BeginPanel);
    }

    public void ReStartGame()
    {
        UIManager.Instance.ReStart();
        UIManager.Instance.PushPanel(UIPanelType.BeginPanel);
    }
}
