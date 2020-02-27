using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Create time
/// Last revision date 
/// </summary>
/// 按钮鼠标移入特效(呼吸特效)，播放移入声音
/// 放在Text脚本上
public class TextBreatheEfc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text text;
    private bool mark = false;
    private float timecounter = 0;
    private float halflooptime;
    public Color textOriginalColor = new Color(0, 0, 0);
    public Color textEndColor = new Color(0, 1, 1);//你呼吸到最后想要的颜色
    public float looptime = 2;//呼吸变换一次时间
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mark == false)
        {
            mark = !mark;
            AudioManager.Instance.PlayEfcMusic(AudioDic.moveIntoBtn1_EfcMusic);
        }
        //当鼠标光标移入该对象时触发

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (mark == true)
        {
            mark = !mark;
            text.color = textOriginalColor;//离开时颜色回复正常
            timecounter = 0;//计数器归0
        }
        //当鼠标光标移出该对象时触发
    }
    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.color = textOriginalColor;
        halflooptime = looptime / 2;
    }
    private void Update()
    {
        TextColorEffect();
    }
    private void TextColorEffect()
    {
        if (mark && timecounter < halflooptime)
        {
            timecounter += Time.unscaledDeltaTime;
            text.color = Color.Lerp(textOriginalColor, textEndColor, timecounter / halflooptime);
        }
        else if (mark && timecounter > halflooptime && timecounter < looptime)
        {
            timecounter += Time.unscaledDeltaTime;
            text.color = Color.Lerp(textEndColor, textOriginalColor, (timecounter - halflooptime) / halflooptime);
        }
        else if (mark && timecounter > looptime)
        {
            timecounter = 0;
        }
    }//文本颜色呼吸特效
}
