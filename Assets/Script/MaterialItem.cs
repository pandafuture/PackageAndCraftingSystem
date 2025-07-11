using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialItem : MonoBehaviour
{
    // 添加 UI 属性
    private Transform UIConditionIcon;
    private Transform UIIcon;
    private Transform UIConditionText;

    private void Awake()
    {
        InitUIName();  // 初始化 UI
    }


    // 初始化 UI 组件
    private void InitUIName()
    {
        UIConditionIcon = transform.Find("ConditionIcon");
        UIIcon = transform.Find("ConditionIcon/Icon");
        UIConditionText = transform.Find("ConditionText");
    }


    // 刷新材料滚动容器的方法
    public void Refresh(int itemId, int currentCount, int requiredCount)
    {
        PackageTableItem itemData = GameManager.Instance.GetPackageItemById(itemId);

        // 更新图片
        Texture2D t = (Texture2D)Resources.Load(itemData.icon_path);
        Debug.Log("图片路径为：" + itemData.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        // 跟新名称和数量
        UIConditionText.GetComponent<Text>().text = $"{currentCount}/{requiredCount} {itemData.name}";

        // 设置颜色（不足时为红色）
        Color ziti = UIConditionText.GetComponent<Text>().color;
        UIConditionText.GetComponent<Text>().color = currentCount >= requiredCount ? ziti : Color.red;
    }
}
