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
    // �Һ� ������ ȿ�� ���ӽð� (0�̸� ��� ȿ��)
    public float duration;

    // �Һ� ������ ��Ÿ�� ����
    public bool isCooldown;    
    // �Һ� ������ ���� ��Ÿ��(��)
    public float cooldownTime;
    // �Һ� ������ ���� ��Ÿ��
    private float curcooldown;
    public float CurCooldown => curcooldown; // �б� ���� ������Ƽ
    public void ResetCooldown() => curcooldown = cooldownTime; // ��Ÿ�� �ʱ�ȭ
    public void UpdateCooldown(float deltaTime) // ��Ÿ�� ������Ʈ
    {
        if (curcooldown > 0)
            curcooldown = Mathf.Max(0, curcooldown - deltaTime); //��Ÿ���� 0���� �۾����� �ʵ��� ó����
    }
    public bool IsReady() => curcooldown <= 0; // ��Ÿ���� 0���� Ȯ���ϴ� �޼��� (��Ÿ���� 0�̸� ��� ������)

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

 
    // �Һ� ������ ������, itemType�� Consumable�� ���� ���    
    [Header("Consumable")]
    public ItemDataConsumable consumableData;
    // Consumable Ÿ�� ���� Ȯ��
    public bool IsConsumable => itemType == ItemType.Consumable;
    //Consumable Ÿ���� ���� consumableData ���
    public ItemDataConsumable GetConsumableData() => IsConsumable ? consumableData : null;
}

