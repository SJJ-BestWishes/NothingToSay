using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamePanelView : BasePanel
{
    public GameObject playerInfo;
    public GameObject motherInfo;

    private Slider playerInfo_Slider;
    private Slider motherInfo_Slider;
    private Text playerInfo_Text;
    private Text motherInfo_Text;

    private void Start()
    {
        playerInfo_Slider = playerInfo.GetComponentInChildren<Slider>();
        motherInfo_Slider = motherInfo.GetComponentInChildren<Slider>();

        playerInfo_Text = playerInfo.GetComponentInChildren<Text>();
        motherInfo_Text = motherInfo.GetComponentInChildren<Text>();

        playerInfo_Slider.interactable = false;
        //必须在Player生成之后
        playerInfo_Slider.maxValue = Player.Instance.maxHp;
        playerInfo_Slider.minValue = 0;
        playerInfo_Slider.wholeNumbers = true;

        motherInfo_Slider.interactable = false;
        motherInfo_Slider.maxValue = Mother.Instance.maxHp;
        motherInfo_Slider.minValue = 0;
        motherInfo_Slider.wholeNumbers = true;
        HpOrStateChange();
    }

    public void HpOrStateChange()
    {
        //状态变化
        if (Mother.Instance.isADD)
        {
            motherInfo.SetActive(true);
        }
        else
        {
            motherInfo.SetActive(false);
        }
        //生命变化
        playerInfo_Slider.value = Player.Instance.hp;
        motherInfo_Slider.value = Mother.Instance.hp;

        playerInfo_Text.text = playerInfo_Slider.value.ToString();
        motherInfo_Text.text = motherInfo_Slider.value.ToString();

        if (playerInfo_Slider.value <= 0 || motherInfo_Slider.value <= 0)
        {
            UIManager.Instance.PushPanel(UIPanelType.LosePanel);
        }
    }
}