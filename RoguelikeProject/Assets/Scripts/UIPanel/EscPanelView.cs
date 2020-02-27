using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EscPanelView : BasePanel
{
    public Button[] button;

    public Text[] text;

    public Image bgIma;
    public float fadeTime = 3;

    public Sprite failIma;
    public Sprite winIma;
    private void Awake()
    {
        foreach (Button item in button)//点击
        {
            item.onClick.AddListener(() =>//void
            {
                OnBtnClick(item.gameObject);
            });
        }
        foreach (Text item in text)//添加呼吸特效
        {
            item.gameObject.AddComponent<TextBreatheEfc>();
        }
    }
    public override void OnEnter()
    {
        base.OnEnter();
        foreach (Button item in button)
        {
            if (item.name == "ContinueBtn")
            {
                if ( GameManager.Instance.isfail && item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(false);
                    bgIma.sprite = failIma;
                    bgIma.SetNativeSize();
                }
            }
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
        if (btn.name == "RestartBtn")
        {
            GameManager.Instance.isShowEscPanel = false;
            GameManager.Instance.RestartGame();

        }
        if (btn.name == "QuitBtn")
        {
            Application.Quit();
        }
    }
}