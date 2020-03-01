using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{  
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        playerModel = new PlayerModel();
        _instance = this;
        //挂在此脚本的物体不会被销毁
        DontDestroyOnLoad(gameObject);
    }

    public PlayerModel playerModel;
    private float currentExchangeBloodTime = 0;
    private float currentRestTime = 0;
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        playerModel.targetPos = transform.position;
        playerModel.HpEventHandler += Die;

    }

    void Update()
    {
        //if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd == true) return;
        rigidbody.MovePosition(Vector2.Lerp(transform.position, playerModel.targetPos, playerModel.speed * Time.deltaTime));
        currentRestTime += Time.deltaTime;
        if (currentRestTime >= playerModel.restTime)
        {
            currentRestTime = 0;
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            //同时按下移动x
            if (x != 0)
            {
                y = 0;
            }
            if (x != 0 || y != 0)
            {
                //GameManager.Instance.ReduceFood(1);
                //检测
                collider.enabled = false;
                RaycastHit2D hit = Physics2D.Linecast(playerModel.targetPos, playerModel.targetPos + new Vector2(x, y));
                collider.enabled = true;
                if (hit.transform == null)
                {
                    playerModel.oldPos = playerModel.targetPos;
                    playerModel.targetPos += new Vector2(x, y);
                    //走路
                    AudioManager.Instance.PlayEfcMusic(AudioDic.foot_EfcMusic);
                    GameManager.Instance.OnPlayerMove();
                    ProduceDisplacementDamage(playerModel.eachStepLoseHp);
                }
                else
                {
                    switch (hit.collider.tag)
                    {
                        case "OutWall":
                            break;
                        case "Wall":
                            animator.SetTrigger("Attack");
                            AudioManager.Instance.PlayEfcMusic(AudioDic.attack_EfcMusic);
                            hit.collider.SendMessage("TakeDamage");
                            GameManager.Instance.OnPlayerMove();
                            break;
                        case "Food":
                        case "Soda":
                            AudioManager.Instance.PlayEfcMusic(AudioDic.eatFood_EfcMusic);
                            hit.collider.SendMessage("TakeFood");
                            playerModel.oldPos = playerModel.targetPos;
                            playerModel.targetPos += new Vector2(x, y);
                            ProduceDisplacementDamage(playerModel.eachStepLoseHp);
                            GameManager.Instance.OnPlayerMove();                            
                            break;
                        case "Enemy":
                            animator.SetTrigger("Attack");
                            //AudioManager.Instance.PlayEfcMusic(AudioDic.attack_EfcMusic);
                            hit.collider.SendMessage("TakeDamage",playerModel.attackDamage);
                            GameManager.Instance.OnPlayerMove();
                            break;
                        case "woman":
                            Mother mother = Mother.Instance;
                            if (!mother.motherModel.IsADD)
                            {
                                mother.motherModel.IsADD = true;
                                AudioManager.Instance.PlayEfcMusic(AudioDic.help_EfcMusic);                                
                                playerModel.isExchangeBlood = true;//可以设计成加一个技能脚本
                                UIManager.Instance.PushPanel(UIPanelType.SaveMotherPanel);
                            }
                            break;
                        case "Exit":
                            playerModel.targetPos += new Vector2(x, y);
                            ProduceDisplacementDamage(playerModel.eachStepLoseHp);
                            //Application.LoadLevel(Application.loadedLevel);
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                            break;
                        case "father":
                            GameManager.Instance.iswin = true;
                            UIManager.Instance.PushPanel(UIPanelType.Act7FinalTalkPanel);
                            enabled = false;
                            break;
                    }
                }
            }
        }
        
        if (playerModel.isExchangeBlood)
        {          
            currentExchangeBloodTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.Z) && playerModel.Hp >= 2 && Mother.Instance.motherModel.Hp < Mother.Instance.motherModel.maxHp && currentExchangeBloodTime>= playerModel.ExchangeBloodTime)
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

        //作弊
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerModel.Hp = 100;
        }
    }

    public void TakeDamage(int damage)
    {
        playerModel.TakeDamage(damage);
        AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
        animator.SetTrigger("Damage");
        //if (hp <= 0)
        //{
        //    Invoke("Die", restTime);
        //}                  
    }
    private void Die(object obj ,EventArgs eventArgs)
    {
        PlayerModel playerModel = (PlayerModel)obj;
        if (playerModel.Hp <= 0)
        {
            Invoke("ImmediDie", playerModel.restTime);
            UIManager.Instance.PushPanel(UIPanelType.LosePanel);
        }
        Debug.Log("进入判断死亡方法");
    }
    private void ImmediDie()
    {
        //TODO
        enabled = false;
    }

    //产生位移的伤害
    private void ProduceDisplacementDamage(int damage)
    {
        playerModel.TakeDamage(damage);
        Mother mother = Mother.Instance;
        if (mother.motherModel.IsADD)
        {
            mother.motherModel.Hp -= damage;
            mother.Move(playerModel.oldPos);
        }
        //if (mother.hp <= 0)
        //{
        //    mother.Die();
        //}      
    }
}
