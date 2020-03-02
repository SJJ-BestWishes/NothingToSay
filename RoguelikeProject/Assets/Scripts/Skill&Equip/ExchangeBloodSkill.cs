using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExchangeBloodSkill : MonoBehaviour
{
    private float currentExchangeBloodTime = 0;
    PlayerModel playerModel;
    private void Awake()
    {
        playerModel = Player.Instance.playerModel;
    }
    private void Update()
    {
        currentExchangeBloodTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Z) && playerModel.Hp >= 2 && Mother.Instance.motherModel.Hp < Mother.Instance.motherModel.maxHp && currentExchangeBloodTime >= playerModel.ExchangeBloodTime)
        {
            currentExchangeBloodTime = 0;
            playerModel.Hp--;
            Mother.Instance.motherModel.Hp++;
        }
        else if (Input.GetKey(KeyCode.X) && Mother.Instance.motherModel.Hp >= 2 && playerModel.Hp < playerModel.maxHp && currentExchangeBloodTime >= playerModel.ExchangeBloodTime)
        {
            currentExchangeBloodTime = 0;
            playerModel.Hp++;
            Mother.Instance.motherModel.Hp--;
        }
    }
}