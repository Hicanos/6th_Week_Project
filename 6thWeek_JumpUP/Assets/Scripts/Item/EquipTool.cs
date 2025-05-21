using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate; // 공격 속도
    private bool attacking; // 공격 중인지 여부
    public float attackDistance; // 공격 거리

    [Header("Resource Gathering")]
    public bool doesGatherResources; //자원 채집 가능 여부

    [Header("Combat")]
    public bool doesDealDamage; // 공격(대미지부여) 가능 여부
    public int damage; // 공격력

    public override void OnAttackInput()
    {
        // 공격 입력 처리
        Debug.Log("EquipTool Attack");
    }

}
