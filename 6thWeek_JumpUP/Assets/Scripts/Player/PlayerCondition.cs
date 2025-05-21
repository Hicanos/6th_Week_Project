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



    //public event Action onTakeDamage; // ����� �Ծ��� �� ȣ��Ǵ� �̺�Ʈ
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

    private void LateUpdate()
    {
        //if ()
        //{
        //    float fallDistance = lastYPosition - transform.position.y;
        //    if (fallDistance > 5f) // ����: 5���� �̻� ���� �� �����
        //        health.Subtract(fallDistance * 2f);
        //}
        //else
        //{
        //    lastYPosition = transform.position.y;
        //}
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void RecoverStamina(float amount)
    {
        stamina.Add(amount);
    }

    public void BeTired(float amount)
    {
        stamina.Subtract(amount);
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
