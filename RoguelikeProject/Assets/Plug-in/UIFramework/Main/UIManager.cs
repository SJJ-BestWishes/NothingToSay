using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
//单例
//挂载:GameRoot挂在Camera下
//注意
//1.在最开始状态下只有一个MainCanvas画布
//2.Prefab需要放在Resource文件夹下面 参考格式：Prefabs/UIPanel/预制体名称
//3.所有面板跳转脚本继承BasePanel
//4.面板隐藏可添加CanvasGroup组件实现(自动添加，若override BasePanel 中OnEnter()方法,则需要自己添加)
//5.jason 数据存储
//  方法1：
//        (需要创建文件夹StreamingAssets...)jason 数据存储在 "Assets" + "/StreamingAssets" + "/UIFramework" + "/UIPanelPath" + "/UIPanelPath.json"
//  方法2：
//        jason 数据存储在 Resources下(利用Resources加载txt(TextAsset)): "/UIFramework" + "/UIPanelPath" + "/UIPanelPath"
//6.TODO 同一场景下单个面板只能被实例化一次 (BasePanel中改OnExit()函数 ->Destory,并删除字典该键,可以解决)
public class UIManager
{
    //单例
    //1.定义一个静态的对象 在外界访问 内部构造
    //2.构造函数私有化
    private static UIManager _uiManager;
    public static UIManager Instance
    {
        get
        {
            if (_uiManager == null)
            {
                _uiManager = new UIManager();
            }
            return _uiManager;
        }
    }
    private UIManager()
    {
        InitialPanelPathDict();
    }

    //TODO
    //Canvas组件（初始状态下就只有一个Canvas）
    private Transform _canvasTransform;
    private Transform canvasTransform
    {
        get
        {
            if (_canvasTransform == null)
            {
                _canvasTransform = GameObject.Find("MainCanvas").transform;
            }
            return _canvasTransform;
        }
    }

    //储存所有面板Prefab的路径
    //名字,路径
    private Dictionary<string, string> panelPathDict;
    //存储所有实例化面板的Prefab的BasePanel组件
    //路径,组件
    private Dictionary<string, BasePanel> panelDict;

    //储存所有面板Prefab的路径的Jason文件路径
    //第一种方法

    #region    
    /*public static readonly string filePath =
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    Application.streamingAssetsPath + "/UIFramework" + "/UIPanelPath" + "/UIPanelPath.json";  //直接IO类读取就行
#elif UNITY_IOS
    =Application.dataPath + "/UIFramework" + "/UIPanelPath" + "/UIPanelPath.json";
#elif UNITY_ANDROID
    ="jar:file:/" + "/" + Application.dataPath + "!/assets/" +"/UIFramework/" + "/UIPanelPath/" + "/UIPanelPath.json"; //需要用www类读取
#else
    =string.Empty;
#endif*/
    #endregion
    //第二种方法
    public static readonly string filePath = "UIFramework" + "/UIPanelPath" + "/UIPanelPath";
    //内部类用来读取Json格式
    class Save
    {
        public UIPanelInfo[] uiPanelInfos;
    }

    /// <summary>
    /// 初始化存储路径字典（转换Json类，添加对应键，值）
    /// </summary>
    private void InitialPanelPathDict()
    {
        if (panelPathDict == null)
            panelPathDict = new Dictionary<string, string>();
        else
            panelPathDict.Clear();//TODO，[再次初始化，第一次初始化后路径发生变化，需在GanmeRoot启动前修正数据，再次初始化]
        string path = filePath;
        if (/*第一种方法*/File.Exists(path) ||/*第二种方法*/Resources.Load<TextAsset>(path))
        {
            //第一种方法
            //StreamReader sr = new StreamReader(path);
            //第二种方法
            StringReader sr = new StringReader(Resources.Load<TextAsset>(path).text);
            //结束
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            Save save = JsonMapper.ToObject<Save>(jsonStr);

            foreach (UIPanelInfo item in save.uiPanelInfos)
            {
                Debug.Log(item.panelTypeStr + "\n" + item.path);
                panelPathDict.Add(item.panelTypeStr, item.path);
            }
        }
        else
            Debug.Log("没有取得面板预制件地址");
    }
    /// <summary>
    /// 根据面板类型得到实例化的面板,并根据对应类型加入实例化字典,Prefab需要放在Resource文件夹下面
    /// </summary>
    /// <param name="panelTypeStr">面板类型，UIPanelType中的值</param>
    /// <returns></returns>
    private BasePanel GetPanel(string panelTypeStr)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<string, BasePanel>();
        }
        //如果没有实例化面板，寻找路径进行实例化，并且存储到已经实例化好的字典面板中
        if (panelDict.TryGet(panelTypeStr) == null)
        {
            string path = panelPathDict[panelTypeStr];
            if (path != null)
            {
                GameObject instPanel = Object.Instantiate(Resources.Load(path)) as GameObject;
                //TODO,是否保持在世界坐标轴的位置
                instPanel.transform.SetParent(canvasTransform, false);

                //如果之前key对应的value为null了,重新添加键（因为LoadSence的原因）
                if (panelDict.ContainsKey(panelTypeStr))
                {
                    panelDict.Remove(panelTypeStr);
                }
                panelDict.Add(panelTypeStr, instPanel.GetComponent<BasePanel>());
                return instPanel.GetComponent<BasePanel>();
            }
            else
            {
                Debug.Log("没有" + panelTypeStr + "类型的Panel");
                return null;
            }
        }
        else
        {
            return panelDict[panelTypeStr];
        }
    }

    //显示页面的栈
    private Stack<BasePanel> panelStack;
    /// <summary>
    /// 暂停当前页面，根据页面类型入栈，显示页面，执行入栈方法
    /// </summary>
    /// <param name="panelTypeStr">面板类型，UIPanelType中str</param>
    public void PushPanel(string panelTypeStr)
    {
        //如果没有栈，创建一个
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        //判断栈中是否有页面
        //1.已有页面
        if (panelStack.Count > 0)
        {
            //栈顶暂停
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }
        //2.没有页面（和已有页面接下来的处理情况一样）
        BasePanel currentPanel = GetPanel(panelTypeStr);
        currentPanel.OnEnter();
        panelStack.Push(currentPanel);
    }
    /// <summary>
    /// 出栈，隐藏当前页面，恢复上一个页面
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        //判断栈中是否有页面
        //1.没有界面
        if (panelStack.Count <= 0)
        {
            Debug.Log("没有界面");
            return;
        }
        //2.有页面
        else
        {
            BasePanel topPanel = panelStack.Pop();
            topPanel.OnExit();
            //如果下面还有页面的话
            if (panelStack.Count > 0)
            {
                BasePanel currentPanel = panelStack.Peek();
                currentPanel.OnResume();
            }
        }
    }

    /// <summary>
    /// 清空栈，用于栈没有手动全部推出,重新加载游戏
    /// </summary>
    public void ReStart()
    {
        panelStack.Clear();
    }
}
