using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 跨场景的全局 UI 管理器
public class UIManager
{
    // 单例模式
    private static UIManager _instance;

    //界面关系配置表
    private Dictionary<string, string> pathDict;

    // UI 界面根节点
    private Transform _uiRoot;

    // 预制件缓存字典
    private Dictionary<string, GameObject> prefabDict;

    // 界面缓存字典，存储当前已打开的界面
    public Dictionary<string, BasePanel> panelDict;


    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }


    private UIManager()
    {
        InitDicts();  // 初始化界面关系配置表
    }


    // 初始化界面关系配置表方法
    private void InitDicts()
    {
        // 初始化预制件字典
        prefabDict = new Dictionary<string, GameObject>();

        // 初始化界面缓存字典
        panelDict = new Dictionary<string, BasePanel>();

        // 把界面路径配置到字典中
        pathDict = new Dictionary<string, string>()
        {
            // 配置 PackagePanel 对应的路径
            {UIConst.PackagePanel, "Package/PackagePanel" },
        };
    }


    // 设置 UI 界面根节点
    public Transform UIRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                // 把 Canvas 作为根节点
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }


    // 打开界面的方法
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;

        // 检查界面是否打开
        if(panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面已打开：" + name);
            return null;
        }

        // 检查路径是否有配置
        string path = "";
        if(!pathDict.TryGetValue(name, out path))
        {
            Debug.Log("界面名称错误，或者未配置路径：" + name);
            return null;
        }

        // 加载使用缓存的界面预制件
        GameObject panelPrefab = null;
        // 通过预制件缓存字典，查看是否被加载过
        if(!prefabDict.TryGetValue(name, out panelPrefab))
        {
            // 未加载，则加载并放入预制件缓存字典
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        // 打开界面
        // 把预制件加载出来，并挂载 UIRoot 下
        GameObject PanelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = PanelObject.GetComponent<BasePanel>();

        // 添加到界面缓存字典中，表示这个界面已经打开了
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }


    // 关闭界面的方法
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;

        // 检查这个界面是否在界面缓存字典中
        if(!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面未打开：" + name);
            return false;
        }

        // 若已打开，则执行这个界面的 ClosePanel()
        panel.ClosePanel();
        return true;
    }


}


// 存储界面名称的常量表
public class UIConst
{
    // 新增 PackagePanel 常量
    public const string PackagePanel = "PackagePanel";
}
