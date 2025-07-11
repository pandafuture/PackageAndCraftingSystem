using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CraftingPanel : BasePanel
{
    // 添加 UI 属性
    // Right
    private Transform UIClosePanel;  // 关闭菜单栏
    private Transform UICloseBtn;  // 关闭按钮

    // Center
    private Transform UICenter;
    private Transform UIScrollView;
    private Transform UIDetailPanel;


    // 添加合成界面子物体预制件属性
    public GameObject CraftingUIItemPrefab;

    // 当前选中的物品是哪一个 id
    private int _chooseId;

    // 外部使用时，则使用 chooseID ，获取当前选中的物品
    public int chooseID
    {
        get
        {
            return _chooseId;
        }
        set
        {
            // 如果获取了一个新的值，就刷新整个详情界面
            _chooseId = value;
            RefreshDetail();  // 刷新详情界面
        }
    }


    protected override void Awake()
    {
        base.Awake();
        InitUI();  // 初始化 UI
    }


    private void Start()
    {
        RefreshUI();  // UI 刷新方法
    }


    private void InitUI()
    {
        InitUIName();  // 初始化 UI 组件

        InitClick();  // 初始化点击事件
    }


    // 绑定 UI 组件
    private void InitUIName()
    {
        UIClosePanel = transform.Find("Right/ClosePanel");
        UICloseBtn = transform.Find("Right/ClosePanel/CloseBtn");

        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/Scroll View");
        UIDetailPanel = transform.Find("Center/DetailPanel");
    }


    // 注册按钮点击事件
    private void InitClick()
    {
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);

    }


    // 按钮点击事件
    private void OnClickClose()
    {
        print(">>>>>  OnClickClose");
        ClosePanel();  // 关闭界面
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

        // 根据静态数据中的 武器和弹药 初始化滚动容器
        // 先获取 武器 和 弹药 数据
        List<PackageTableItem> weapons = GameManager.Instance.GetPackageTableByType(2);
        List<PackageTableItem> ammo = GameManager.Instance.GetPackageTableByType(3);
        List<PackageTableItem> weaponsAndammo = new List<PackageTableItem>();
        weaponsAndammo.AddRange(weapons);
        weaponsAndammo.AddRange(ammo);

        // 再刷新滚动容器
        foreach(PackageTableItem Item in weaponsAndammo)
        {
            Transform CraftingUIItem = Instantiate(CraftingUIItemPrefab.transform, scrollContent) as Transform;
            CraftingCell craftingCell = CraftingUIItem.GetComponent<CraftingCell>();

            // 刷新这个物品的状态
            craftingCell.Refresh(Item, this);
        }
    }


    // 刷新详情界面的方法
    private void RefreshDetail()
    {
        // 找到 id 对应的静态数据
        PackageTableItem Item = GameManager.Instance.GetPackageItemById(chooseID);

        // 刷新详情界面
        UIDetailPanel.GetComponent<CraftingDetail>().Refresh(Item, this);
    }
}
