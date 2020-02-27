using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    public Button[] buttons;
    private void Start()
    {
        foreach (Button item in buttons)
        {
            item.onClick.AddListener(delegate ()             
            {
                OnClick(item.gameObject);
            });
        }
    }
    private void OnClick(GameObject obj)
    {
        if (obj.name == "")
        {
        }
        else if (obj.name == "")
        {
        }
    }
}
