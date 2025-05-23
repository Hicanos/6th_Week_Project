using System;
using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;
    public PlayerController controller; // �÷��̾� ��Ʈ�ѷ��� �����ϴ� ����

    Condition health { get { return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }
    public Condition speed { get { return uiCondition.speed; } }
    public Condition jumpPower { get { return uiCondition.jumpPower; } }

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

    //public void SpeedUP(float amount)
    //{
    //    //���ǵ� ����, ȿ���� ������ ���� �ӵ���(�ڷ�ƾ)
    //    //originSpeed�� 0�̸� ���� ���ǵ� ����
    //    // 0�� �ƴѵ� moveSpeed�� ���� �ٸ��� �������� ����
    //    if(originSpeed == 0 || originSpeed != controller.moveSpeed)
    //        originSpeed = controller.moveSpeed; // ���� ���ǵ� ����
    //    else return; // ���� ���ǵ�� ������ �������� ����
    //    controller.moveSpeed += amount;
    //}

    // ������ �����ϰ� �ִ� ���� ��� �����ϴ� ����
    // ���� ������ ���� ������
    public void SpeedUP()
    {

    }


    //public void JumpUP(float amount)
    //{
    //    //������ ����, ȿ���� ������ ���� ����������(�ڷ�ƾ)
    //    //originJumpPower�� 0�̸� ���� ������ ����
    //    // 0�� �ƴѵ� jumpPower�� ���� �ٸ��� �������� ����
    //    if (originJumpPower == 0 || originJumpPower != controller.jumpPower)
    //        originJumpPower = controller.jumpPower; // ���� ������ ����
    //    else return; // ���� �����°� ������ �������� ����
    //    controller.jumpPower += amount;
    //}

    public void JumpUP()
    {

    }


    public IEnumerator ApplyBuffWithDuration(ConsumableType type, float value, float duration)
    {
        switch (type)
        {
            case ConsumableType.Speed:
                    speed.curValue += value; //���ǵ� �����ġ+������ value�߰�
                controller.moveSpeed += value; //��Ʈ�ѷ� ���ǵ� �����ġ+������ value�߰�
                yield return new WaitForSeconds(duration);
                speed.curValue -= value;
                controller.moveSpeed -= value; //��Ʈ�ѷ� ���ǵ� �����ġ-������ value����
                break;
            case ConsumableType.JumpPower:
                    jumpPower.curValue += value;
                controller.jumpPower += value; //��Ʈ�ѷ� ������ �����ġ+������ value�߰�
                yield return new WaitForSeconds(duration);
                jumpPower.curValue -= value;
                controller.jumpPower -= value; //��Ʈ�ѷ� ������ �����ġ-������ value����
                break;
        }
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
