using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int level = 1;
    //怪物链表
    [HideInInspector]
    public List<Enemy> enemyList = new List<Enemy>();

    private MapManager mapManager;

    //死否重新开始游戏
    private bool isRestart = false;
    //是否已显示EscPanel
    public bool isShowEscPanel = false;
    //
    public bool isfail = false;
    //
    public bool iswin = false;
    private static GameManager _instance;
    public static GameManager Instance
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
        //删除
        //InitGame();
    }
    public void InitGame()
    {
        //清空怪物链表
        enemyList.Clear();
        //加载场景音乐
        AudioManager.Instance.PlayBgMusic(AudioDic.level_BgMusic);
        //初始化地图
        mapManager = GetComponent<MapManager>();
        mapManager.InitMap();
    }
    public void OnPlayerMove()
    {       
        if (enemyList.Count > 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                //如果怪物死掉,从列表中去除
                if (enemyList[i] != null)
                {
                    enemyList[i].Move();
                }
                else
                {
                    enemyList.RemoveAt(i);
                }
            }
        }
        //GameObject gamePanel = GameObject.FindGameObjectWithTag("GamePanel");
        //gamePanel.SendMessage("HpOrStateChange");
    }

    void OnLevelWasLoaded(int sceneLevel)
    {
        if (!isRestart)
        {
            //GamePanel没有手动清除，清空UI栈
            UIManager.Instance.ReStart();
            level++;
            Player.Instance.enabled = false;
            switch (level)
            {
                case 2:
                    UIManager.Instance.PushPanel(UIPanelType.Act2StoryPanel);
                    break;
                case 3:
                    UIManager.Instance.PushPanel(UIPanelType.Act3StoryPanel);
                    break;
                case 4:
                    UIManager.Instance.PushPanel(UIPanelType.Act4StoryPanel);
                    break;
                case 5:
                    UIManager.Instance.PushPanel(UIPanelType.Act5StoryPanel);
                    break;
                case 6:
                    UIManager.Instance.PushPanel(UIPanelType.Act6StoryPanel);
                    break;
                case 7:
                    UIManager.Instance.PushPanel(UIPanelType.ReportActPanel);
                    break;
            }
        }
        else
        {
            isRestart = false;
            //GamePanel没有手动清除，清空UI栈
            UIManager.Instance.ReStart();
            level = 1;
            UIManager.Instance.PushPanel(UIPanelType.ReportActPanel);
        }
    }
    public void RestartGame()
    {
        isRestart = true;
        isfail = false;
        iswin = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(Player.Instance.gameObject);
        Destroy(Mother.Instance.gameObject);
        //Destroy(gameObject);
    }
    public void PauseGame()
    {
        Player.Instance.enabled = false;
    }
    public void ContinueGame()
    {
        Player.Instance.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Player.Instance && !isShowEscPanel)
        {
            ShowEscPanel();
        }
    }

    public void ShowEscPanel()
    {
        isShowEscPanel = !isShowEscPanel;
        UIManager.Instance.PushPanel(UIPanelType.EscPanel);
        PauseGame();
    }
}