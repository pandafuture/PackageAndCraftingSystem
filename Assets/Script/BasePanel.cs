using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false;  // 当前界面是否已关闭，标志位
    protected new string name;  // 界面的名称


    protected virtual void Awake()
    {

    }


    // 界面开关
    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }


    // 打开界面的方法
    public virtual void OpenPanel(string name)
    {
        this.name = name;  // 给当前界面名称赋值
        gameObject.SetActive(true);  // 打开界面
    }

    // 关闭界面的方法
    public virtual void ClosePanel()
    {
        isRemove = true;  // 标记当前界面已关闭
        gameObject.SetActive(false);  // 关闭界面
        Destroy(gameObject);  // 销毁界面物体

        // 检查当前界面是否存在，若存在则移除缓存，表示未打开
        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
