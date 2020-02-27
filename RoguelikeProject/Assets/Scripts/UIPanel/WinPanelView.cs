using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WinPanelView : BasePanel
{
    public Image bgIma;
    public Text winText;
    public Text titleText;
    public Button quitBtn;

    public float bgFadeTime = 2;
    public float winStrTime = 5;
    public float textFadeTime = 3;
    public string path;

    private string winStr;
    private Sequence sequence;
    private void Start()
    {
        //静止插入EscPanel
        GameManager.Instance.isShowEscPanel = true;

        sequence = DOTween.Sequence();
        winStr = Resources.Load<TextAsset>(path).text;
        quitBtn.onClick.AddListener(()=>Application.Quit());

        sequence.Append(bgIma.DOFade(0, bgFadeTime).From()).
            Append(winText.DOText(winStr, winStrTime).OnComplete(()=> winText.text = "")).
            Append(winText.DOText("一切尽在不言之中", 2)).
            Append(winText.DOFade(0, textFadeTime)).
            Append(titleText.DOFade(1, bgFadeTime).OnComplete(()=>               
                {
                    quitBtn.gameObject.SetActive(true);
                    quitBtn.GetComponentInChildren<Text>().gameObject.AddComponent<TextBreatheEfc>();
                }));
    }

}