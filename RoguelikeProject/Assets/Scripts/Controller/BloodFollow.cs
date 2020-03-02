using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BloodFollow : MonoBehaviour
{
    private Transform player;
    private BoxCollider2D boxCollider;
    private Image image;

    public Vector3 offset;
    private void Start()
    {
        boxCollider = GetComponentInParent<BoxCollider2D>();
        player = boxCollider.transform;
        offset = new Vector3(0,boxCollider.bounds.extents.y+0.1f,0);
        image = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        Vector3 player3DPosition = Camera.main.WorldToScreenPoint(player.position + offset);
        image.transform.position = player3DPosition;
    }
}
