using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate; // ���� �ӵ�
    private bool attacking; // ���� ������ ����
    public float attackDistance; // ���� �Ÿ�

    [Header("Resource Gathering")]
    public bool doesGatherResources; //�ڿ� ä�� ���� ����

    [Header("Combat")]
    public bool doesDealDamage; // ����(������ο�) ���� ����
    public int damage; // ���ݷ�

    public override void OnAttackInput()
    {
        // ���� �Է� ó��
        Debug.Log("EquipTool Attack");
    }

}
