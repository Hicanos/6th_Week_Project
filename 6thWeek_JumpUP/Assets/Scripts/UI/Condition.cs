using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //현재 수치
    public float maxValue; //최대 수치
    public float startValue; // 시작할 때의 값
    public float passiveValue; // 기본적으로 변화하는 값
    public Image uiBar; // Image로 변환되는 수치를 담을 UI바

    private void Start()
    {
        curValue = startValue; // 시작 시 현재의 값을 초기값으로 설정
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage(); // UI바의 fillAmount를 현재 수치에 비례하여 설정
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue); 
        // 현재 수치가 최대 수치를 초과하지 않도록 설정
        // min을 이용해 둘 중 더 작은 값을 선택
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
