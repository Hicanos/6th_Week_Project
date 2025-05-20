using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // ĳ������ ���¸� ��Ÿ���� Ŭ����
    // �ִ��ġ, ���۰�: ���⼭�� �����ǹǷ� private
    // ���� ��ġ, �⺻������ ��ȭ�ϴ� ��: PlayerCondition���� ���� ����

    [SerializeField] private float maxValue; //�ִ� ��ġ
    [SerializeField] private float startValue; // ������ ���� ��
    public float curValue; //���� ��ġ
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
        // max�� �̿��� �� �� �� ū ���� ���� (��ġ�� 0���� �۾����� �ʵ���)
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
        // ���� ��ġ�� �ִ� ��ġ�� ������ ��ȯ, maxValue�� �׻� curValue���� ũ�ų� ����
    }
}
