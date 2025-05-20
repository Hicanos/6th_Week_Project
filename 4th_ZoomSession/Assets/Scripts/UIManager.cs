using System.Collections;
using System.Collections.Generic;
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
                if (instance == null)
                {
                    instance = new GameObject() { name = "UIManager" }.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    #endregion
}
