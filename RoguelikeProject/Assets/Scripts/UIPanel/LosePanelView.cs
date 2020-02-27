using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LosePanelView : BasePanel
{
    public Image bgIma;
    public Text failText;
    public float bgFadeTime = 2;
    public float bgRunTime = 4;
    public float failStrTime = 5;
    public float textFadeTime = 3;
    public string path;
    private string failStr;
    private Sequence sequence;
    private void Start()
    {
        GameManager.Instance.isShowEscPanel = true;
        AudioManager.Instance.PlayBgMusic(AudioDic.act_BgMusic);
        failStr = Resources.Load<TextAsset>(path).text;
        sequence = DOTween.Sequence();
        sequence.Append(bgIma.DOFade(0, bgFadeTime).From()).
            Append(bgIma.transform.DOMove(new Vector3(325, 384, 0), bgRunTime).SetEase(Ease.Linear)).
            Append(bgIma.DOFade(0, bgFadeTime)).
            Append(failText.DOText(failStr, failStrTime)).
            Append(failText.DOFade(0, textFadeTime).OnComplete(End));
    }
    public void End()
    {
        UIManager.Instance.PopPanel();
        GameManager.Instance.isfail = true;
        GameManager.Instance.isShowEscPanel = false;
        GameManager.Instance.ShowEscPanel();
    }
}