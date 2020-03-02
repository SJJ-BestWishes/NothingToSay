using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mother : MonoBehaviour
{
    public static Mother Instance
    {
        get
        {
            return _instance;
        }
    }
    public MotherModel motherModel;

    private static Mother _instance;
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;

    private void Awake()
    {
        _instance = this;
        //挂在此脚本的物体不会被销毁
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        motherModel.targetPos = transform.position;

        motherModel.HpEventHandler += On_IsDie;
    }

    private void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, motherModel.targetPos, motherModel.speed * Time.deltaTime));
    }

    public void Move(Vector2 oldPos)
    {
        motherModel.targetPos = oldPos;
    }

    public void TakeDamage(int damage)
    {
        motherModel.TakeDamage(damage);
        AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
        animator.SetTrigger("motherDamage");
        //if (hp <= 0)
        //{
        //    Die();
        //}
    }
    public void On_IsDie(object obj ,EventArgs eventArgs)
    {
        MotherModel motherModel = (MotherModel)obj;
        if (motherModel.Hp <= 0)
        {
            Invoke("DieImmediately", Player.Instance.playerModel.restTime);
            
        }
    }

    private void DieImmediately()
    {
        motherModel.HpEventHandler -= On_IsDie;
        Player.Instance.ImmediDie();
    }
}
