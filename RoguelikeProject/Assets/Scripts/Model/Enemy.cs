using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    //主角走几步,怪物走一步
    public int frequency = 2;
    public int hp = 2;
    public int attackDamage = 10;

    private int currentFrequency = 0;
    private Player player;
    private Mother mother;
    private Vector2 targetPos;

    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;

    private void Start()
    {
        player = Player.Instance;
        mother = Mother.Instance;
        targetPos = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        //加入GameManger 怪物链表
        GameManager.Instance.enemyList.Add(GetComponent<Enemy>());
    }
    void Update()
    {
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime));
    }
    public void Move()
    {
        //怪物和主角的距离
        Vector2 player_offset = player.targetPos - new Vector2(transform.position.x, transform.position.y);
        //怪物和妈妈的距离
        Vector2 mother_offset = mother.targetPos - new Vector2(transform.position.x, transform.position.y);
        if (player_offset.magnitude < 1.1f)
        {
            //攻击
            animator.SetTrigger("Attack");
            player.SendMessage("TakeDamage", attackDamage);

        }
        else if (mother.isADD && mother_offset.magnitude < 1.1f)
        {
            //攻击
            animator.SetTrigger("Attack");
            mother.SendMessage("TakeDamage", attackDamage);

        }
        else if (++currentFrequency >= frequency)
        {
            currentFrequency = 0;
            Vector2 move = new Vector2(0, 0);
            if (Mathf.Abs(player_offset.y) > Mathf.Abs(player_offset.x))
            {
                //按照y轴移动
                if (player_offset.y < 0)
                {
                    move.y = -1;
                }
                else
                {
                    move.y = 1;
                }
            }
            else
            {
                //按照x轴移动
                if (player_offset.x > 0)
                {
                    move.x = 1;
                }
                else
                {
                    move.x = -1;
                }
            }
            //设置目标位置之前 先做检测
            collider.enabled = false;
            //针对不会动的物体（和玩家要开始移动的时间点差不多）
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + move);
            collider.enabled = true;
            if (hit.transform == null)
            {
                bool canGo = true;
                //也不能和即将到这个位置的物体重合
                //1.怪物
                foreach (Enemy item in GameManager.Instance.enemyList)
                {
                    if (targetPos + move == item.targetPos)
                    {
                        canGo = false;
                        break;
                    }
                }
                //2.人物
                if (targetPos + move == player.targetPos || targetPos + move == mother.targetPos)
                {
                    canGo = false;
                }
                if (canGo)
                    targetPos += move;
            }
            else
            {
                //AI其他判断
            }
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //移除怪物列表(GameManger中自动检测)
            animator.SetTrigger("Die");
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
