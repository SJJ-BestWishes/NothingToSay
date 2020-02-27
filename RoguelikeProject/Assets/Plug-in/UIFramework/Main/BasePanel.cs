using UnityEngine;
//在UIManger中调用这几个方法
//可编辑，在所有子页面中统一使用方法
//注意
//OnEnter与Start区别，每次入栈都会执行一次OnEnter，而Start只会执行一次(没有SetActive的情况下)
//Start中可以加获得组件,OnEnter则加入动画等
public class BasePanel : MonoBehaviour
{   
    protected CanvasGroup canvasGroup;
    public virtual void OnEnter()
    {
        if (!canvasGroup)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
    /// <summary>
    /// 执行界面暂停方法（有别的界面入栈）
    /// </summary>
    public virtual void OnPause()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 执行界面继续方法（楼上页面出栈，使得这个界面恢复）
    /// </summary>
    public virtual void OnResume()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
    }
    public virtual void OnExit()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
