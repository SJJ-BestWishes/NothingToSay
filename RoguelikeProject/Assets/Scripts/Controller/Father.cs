using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Father : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [Header("主角与父亲")]
    public Vector2 distance = new Vector2(0,4);

    private Player player;
    private Vector2 targetPos;
    private float speed;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = Player.Instance;
        speed = player.playerModel.speed;
        targetPos = transform.position;
    }
    private void Update()
    {
        if (player.playerModel.targetPos.y + distance.y <= 10)
        {
            targetPos = player.playerModel.targetPos + distance;
        }
        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime));
    }
}