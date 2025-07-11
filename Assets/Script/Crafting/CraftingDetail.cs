using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CraftingDetail : MonoBehaviour
{
    // 添加 UI 属性
    // Detail_Top
    private Transform UIDetail_Top;
    private Transform UIDetail_Top_Icon;
    private Transform UIDetail_Top_IconName;
    private Transform UIDetail_Top_Num;
    private Transform UIDetail_Top_DetailText;
    // Detail_Center
    private Transform UIDetail_Center;
    // Detail_Center_Bottom
    private Transform UIDetail_Center_Bottom;
    private Transform UIDetail_Center_Bottom_ScrollView;
    // Detail_Bottom
    private Transform UIDetail_Bottom;
    // Detail_Bottom_Center
    private Transform UIDetail_Bottom_Center_CraftingBtn;
    private Transform UIDetail_Bottom_Center_CraftingText;
    // Detail_Bottom_Left
    private Transform UIDetail_Bottom_Left_SubtractionBtn;
    // Detail_Bottom_Right
    private Transform UIDetail_Bottom_Right_AdditionBtn;


    // 拿到这个物品的动态信息、静态信息、uiParent
    //private PackageLocalItem packageLocalData;
    private PackageTableItem packageTableItem;
    private CraftingPanel uiParent;


    // 当前制作数量
    private int craftCount = 1;
    // 当前选中的物品
    private PackageTableItem currentItem;
    // 当前配方
    private Recipe currentRecipe;

    // 材料项预制体
    public GameObject materialPrefab;



    private void Awake()
    {
        InitUIName();  // 初始化 UI

        InitClick();  // 初始化点击事件

        // 测试
        //Refresh(GameManager.Instance.GetPackageLocalData()[0], null);
    }


    // 绑定 UI 组件
    private void InitUIName()
    {
        UIDetail_Top = transform.Find("Top");
        UIDetail_Top_Icon = transform.Find("Top/Icon");
        UIDetail_Top_IconName = transform.Find("Top/IconName");
        UIDetail_Top_Num = transform.Find("Top/Num");
        UIDetail_Top_DetailText = transform.Find("Top/DetailText");

        UIDetail_Center = transform.Find("Center");
        UIDetail_Center_Bottom = transform.Find("Center/Bottom");
        UIDetail_Center_Bottom_ScrollView = transform.Find("Center/Bottom/Scroll View");

        UIDetail_Bottom = transform.Find("Bottom");
        UIDetail_Bottom_Center_CraftingBtn = transform.Find("Bottom/Center/CraftingBtn");
        UIDetail_Bottom_Center_CraftingText = transform.Find("Bottom/Center/CraftingText");
        UIDetail_Bottom_Left_SubtractionBtn = transform.Find("Bottom/Left/Subtraction");
        UIDetail_Bottom_Right_AdditionBtn = transform.Find("Bottom/Right/Addition");

    }


    // 注册按钮点击事件
    private void InitClick()
    {
        UIDetail_Bottom_Center_CraftingBtn.GetComponent<Button>().onClick.AddListener(OnClickCrafting);
        UIDetail_Bottom_Left_SubtractionBtn.GetComponent<Button>().onClick.AddListener(OnClickSubtracton);
        UIDetail_Bottom_Right_AdditionBtn.GetComponent<Button>().onClick.AddListener(OnClickAddition);

    }


    // 按钮点击事件
    // 制作按钮
    private void OnClickCrafting()
    {
        print(">>>>>  OnClickCrafting");

        if (currentRecipe == null)
            return;

        // 检查材料是否足够
        if (!HasEnoughMaterials())
        {
            Debug.Log("材料不足，无法制作");
            return;
        }

        // 扣除材料
        DeductMaterial(MaterialType.shengtie, currentRecipe.shengtie);
        DeductMaterial(MaterialType.mutou, currentRecipe.mutou);
        DeductMaterial(MaterialType.jiaodai, currentRecipe.jiaodai);
        DeductMaterial(MaterialType.masheng, currentRecipe.masheng);
        DeductMaterial(MaterialType.yumao, currentRecipe.yumao);

        Debug.Log($"要制作的数量为：{craftCount}");
        // 添加成品
        GameManager.Instance.AddItem(currentRecipe.result_item_id, craftCount);


        // 更新当前物品数据
        currentItem = GameManager.Instance.GetPackageItemById(currentRecipe.result_item_id);
        // 刷新 UI 
        Refresh(currentItem, uiParent);
    }

    // 减少按钮
    private void OnClickSubtracton()
    {
        print(">>>>>  OnClickSubtracton");
        craftCount = Mathf.Max(1, craftCount - 1);

        RefreshScroll();
        RefreshCraftCount();
    }

    // 增加按钮
    private void OnClickAddition()
    {
        print(">>>>>  OnClickAddition");
        craftCount = Mathf.Max(1, craftCount + 1);

        RefreshScroll();
        RefreshCraftCount();
    }


    // 刷新详情界面的方法
    public void Refresh(PackageTableItem packageData, CraftingPanel uiParent)
    {
        // 初始化：动态数据、静态数据、父物体逻辑
        //this.packageLocalData = GameManager.Instance.GetPackageLocalDataById(packageData.id);
        this.packageTableItem = packageData;
        this.uiParent = uiParent;


        // 获取当前物品的配方
        currentRecipe = GameManager.Instance.GetRecipeByResultId(packageData.id);


        // 初始化 UI 组件的信息
        // 物品名称
        UIDetail_Top_IconName.GetComponent<Text>().text = string.Format(this.packageTableItem.name.ToString());

        // 物品数量
        UIDetail_Top_Num.GetComponent<Text>().text = string.Format("拥有：{0}", GameManager.Instance.GetItemCount(packageData.id));

        // 详细描述
        UIDetail_Top_DetailText.GetComponent<Text>().text = string.Format(this.packageTableItem.description.ToString());

        // 武器图片
        Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIDetail_Top_Icon.GetComponent<Image>().sprite = temp;
        Debug.Log("已刷新详情界面");

        RefreshScroll();  // 刷新滚动容器
        RefreshCraftCount();  // 刷新制作数量的方法
    }


    // 刷新滚动容器的方法
    private void RefreshScroll()
    {
        // 先清理滚动容器中原本的物品
        RectTransform scrollContent = UIDetail_Center_Bottom_ScrollView.GetComponent<ScrollRect>().content;
        for(int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        // 若当前配方为空就返回
        if (currentRecipe == null)
        {
            Debug.LogError("当前配方为空");
            return;
        }

        // 添加材料项
        AddMaterialItem(scrollContent, MaterialType.shengtie, currentRecipe.shengtie);
        AddMaterialItem(scrollContent, MaterialType.mutou, currentRecipe.mutou);
        AddMaterialItem(scrollContent, MaterialType.jiaodai, currentRecipe.jiaodai);
        AddMaterialItem(scrollContent, MaterialType.masheng, currentRecipe.masheng);
        AddMaterialItem(scrollContent, MaterialType.yumao, currentRecipe.yumao);
    }


    // 添加材料项的方法
    private void AddMaterialItem(Transform parent, int materialId,int? requiredCount)
    {
        // 该材料所需数量为 0，就不添加
        if (!requiredCount.HasValue || requiredCount <= 0)
            return;

        // 实例化材料项
        GameObject materialItem = Instantiate(materialPrefab, parent);
        MaterialItem item = materialItem.GetComponent<MaterialItem>();

        // 获取数量
        int currentCount = GameManager.Instance.GetItemCount(materialId);
        int totalRequired = requiredCount.Value * craftCount;

        item.Refresh(materialId, currentCount, totalRequired);
    }


    // 刷新制作数量的方法
    private void RefreshCraftCount()
    {
        UIDetail_Bottom_Center_CraftingText.GetComponent<Text>().text = string.Format("制作X" + craftCount.ToString());
    }


    // 检查所需材料是否足够的方法
    private bool HasEnoughMaterials()
    {
        return CheckMaterial(MaterialType.shengtie, currentRecipe.shengtie) &&
               CheckMaterial(MaterialType.mutou, currentRecipe.mutou) &&
               CheckMaterial(MaterialType.jiaodai, currentRecipe.jiaodai) &&
               CheckMaterial(MaterialType.masheng, currentRecipe.masheng) &&
               CheckMaterial(MaterialType.yumao, currentRecipe.yumao);
    }

    // 检查某一材料是否足够的方法
    private bool CheckMaterial(int materialId, int? require)
    {
        if (!require.HasValue || require <= 0)
            return true;

        int available = GameManager.Instance.GetItemCount(materialId);
        return available >= require.Value * craftCount;
    }

    
    // 扣除所需材料的方法
    private void DeductMaterials()
    {
        DeductMaterial(MaterialType.shengtie, currentRecipe.shengtie);
        DeductMaterial(MaterialType.mutou, currentRecipe.mutou);
        DeductMaterial(MaterialType.jiaodai, currentRecipe.jiaodai);
        DeductMaterial(MaterialType.masheng, currentRecipe.masheng);
        DeductMaterial(MaterialType.yumao, currentRecipe.yumao);
    }

    // 扣除某一材料的方法
    private void DeductMaterial(int materialId, int? amount)
    {
        if (!amount.HasValue || amount <= 0)
            return;
        
        GameManager.Instance.RemoveItem(materialId, amount.Value * craftCount);
    }
}
