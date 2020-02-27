using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Food : MonoBehaviour
{
    public int addHp = 10;
    public void TakeFood()
    {
        Mother mother = Mother.Instance;
        Player player = Player.Instance;
        if (mother.isADD && mother.hp >= 0)
        {
            if (mother.hp + addHp > mother.maxHp)
                mother.hp = mother.maxHp;
            else
                mother.hp += addHp;
        }
        if (player.hp >= 0)
        {
            if (player.hp + addHp > player.maxHp)
                player.hp = mother.maxHp;
            else
                player.hp += addHp;
        }
        Destroy(gameObject);
    }
}