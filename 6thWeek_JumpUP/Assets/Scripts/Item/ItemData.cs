using UnityEngine;

//아이템 종류 - 자원, 장비, 소비
public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

//아이템 종류 - 소비 아이템 (체력, 스태미나)
public enum ConsumableType
{
    Health,
    Stamina,
    Speed,
    JumpPower
}

[System.Serializable]
public class ItemDataConsumable
{
    // 소비 아이템 종류
    public ConsumableType consumeType;

    // 소비 아이템 회복증감량
    public float value;
    // 소비 아이템 효과 지속시간 (0이면 즉시 효과)
    public float duration;

    // 소비 아이템 쿨타임 여부
    public bool isCooldown;    
    // 소비 아이템 지정 쿨타임(초)
    public float cooldownTime;
    // 소비 아이템 현재 쿨타임
    private float curcooldown;
    public float CurCooldown => curcooldown; // 읽기 전용 프로퍼티
    public void ResetCooldown() => curcooldown = cooldownTime; // 쿨타임 초기화
    public void UpdateCooldown(float deltaTime) // 쿨타임 업데이트
    {
        if (curcooldown > 0)
            curcooldown = Mathf.Max(0, curcooldown - deltaTime); //쿨타임이 0보다 작아지지 않도록 처리함
    }
    public bool IsReady() => curcooldown <= 0; // 쿨타임이 0인지 확인하는 메서드 (쿨타임이 0이면 사용 가능함)

}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string displayName; // 아이템 이름
    public string description; // 아이템 설명
    public ItemType itemType; // 아이템 종류
    public Sprite icon; // 아이템 아이콘
    public GameObject prefab; // 아이템 프리팹

    [Header("Stacking")]
    public bool canStack; // 스택 가능 여부
    public int maxStackAmount; // 최대 스택 수

 
    // 소비 아이템 데이터, itemType이 Consumable일 때만 사용    
    [Header("Consumable")]
    public ItemDataConsumable consumableData;
    // Consumable 타입 여부 확인
    public bool IsConsumable => itemType == ItemType.Consumable;
    //Consumable 타입일 때만 consumableData 사용
    public ItemDataConsumable GetConsumableData() => IsConsumable ? consumableData : null;
}

