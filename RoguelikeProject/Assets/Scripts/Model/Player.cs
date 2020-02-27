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
        _instance = this;
        //挂在此脚本的物体不会被销毁
        DontDestroyOnLoad(gameObject);
    }

    public float restTime = 0.5f;
    public float speed = 10;
    public int attackDamage = 1;
    public int hp = 30;
    public int maxHp = 100;
    public int eachStepLoseHp = 1;

    //技能
    public bool isExchangeBlood = false;
    private float currentExchangeBloodTime = 0;
    public float ExchangeBloodTime = 0.1f;

    [HideInInspector] 
    public Vector2 targetPos;
    [HideInInspector]
    public Vector2 oldPos;

    private float currentRestTime = 0;
    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        targetPos = transform.position;
    }

    void Update()
    {
        //if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd == true) return;
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime));
        currentRestTime += Time.deltaTime;
        if (currentRestTime >= restTime)
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
                RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(x, y));
                collider.enabled = true;
                if (hit.transform == null)
                {
                    oldPos = targetPos;
                    targetPos += new Vector2(x, y);
                    //走路
                    AudioManager.Instance.PlayEfcMusic(AudioDic.foot_EfcMusic);
                    GameManager.Instance.OnPlayerMove();
                    ProduceDisplacementDamage(eachStepLoseHp);
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
                            oldPos = targetPos;
                            targetPos += new Vector2(x, y);
                            ProduceDisplacementDamage(eachStepLoseHp);
                            GameManager.Instance.OnPlayerMove();                            
                            break;
                        case "Enemy":
                            animator.SetTrigger("Attack");
                            //AudioManager.Instance.PlayEfcMusic(AudioDic.attack_EfcMusic);
                            hit.collider.SendMessage("TakeDamage",attackDamage);
                            GameManager.Instance.OnPlayerMove();
                            break;
                        case "woman":
                            Mother mother = Mother.Instance;
                            if (!mother.isADD)
                            {
                                mother.isADD = true;
                                AudioManager.Instance.PlayEfcMusic(AudioDic.help_EfcMusic);
                                GameObject.FindGameObjectWithTag("GamePanel").SendMessage("HpOrStateChange");
                                isExchangeBlood = true;
                                UIManager.Instance.PushPanel(UIPanelType.SaveMotherPanel);
                            }
                            break;
                        case "Exit":
                            targetPos += new Vector2(x, y);
                            ProduceDisplacementDamage(eachStepLoseHp);
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
        
        if (isExchangeBlood)
        {          
            currentExchangeBloodTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.Z) && hp>=2 && Mother.Instance.hp <= Mother.Instance.maxHp && currentExchangeBloodTime>= ExchangeBloodTime)
            {
                currentExchangeBloodTime = 0;
                hp--;
                Mother.Instance.hp++;
                GameObject.FindGameObjectWithTag("GamePanel").SendMessage("HpOrStateChange");
            }
            else if (Input.GetKey(KeyCode.X) && Mother.Instance.hp >= 2 && hp <= maxHp && currentExchangeBloodTime >= ExchangeBloodTime)
            {
                currentExchangeBloodTime = 0;
                hp++;
                Mother.Instance.hp--;
                GameObject.FindGameObjectWithTag("GamePanel").SendMessage("HpOrStateChange");
            }           
        }

        //作弊
        if (Input.GetKeyDown(KeyCode.K))
        {
            hp = 100;
            GamePanelView gamePanelView = GameObject.FindGameObjectWithTag("GamePanel").GetComponent<GamePanelView>();
            if(gamePanelView)
            {
                gamePanelView.HpOrStateChange();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        AudioManager.Instance.PlayEfcMusic(AudioDic.damage_EfcMusic);
        animator.SetTrigger("Damage");
        if (hp <= 0)
        {
            Invoke("Die", restTime);
        }                  
    }
    private void Die()
    {
        //TODO
        enabled = false;
    }

    //产生位移的伤害
    private void ProduceDisplacementDamage(int damage)
    {
        Mother mother = Mother.Instance;
        GameObject gamePanel = GameObject.FindGameObjectWithTag("GamePanel");
        if (mother.isADD)
        {
            mother.hp-= damage;
            mother.Move(oldPos);
        }
        hp -= damage;
        if (hp <= 0)
        {
            Invoke("Die", restTime);
        }
        if (mother.hp <= 0)
        {
            mother.Die();
        }       
        gamePanel.SendMessage("HpOrStateChange");
    }
}
