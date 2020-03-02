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
        if (mother.motherModel.IsADD && mother.motherModel.Hp >= 0)
        {
            mother.motherModel.Hp += addHp;
        }
        if (player.playerModel.Hp >= 0)
        {
            player.playerModel.Hp += addHp;
        }
        Destroy(gameObject);
    }
}