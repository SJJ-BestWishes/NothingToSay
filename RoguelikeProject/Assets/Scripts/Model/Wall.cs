using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wall : MonoBehaviour
{
    //默认为1血
    public int hp = 1;

    //按照每次受伤后需要呈现的图片
    public Sprite[] sprites;
    private int currentspriteIndex = 0;
    public void TakeDamage()
    {
        hp -= 1;
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