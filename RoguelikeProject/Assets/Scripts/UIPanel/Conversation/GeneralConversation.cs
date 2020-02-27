using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
//注意
//挂载在Panel下
//1.对话存储路径(Resources下面)
public class GeneralConversation : BasePanel
{
    //对话文字(整篇)，一个元素放一句话
    public string[] strings;
    [Header("对话存储路径(Resources下面)")]
    public string path;
    //对话的人
    public Text[] texts;
    //每句话对应说话的是第几个人(whichPerson.Lenth =strings.Lenth  0是第一个，1是第二个)
    public int[] whichPerson;

    //每一句话说的时间
    [Header("说话时长")]
    public float speakTime =0.5f;
    //当前说话下标
    private int currentIndex = 0;
    //是否交谈完毕
    private bool isConversationOver = false;

    //字体
    public Font font;
    //字体大小
    public int fontSize = 30;
    //字体颜色
    public Color color = Color.white;

    private Tween currentTween = null;
    private void Start()
    {
        foreach (Text item in texts)
        {
            item.color = color;
            item.font = font;
            item.fontSize = fontSize;
            item.transform.parent.gameObject.SetActive(false);
        }
        //加载对话
        strings = LoadConversation(path);
        //DoTween初始化
        texts[whichPerson[currentIndex]].transform.parent.gameObject.SetActive(true);

        currentTween = texts[whichPerson[currentIndex]].DOText(strings[currentIndex], speakTime);
        currentTween.SetAutoKill(false);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        AudioManager.Instance.PlayBgMusic(AudioDic.act_BgMusic);
        GameManager.Instance.isShowEscPanel = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !currentTween.IsPlaying() && !isConversationOver)
        {
            currentIndex++;
            if (currentIndex == strings.Length -1)
            {
                isConversationOver = true;
            }
            //说到第二句话以上，判断这句话是不是和上面一个人说的相同
            if (currentIndex >= 1 && whichPerson[currentIndex] != whichPerson[currentIndex - 1])
            {
                texts[whichPerson[currentIndex - 1]].transform.parent.gameObject.SetActive(false);
                texts[whichPerson[currentIndex]].transform.parent.gameObject.SetActive(true);
            }
            OnSession(texts[whichPerson[currentIndex]], strings[currentIndex], speakTime);
        }
        else if (Input.GetMouseButtonDown(0) && !currentTween.IsPlaying() && isConversationOver)
        {
            UIManager.Instance.PopPanel();
            GameManager.Instance.isShowEscPanel = false;
            if (GameManager.Instance.iswin)
            {
                UIManager.Instance.PushPanel(UIPanelType.WinPanel);
            }
            else
            {
                UIManager.Instance.PushPanel(UIPanelType.ReportActPanel);
            }

        }
    }
    /// <summary>
    /// 对话
    /// </summary>
    /// <param name="text"></param>
    /// <param name="Strings"></param>
    /// <param name="time"></param>
    private void OnSession(Text text, string Strings, float time)
    {
        text.text = "";
        currentTween = text.DOText(Strings, time);
    }

    /// <summary>
    /// 加载对话
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string[] LoadConversation(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        string[] strs = textAsset.text.Split('\n');
        return strs;
    }
}