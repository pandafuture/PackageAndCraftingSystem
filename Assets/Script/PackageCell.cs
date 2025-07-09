using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour
{
    // 添加物品 UI 属性
    private Transform UIIcon;
    private Transform UIIconName;
    private Transform UINum;
    private Transform UISelect;


    private PackageLocalItem packageLocalData;  // 当前物品的动态数据
    private PackageTableItem packageTableItem;  // 当前物品的静态数据
    private PackagePanel uiParent;  // 当前物品的父物品（PackagePanel）


    private void Awake()
    {
        InitUIName();  // 初始化 UI
    }


    // 获取 UI 组件
    private void InitUIName()
    {
        // 绑定 UI 组件
        UIIcon = transform.Find("Item/Center/Icon");
        UIIconName = transform.Find("Item/Bottom/IconName");
        UINum = transform.Find("Item/Top/Num");
        UISelect = transform.Find("Item/Select");
    }


    // 刷新物品状态的方法
    public void Refresh(PackageLocalItem packageLcoalData, PackagePanel uiParent)
    {
        // 数据初始化
        this.packageLocalData = packageLcoalData;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLocalData.ID);
        this.uiParent = uiParent;


        // 对 UI 组件的信息进行初始化
        // 物品名称
        UIIconName.GetComponent<Text>().text = this.packageTableItem.name.ToString();

        // 物品的图片
        Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        // 物品的数量
        UINum.GetComponent<Text>().text = this.packageLocalData.NUM.ToString();
    }
}
