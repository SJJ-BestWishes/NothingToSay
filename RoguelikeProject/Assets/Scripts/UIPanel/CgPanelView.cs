using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CgPanelView : BasePanel
{
    public Image bgIma;
    public Image familyIma;
    public GameObject nearObj;
    public GameObject fireObj;
    public GameObject clouldObj;
    

    public float fadeTime = 2;
    public float cgRunningTime = 30;
    private Sequence sequence;
    public override void OnEnter()
    {
        base.OnEnter();
        AudioManager.Instance.PlayBgMusic(AudioDic.cg_BgMusic);
        sequence = DOTween.Sequence();
        sequence.Append(bgIma.DOFade(0, fadeTime).From())//.OnComplete(() => AudioManager.Instance.PlayBgMusic(AudioDic.GetBgAudioClip(AudioDic.cgBgMusic))))
            .Append(bgIma.transform.DOLocalMove(new Vector3(-13224, 0, 0), cgRunningTime).SetEase(Ease.Linear))
            .Join(nearObj.transform.DOLocalMove(new Vector3(-3000, 0, 0), 30).SetEase(Ease.Linear))
            .Join(fireObj.transform.DOShakePosition(30, 10))
            .Join(clouldObj.transform.DOShakeRotation(30, new Vector3(0, 15, 0)))
            .Append(bgIma.DOFade(0, fadeTime).OnComplete(()=>
            {
                UIManager.Instance.PopPanel();
                UIManager.Instance.PushPanel(UIPanelType.Act1StoryPanel);
            }))
            .Join(familyIma.DOFade(0, fadeTime));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sequence.Pause();
            UIManager.Instance.PopPanel();
            UIManager.Instance.PushPanel(UIPanelType.Act1StoryPanel);
        }
    }
}