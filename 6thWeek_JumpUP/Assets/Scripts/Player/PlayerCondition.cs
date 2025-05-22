using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;
    public PlayerController controller; // �÷��̾� ��Ʈ�ѷ��� �����ϴ� ����

    Condition health { get { return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }

    public float originSpeed; // ���� ���ǵ�
    public float originJumpPower; // ���� ������
    private float lastYPosition; // ������ Y��ǥ�� �����ϴ� ����



    public event Action onTakeDamage; // ����� �Ծ��� �� ȣ��Ǵ� �̺�Ʈ
    public event Action onStaminaChange; // ���¹̳� ��ȭ �� ȣ��Ǵ� �̺�Ʈ
    // ü��: ���� ������ �������� �� �Ҹ�� : ������� �ʿ�
    // ���¹̳�: �뽬, ���� ������ �Ҹ��


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

    public void RecoverStamina(float amount)
    {
        stamina.Add(amount);
        onStaminaChange?.Invoke();
    }

    public void BeTired(float amount)
    {
        stamina.Subtract(amount);
        onStaminaChange?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        // ����� �Ծ��� �� ȣ��Ǵ� �̺�Ʈ
        health.Subtract(amount);
        onTakeDamage?.Invoke();
    }

    public void SpeedUP(float amount)
    {
        //���ǵ� ����, ȿ���� ������ ���� �ӵ���(�ڷ�ƾ)
        originSpeed = controller.moveSpeed; // ���� ���ǵ� ����
        controller.moveSpeed += amount;
    }

    public void JumpUP(float amount)
    {
        //������ ����, ȿ���� ������ ���� ����������(�ڷ�ƾ)
        originJumpPower = controller.jumpPower; // ���� ������ ����
        controller.jumpPower += amount;
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
