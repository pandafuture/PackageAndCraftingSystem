using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// 继承 点击、进入、退出回调方法的接口
public class PackageCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // 添加物品 UI 属性
    private Transform UIIcon;
    private Transform UIIconName;
    private Transform UINum;
    private Transform UISelect;

    private Transform UIZhuangbeikuang;
    private Transform UIEmpty;


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

        UIZhuangbeikuang = transform.Find("Item/zhuangbeikuang");
        UIEmpty = transform.Find("Item/Empty");
    }


    // 刷新物品状态的方法
    public void Refresh(PackageLocalItem packageLcoalData, PackagePanel uiParent)
    {
        // 确保物品 UI 可见
        UIIcon.gameObject.SetActive(true);
        UIIconName.gameObject.SetActive(true);
        UINum.gameObject.SetActive(true);
        UIZhuangbeikuang.gameObject.SetActive(true);
        UIEmpty.gameObject.SetActive(false);
        this.enabled = true;


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


    // 设置空格状态的方法
    public void SetEmpty()
    {
        // 隐藏物品相关 UI
        UIIcon.gameObject.SetActive(false);
        UIIconName.gameObject.SetActive(false);
        UINum.gameObject.SetActive(false);
        UIZhuangbeikuang.gameObject.SetActive(false);

        // 显示空格图片
        UIEmpty.gameObject.SetActive(true);

        // 禁用交互
        this.enabled = false;
    }


    // 实现鼠标点击的回调方法
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick：" + eventData.ToString());
    }


    // 实现鼠标进入的回调方法
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter：" + eventData.ToString());
    }


    // 实现鼠标退出的回调方法
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit：" + eventData.ToString());
    }



}
