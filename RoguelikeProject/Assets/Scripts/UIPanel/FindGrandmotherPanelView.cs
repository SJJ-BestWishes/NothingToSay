using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FindGrandmotherPanelView : BasePanel
{
    public Button[] buttons;
    public Text[] texts;
    private void Start()
    {
        //禁止ECS和角色移动
        GameManager.Instance.isShowEscPanel = true;
        GameManager.Instance.PauseGame();

        foreach (Button item in buttons)//点击
        {
            item.onClick.AddListener(() =>//void
            {
                OnBtnClick(item.gameObject);
            });
        }
        foreach (Text item in texts)//添加呼吸特效
        {
            item.gameObject.AddComponent<TextBreatheEfc>();
        }
    }
    public void OnBtnClick(GameObject btn)
    {
        if (btn.name == "ContinueBtn")
        {
            GameManager.Instance.isShowEscPanel = false;
            GameManager.Instance.ContinueGame();
            UIManager.Instance.PopPanel();
        }
    }
}