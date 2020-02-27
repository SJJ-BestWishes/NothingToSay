using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mother : MonoBehaviour
{
    private static Mother _instance;
    public static Mother Instance
    {
        get
        {
            return _instance;
        }
    }

    public float speed = 10;
    public int hp = 30;
    public int maxHp = 100;
    public bool isADD = false;
    
    [HideInInspector]
    public Vector2 targetPos;
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
        targetPos = transform.position;
    }

    private void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime));
    }

    public void Move(Vector2 oldPos)
    {
        targetPos = oldPos;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
        animator.SetTrigger("motherDamage");
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Invoke("DieImmediately", Player.Instance.restTime);
    }

    private void DieImmediately()
    {
        Player.Instance.enabled = false;
    }
}
