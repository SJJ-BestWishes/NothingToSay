using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class PlayerModel
{
    public float restTime = 0.5f;
    public float speed = 10;
    public int attackDamage = 1;
    [SerializeField]
    public int hp = 80;
    public int defense = 0;
    public int Hp {
        get 
        {
            return hp;
        }
        set
        {
            if (value >= maxHp)
                hp = maxHp;
            else if (value <= 0)
                hp = 0;
            else
                hp = value;
            HpEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
    public EventHandler HpEventHandler;

    public int maxHp = 100;
    public int eachStepLoseHp = 1;

    //技能
    private bool isExchangeBlood = false;
    public bool IsExchangeBlood {
        get
        {
            return isExchangeBlood;
        }
        set {
            isExchangeBlood = value;
            IsExchangeBloodEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
    public EventHandler IsExchangeBloodEventHandler;
    public float ExchangeBloodTime = 0.1f;

    public Vector2 targetPos;
    public Vector2 oldPos;

    public PlayerModel()
    {       
    }
    public void TakeDamage(int damage)
    {
        Hp -= (damage - defense);
        //AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
        //animator.SetTrigger("Damage");
        //if (hp <= 0)
        //{
        //    Invoke("Die", restTime);
        //}
    }

}