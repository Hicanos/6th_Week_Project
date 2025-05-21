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
    // 소비 아이템 효과 지속시간
    public float duration;

    // 소비 아이템 현재 쿨타임
    public float curcooldown;
    // 소비 아이템 지정 쿨타임(초)
    public float cooldownTime;
    // 소비 아이템 쿨타임 여부
    public bool isCooldown;
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

    [Header("Consumable")]
    public ItemDataConsumable consumableData; // 소비 아이템 데이터
}

