using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public GameObject startPanel;
    void Awake() {
        if (GameManager.Instance == null)
        {
            GameObject.Instantiate(gameManager);
            GameManager.Instance.startPanel = startPanel;
        }
    }
	
}
