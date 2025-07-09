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



    override protected void Awake()
    {
        base.Awake();
        InitUI();
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
}
