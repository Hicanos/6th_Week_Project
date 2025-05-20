using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

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

    public void Die()
    {
        Debug.Log("�÷��̾ �׾���.");
    }
}
