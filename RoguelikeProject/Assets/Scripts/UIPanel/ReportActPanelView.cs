using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ReportActPanelView : BasePanel
{
    //存储每一幕的标题
    public string[] strings;
    [Header("对话存储路径(Resources下面)")]
    public string path;
    public float showTime = 2;
    [Header("Level7设置")]
    public float soundBreakTime = 1;
    public int soundTime = 3;
    public int currentSoundTime = 1;
    public float fadeTime = 3;

    private Text text;
    private int level;
    private void Start()
    {
        GameManager.Instance.isShowEscPanel = true;
        strings = LoadConversation(path);
        level = GameManager.Instance.level;
        text = GetComponentInChildren<Text>();
        text.text = strings[level - 1];
        if (level != 7)
        {
            Invoke("Dispear", showTime);
        }
        else
        {           
            AudioManager.Instance.Stop();
            Invoke("PlayEfcAudio", soundBreakTime);
        }
        
    }

    /// <summary>
    /// 加载
    /// </summary>
    private string[] LoadConversation(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        string[] strs = textAsset.text.Split('\n');
        return strs;
    }

    private void Dispear()
    {
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.GamePanel);
        GameManager.Instance.InitGame();
        Player.Instance.enabled = true;
        GameManager.Instance.isShowEscPanel = false;
    }

    private void PlayEfcAudio()
    {
        if (currentSoundTime == soundTime)
        {
            AudioManager.Instance.PlayEfcMusic(AudioDic.DamageDrop_EfcMusic);
            Invoke("Fade", 2);
        }
        else
        {
            AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
            currentSoundTime++;
            Invoke("PlayEfcAudio", soundBreakTime);           
        }
    }
    private void Fade()
    {
        GameManager.Instance.InitGame();
        Mother.Instance.gameObject.SetActive(false);
        Mother.Instance.motherModel.IsADD = false;
        GetComponent<Image>().DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            UIManager.Instance.PopPanel();
            UIManager.Instance.PushPanel(UIPanelType.GamePanel);
            GamePanelView gamePanelView = GameObject.FindGameObjectWithTag("GamePanel").GetComponent<GamePanelView>();
            gamePanelView.playerInfo.SetActive(false);        
            Player.Instance.playerModel.Hp = 1000;
            Player.Instance.enabled = true;
            //恢复
            GameManager.Instance.isShowEscPanel = false;
        }
        );
    }
}