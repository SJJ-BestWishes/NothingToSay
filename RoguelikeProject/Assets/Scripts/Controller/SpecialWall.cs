using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpecialWall : MonoBehaviour
{
    //默认为1血
    public int hp = 1;

    //按照每次受伤后需要呈现的图片
    public Sprite[] sprites;
    private int currentspriteIndex = 0;
    public void TakeDamage()
    {
        if (Player.Instance.IsFindGrandmother)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject item in objs)
            {
                Destroy(item);
                
            }
            UIManager.Instance.PushPanel(UIPanelType.FindGrandmotherPanel);
            string titleText = "触发遗物";
            string mainText = "【祖母的项链】\n\n所有障碍物\n得到移除";
            GameObject.FindGameObjectWithTag("FindGrandmotherPanel").GetComponent<FindGrandmotherPanelView>().ChangeShowTextMessage(titleText, mainText);
        }
        else
        { hp -= 1;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[currentspriteIndex++];
            }
        }
    }
}