using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCell : MonoBehaviour
{
    // 添加物品 UI 属性
    private Transform UIIcon;
    private Transform UIIconName;
    private Transform UINum;
    private Transform UISelect;


    private void Awake()
    {
        InitUIName();
    }


    private void InitUIName()
    {
        // 绑定 UI 组件
        UIIcon = transform.Find("Item/Center/Icon");
        UIIconName = transform.Find("Item/Bottom/IconName");
        UINum = transform.Find("Item/Top/Num");
        UISelect = transform.Find("Item/Select");
    }
}
