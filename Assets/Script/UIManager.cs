using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �糡����ȫ�� UI ������
public class UIManager
{
    // ����ģʽ
    private static UIManager _instance;

    //�����ϵ���ñ�
    private Dictionary<string, string> pathDict;

    // UI ������ڵ�
    private Transform _uiRoot;

    // Ԥ�Ƽ������ֵ�
    private Dictionary<string, GameObject> prefabDict;

    // ���滺���ֵ䣬�洢��ǰ�Ѵ򿪵Ľ���
    public Dictionary<string, BasePanel> panelDict;


    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }


    private UIManager()
    {
        InitDicts();  // ��ʼ�������ϵ���ñ�
    }


    // ��ʼ�������ϵ���ñ���
    private void InitDicts()
    {
        // ��ʼ��Ԥ�Ƽ��ֵ�
        prefabDict = new Dictionary<string, GameObject>();

        // ��ʼ�����滺���ֵ�
        panelDict = new Dictionary<string, BasePanel>();

        // �ѽ���·�����õ��ֵ���
        pathDict = new Dictionary<string, string>()
        {
            // ���� PackagePanel ��Ӧ��·��
            {UIConst.PackagePanel, "Package/PackagePanel" },
        };
    }


    // ���� UI ������ڵ�
    public Transform UIRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                // �� Canvas ��Ϊ���ڵ�
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }


    // �򿪽���ķ���
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;

        // �������Ƿ��
        if(panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("�����Ѵ򿪣�" + name);
            return null;
        }

        // ���·���Ƿ�������
        string path = "";
        if(!pathDict.TryGetValue(name, out path))
        {
            Debug.Log("�������ƴ��󣬻���δ����·����" + name);
            return null;
        }

        // ����ʹ�û���Ľ���Ԥ�Ƽ�
        GameObject panelPrefab = null;
        // ͨ��Ԥ�Ƽ������ֵ䣬�鿴�Ƿ񱻼��ع�
        if(!prefabDict.TryGetValue(name, out panelPrefab))
        {
            // δ���أ�����ز�����Ԥ�Ƽ������ֵ�
            string realPath = "Prefab/Panel/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        // �򿪽���
        // ��Ԥ�Ƽ����س����������� UIRoot ��
        GameObject PanelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = PanelObject.GetComponent<BasePanel>();

        // ��ӵ����滺���ֵ��У���ʾ��������Ѿ�����
        panelDict.Add(name, panel);
        panel.OpenPanel(name);
        return panel;
    }


    // �رս���ķ���
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;

        // �����������Ƿ��ڽ��滺���ֵ���
        if(!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("����δ�򿪣�" + name);
            return false;
        }

        // ���Ѵ򿪣���ִ���������� ClosePanel()
        panel.ClosePanel();
        return true;
    }


}


// �洢�������Ƶĳ�����
public class UIConst
{
    // ���� PackagePanel ����
    public const string PackagePanel = "PackagePanel";
}
