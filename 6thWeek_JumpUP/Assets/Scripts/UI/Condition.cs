using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //���� ��ġ
    public float maxValue; //�ִ� ��ġ
    public float startValue; // ������ ���� ��
    public float passiveValue; // �⺻������ ��ȭ�ϴ� ��
    public Image uiBar; // Image�� ��ȯ�Ǵ� ��ġ�� ���� UI��

    private void Start()
    {
        curValue = startValue; // ���� �� ������ ���� �ʱⰪ���� ����
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage(); // UI���� fillAmount�� ���� ��ġ�� ����Ͽ� ����
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue); 
        // ���� ��ġ�� �ִ� ��ġ�� �ʰ����� �ʵ��� ����
        // min�� �̿��� �� �� �� ���� ���� ����
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
