using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCell : MonoBehaviour
{
    // 添加 UI 属性
    private Transform UIItem;
    private Transform UISelect;
    private Transform UIBottom;
    private Transform UIIconName;
    private Transform UICenter;
    private Transform UIIcon;

    private PackageTableItem packageTableData;  // 当前物品的静态数据
    private CraftingPanel uiParent;  // 当前物品的父物品（CraftingPanel)


    private void Awake()
    {
        InitUIName();  // 初始化 UI 组件
    }


    // 获取 UI 组件
    private void InitUIName()
    {
        UIItem = transform.Find("Item");
        UIIconName = transform.Find("Item/Bottom/IconName");
        UIIcon = transform.Find("Item/Center/Icon");
    }


    // 刷新物品状态的方法
    public void Refresh(PackageTableItem packageItem, CraftingPanel uiParent)
    {
        // 数据初始化
        this.packageTableData = packageItem;
        this.uiParent = uiParent;

        // 对 UI 组件的信息进行初始化
        // 物品名字
        UIIconName.GetComponent<Text>().text = this.packageTableData.name.ToString();

        // 物品图片
        Texture2D t = (Texture2D)Resources.Load(this.packageTableData.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;
    }
}
