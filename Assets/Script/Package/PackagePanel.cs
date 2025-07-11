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
        InitUIName();  // 初始化 UI 组件

        InitClick();  // 注册点击事件
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
    // PackagePanel 界面的关闭按钮
    private void OnClickClose()
    {
        print(">>>>> OnClickClose");
        ClosePanel();
    }


    // UI 刷新方法
    private void RefreshUI()
    {
        RefreshScroll();  // 刷新滚动容器

    }


    // 刷新滚动容器的方法
    private void RefreshScroll()
    {
        int packagecellnum = 0;  // 背包格使用计数器 

        // 先清理滚动容器中原本的物品
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        // 创建 30 个背包格
        for(int i = 0; i < 30; i++)
        {
            Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
            PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();

            // 检查当前格是否有物品
            if(i < GameManager.Instance.GetSortPackageLocalData().Count)
            {
                // 刷新这个物品的状态
                packageCell.Refresh(GameManager.Instance.GetSortPackageLocalData()[i], this);
                packagecellnum++;
            }
            else
            {
                // 空格状态
                packageCell.SetEmpty();
            }
        }

        // 刷新背包格
        UIPackageText.GetComponent<Text>().text = "背包（" + packagecellnum + "/30）";

        //// 根据本地数据初始化滚动容器
        //foreach(PackageLocalItem localData in GameManager.Instance.GetSortPackageLocalData())
        //{
        //    Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
        //    PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();

        //    // 刷新这个物品的状态
        //    packageCell.Refresh(localData, this);
        //    packagecellnum++;
        //}


        //// 刷新背包格
        //UIPackageText.GetComponent<Text>().text = "背包（" + packagecellnum + "/30）";
    }
}
