using UnityEngine;

//������ ���� - �ڿ�, ���, �Һ�
public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

//������ ���� - �Һ� ������ (ü��, ���¹̳�)
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
    // �Һ� ������ ����
    public ConsumableType consumeType;

    // �Һ� ������ ȸ��������
    public float value;
    // �Һ� ������ ȿ�� ���ӽð�
    public float duration;

    // �Һ� ������ ���� ��Ÿ��
    public float curcooldown;
    // �Һ� ������ ���� ��Ÿ��(��)
    public float cooldownTime;
    // �Һ� ������ ��Ÿ�� ����
    public bool isCooldown;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string displayName; // ������ �̸�
    public string description; // ������ ����
    public ItemType itemType; // ������ ����
    public Sprite icon; // ������ ������
    public GameObject prefab; // ������ ������

    [Header("Stacking")]
    public bool canStack; // ���� ���� ����
    public int maxStackAmount; // �ִ� ���� ��

    [Header("Consumable")]
    public ItemDataConsumable consumableData; // �Һ� ������ ������
}

