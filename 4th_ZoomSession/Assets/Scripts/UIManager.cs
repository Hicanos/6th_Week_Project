using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region ��Ŭ�� ����
    //Singleton instance

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            // ���̾��Ű���� UIManager�� ã�� ������ ���� ����
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();                
            }
            if (instance == null)
            {
                instance = new GameObject() { name = "UIManager" }.AddComponent<UIManager>();
            }
            return instance;
        }
    }
    #endregion

    private Dictionary<string, UIBase> _uiList = new();

    private string GetUIName<T>() where T : UIBase
    {
        return typeof(T).Name;
    }

    /// <summary>
    /// ���Ӿ��� UI�� ����
    /// </summary>
    /// <typeparam name="T">�����Ϸ��� UI Ÿ��</typeparam>
    /// <returns>������ UI �ν��Ͻ�</returns>
    public T Open<T>() where T : UIBase
    {
        string uiName = GetUIName<T>();

        if (_uiList.ContainsKey(uiName) == false) // UI�� ������ �� ����
        {
            T prefab = Resources.Load<T>($"UI/{uiName}");

            if (prefab == null)
                throw new Exception($"Resources/UI/{uiName} �������� �������� �ʽ��ϴ�.");

            T newUI = Instantiate(prefab);

            _uiList[uiName] = newUI;

            return newUI;
        }

        UIBase ui = _uiList[uiName];
        ui.Open();
        return ui as T;
    }

    /// <summary>
    /// ���� �����ϴ� UI�� ����
    /// </summary>
    /// <param name="kill">��Ȱ��ȭ�� �ƴ� ������Ʈ �ı�</param>
    /// <typeparam name="T">�������� �ϴ� UI Ÿ��</typeparam>
    public void Close<T>(bool kill = false) where T : UIBase
    {
        string uiName = GetUIName<T>();

        if (_uiList.TryGetValue(uiName, out UIBase ui) == false)
            return;

        if (kill)
        {
            Destroy(ui.gameObject);
            _uiList.Remove(uiName);
            return;
        }

        ui.Close();
    }

    /// <summary>
    /// ����Ʈ�� �ִ� UI�� �ν��Ͻ��� ��ȯ
    /// </summary>
    /// <param name="ui">�����Ϸ��� UI�� �ν��Ͻ�</param>
    /// <typeparam name="T">�����Ϸ��� UI Ÿ��</typeparam>
    /// <returns>����Ʈ�� UI�� �ִ��� ����</returns>
    public bool TryGet<T>(out T ui) where T : UIBase
    {
        ui = null;

        string uiName = GetUIName<T>();

        if (_uiList.TryGetValue(uiName, out UIBase spawnedUI) == false)
            return false;

        ui = spawnedUI as T;

        return true;
    }
}
