using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region 싱클톤 구현
    //Singleton instance

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            // 하이어라키에서 UIManager를 찾고 없으면 새로 생성
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
    /// 게임씬에 UI를 생성
    /// </summary>
    /// <typeparam name="T">생성하려는 UI 타입</typeparam>
    /// <returns>생성된 UI 인스턴스</returns>
    public T Open<T>() where T : UIBase
    {
        string uiName = GetUIName<T>();

        if (_uiList.ContainsKey(uiName) == false) // UI가 생성된 적 없음
        {
            T prefab = Resources.Load<T>($"UI/{uiName}");

            if (prefab == null)
                throw new Exception($"Resources/UI/{uiName} 프리팹이 존재하지 않습니다.");

            T newUI = Instantiate(prefab);

            _uiList[uiName] = newUI;

            return newUI;
        }

        UIBase ui = _uiList[uiName];
        ui.Open();
        return ui as T;
    }

    /// <summary>
    /// 씬에 존재하는 UI를 닫음
    /// </summary>
    /// <param name="kill">비활성화가 아닌 오브젝트 파괴</param>
    /// <typeparam name="T">닫으려고 하는 UI 타입</typeparam>
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
    /// 리스트에 있는 UI의 인스턴스를 반환
    /// </summary>
    /// <param name="ui">참조하려는 UI의 인스턴스</param>
    /// <typeparam name="T">참조하려는 UI 타입</typeparam>
    /// <returns>리스트에 UI가 있는지 여부</returns>
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
