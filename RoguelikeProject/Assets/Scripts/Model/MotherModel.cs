using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MotherModel
{
    public float speed = 10;
    private int hp = 30;
    public int Hp
    {
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
    private bool isADD = false;
    public bool IsADD 
    {
        get { return isADD; }
        set 
        { 
            isADD = value;
            IsADDEventHandler ?.Invoke(this, EventArgs.Empty);
        }       
    }
    public EventHandler IsADDEventHandler;
    public Vector2 targetPos;



    public void Move(Vector2 oldPos)
    {
        targetPos = oldPos;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        //AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
        //animator.SetTrigger("motherDamage");
        //if (hp <= 0)
        //{
        //    Die();
        //}
    }
    //public void Die()
    //{
    //    Invoke("DieImmediately", Player.Instance.restTime);
    //}

    //private void DieImmediately()
    //{
    //    Player.Instance.enabled = false;
    //}
}