using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//需要自定义编辑，UI入口
public class GameRoot : MonoBehaviour
{
    private void Start()
    {
        ReStartGame();
        UIManager.Instance.PushPanel(UIPanelType.BeginPanel);
        /*
        * UI入口，指定显示面板
        * UIManger.Instance 创建UIManger 静态对象,调用构造函数InitialPanelPathDict()初始化存储路径字典（转换Json类，添加对应键，值）
        * .PushPanel(UIPanelType)初始化栈，根据面板类型显示页面,并根据对应类型加入实例化字典,并且入栈，执行入栈方法
        */
    }

    public void ReStartGame()
    {
        UIManager.Instance.ReStart();
        UIManager.Instance.PushPanel(UIPanelType.BeginPanel);
    }
}
