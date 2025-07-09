using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel : BasePanel
{
    // 对各个 UI 组件初始化
    // LeftTop
    private Transform UIPackageText;

    // Right
    private Transform UICloseBtn;

    // Center
    private Transform UICenter;
    private Transform UIScrollView;

    // 添加背包子物体预制件属性
    public GameObject PackageUIItemPrefab;



    override protected void Awake()
    {
        base.Awake();
        InitUI();  // 初始化 UI

        // 初始化背包子物品的预制件
        InitPrefab();
    }

    private void Start()
    {
        RefreshUI();  // UI 刷新方法
    }


    private void InitUI()
    {
        InitUIName();
    }


    // 初始化 UI 组件
    private void InitUIName()
    {
        // 绑定 UI 组件
        UIPackageText = transform.Find("LeftTop/PackageText");

        UICloseBtn = transform.Find("Right/ClosePanel/CloseBtn");

        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/Scroll View");
    }


    // 初始化背包子物品的预制件
    private void InitPrefab()
    {
        PackageUIItemPrefab = Resources.Load("Prefab/Panel/Package/PackageUIItem") as GameObject;
    }


    // 注册按钮点击事件
    private void InitClick()
    {
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
    }


    // 添加点击事件
    private void OnClickClose()
    {
        print(">>>>> OnClickClose");
    }


    // UI 刷新方法
    private void RefreshUI()
    {
        RefreshScroll();  // 刷新滚动容器
    }


    // 刷新滚动容器的方法
    private void RefreshScroll()
    {
        // 先清理滚动容器中原本的物品
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        // 根据本地数据初始化滚动容器
        foreach(PackageLocalItem localData in GameManager.Instance.GetSortPackageLocalData())
        {
            Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
            PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();
        }
    }
}
