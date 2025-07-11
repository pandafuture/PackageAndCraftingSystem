using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingCell : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // 添加 UI 属性
    private Transform UIItem;
    private Transform UISelect;
    private Transform UIBottom;
    private Transform UIIconName;
    private Transform UICenter;
    private Transform UIIcon;

    private PackageLocalItem PackageLocalData;  // 当前物品的动态数据
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
        UISelect = transform.Find("Item/Select");

        UISelect.gameObject.SetActive(false);
    }


    // 刷新物品状态的方法
    public void Refresh(PackageTableItem packageItem, CraftingPanel uiParent)
    {
        // 数据初始化
        this.PackageLocalData = GameManager.Instance.GetPackageLocalDataById(packageItem.id);
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


    // 实现鼠标点击的回调方法
    public void OnPointerClick(PointerEventData eventData)
    {
        // 打印当前执行的方法名，以及这一数据
        Debug.Log("OnPonterClick: " + eventData.ToString());

        // 先关闭之前的选中效果
        GameObject select = GameObject.Find("Select");
        if (select != null)
        {
            select.SetActive(false);
        }

        // 选中效果
        UISelect.gameObject.SetActive(true);

        // 判断当前点击选中的物品是否和父物品的 uid 一样，若一样则为重复点击，不执行逻辑
        if (this.uiParent.chooseID == this.packageTableData.id)
            return;
        // 若不一样，则代表点击到了新物品身上，
        this.uiParent.chooseID = this.packageTableData.id;
    }


    // 实现鼠标进入的回调方法
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter: " + eventData.ToString());
    }


    // 实现鼠标退出的回调方法
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit: " + eventData.ToString());
    }

}
