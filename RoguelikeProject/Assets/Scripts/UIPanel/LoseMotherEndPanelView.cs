using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoseMotherEndPanelView : BasePanel
{
    public float bgFadeTime = 3;
    public float CameraAwayTime = 5;
    private bool isEnter = false;
    private Transform player;
    private Image bgImage;
    private void Start()
    {
        player = Player.Instance.gameObject.transform;
        bgImage = GetComponent<Image>();

        Mother.Instance.enabled = false;
        GameManager.Instance.isShowEscPanel = true;
        GameManager.Instance.level = -1;
        GameManager.Instance.InitGame();
        AudioManager.Instance.PlayBgMusic(AudioDic.act_BgMusic);
        bgImage.DOFade(0, 3).OnComplete(()=>
        {
            Player.Instance.enabled = true;
            GameManager.Instance.isShowEscPanel = false;
            Mother.Instance.gameObject.SetActive(true);
        });
    }
    private void Update()
    {
        if (!isEnter && player.position.y >= 4.99)
        {
            isEnter = true;
            Player.Instance.enabled = false;
            GameManager.Instance.isShowEscPanel = true;
            Camera.main.DOOrthoSize(4, CameraAwayTime).OnComplete(()=>
            {
                bgImage.DOFade(1, 3).OnComplete(()=>
                {
                    UIManager.Instance.PushPanel(UIPanelType.WinPanel);
                });
            });
        }
    }
}