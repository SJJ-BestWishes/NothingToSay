using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BeginPanelView : BasePanel
{
    public Button[] button;

    public Text[] text;

    public Image bgIma;
    public float fadeTime = 3;

    private Color bgImaOriginalColor = new Color(1,1,1,1);
    private void Awake()
    {
        foreach (Button item in button)//点击
        {
            item.onClick.AddListener(()=>//void
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
        //text[0].GetComponent<TextBreatheEfc>().gameObject.SetActive(true);
        bgIma.color = bgImaOriginalColor;
        bgIma.DOFade(0, fadeTime).From();
        AudioManager.Instance.PlayBgMusic(AudioDic.act_BgMusic);
    }

    public void OnBtnClick(GameObject btn)
    {
        if (btn.name == "StartBtn")
        {
            btn.SetActive(false);
            AudioManager.Instance.PlayEfcMusic(AudioDic.btnClickWater_EfcMusic);
            bgIma.DOFade(0, fadeTime).OnComplete(() =>
            {
                AudioManager.Instance.Stop();
                UIManager.Instance.PopPanel();
                UIManager.Instance.PushPanel(UIPanelType.CgPanel);
            });
        }
    }


}