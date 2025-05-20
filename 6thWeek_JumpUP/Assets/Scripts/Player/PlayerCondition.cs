using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage; // ����� �Ծ��� �� ȣ��Ǵ� �̺�Ʈ

    private void Update()
    {
        // ü�°� ���¹̳��� �ڿ� ȸ��
        health.Add(health.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);


        if (health.curValue < 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Die() //ĳ���� ����� �۵��ϴ� ����
    {
        // ĳ���� ��� �� ȭ�鿡 �޼��� ���, Restart Ȥ�� Exit ��ư�� ������ ��
        // Restart ��ư�� ������ ������ �ٽ� ���۵�
        // Exit ��ư�� ������ ������ �����
        // UI�Ŵ������� UI�� Ȱ��ȭ ��Ű�� ���ӸŴ������� ���� ����
        Debug.Log("�÷��̾ �׾���.");
    }
}
