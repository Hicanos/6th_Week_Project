using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // �κ��丮 ���� �迭

    public GameObject inventoryWindow; // �κ��丮 UIâ
    public Transform slotPanel; // ���� �г�
    public Transform dropPosition; // ������ ��� ��ġ

    [Header("Selected Item")]
    private ItemSlot selectedItem; // ���õ� ������ ����
    private int selectedItemIndex; // ���õ� ������ �ε���
    public TextMeshProUGUI selectedItemName; // ���õ� ������ �̸�/����/�����̸�/���Ȱ�
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    [Header("UI")]
    //UI��ư
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex; // ���� ������ ������ �ε���

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // PlayerController ���� ����

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData; // �÷��̾ ȹ���� ������ ������

        //�������� ������ ������ ���
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();


        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    //������ ���� UI ������Ʈ
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // ������ ������(���� �������� ��� ��ġ�� ����)
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    //������ ����
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName; // ���õ� ������ �̸�
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty; 
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].consumeType.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // ��Ÿ���� ���������� ��� �Ǵ�
        bool canUse = true;
        foreach (var consumable in selectedItem.item.consumables)
        {
            if (consumable.isCooldown && !consumable.IsReady())
            {
                canUse = false;
                break;
            }
        }
        useButton.SetActive(selectedItem.item.itemType == ItemType.Consumable && canUse);
        equipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);

        // ��Ÿ���� �������� �Һ� �������̸�, ��Ÿ�� ���� �� ��ư Ȱ��ȭ�� ���� �ڷ�ƾ ����
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            foreach (var consumable in selectedItem.item.consumables)
            {
                if (consumable.isCooldown && !consumable.IsReady())
                {
                    // �̹� �ڷ�ƾ�� ���� �־ �ߺ� ������ ����(��Ÿ�� ������ ��ư�� Ȱ��ȭ)
                    StartCoroutine(UseItemCooldownForSelection(consumable, selectedItem, selectedItemIndex));
                }
            }
        }
    }

    public void OnUseButton()
    {
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            foreach(var consumable in selectedItem.item.consumables)
            {
                if (consumable.isCooldown && !consumable.IsReady())
                {
                    return; // ��Ÿ���� ���������� ��� �Ұ�
                }
                StartCoroutine(UseConsumableWithCooldown(consumable));
            }
            RemoveSelectedItem();
            UpdateUI();
            SelectItem(selectedItemIndex);
            useButton.SetActive(false);
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                //UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;

        CharacterManager.Instance.Player.equip.EquipNew(selectedItem.item);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUpEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }

    // ��Ÿ���� ������ useButton�� �ٽ� Ȱ��ȭ�ϴ� �ڷ�ƾ (���õ� �������� ������ ����)
    IEnumerator UseItemCooldownForSelection(ItemDataConsumable consumable, ItemSlot refSlot, int refIndex)
    {
        yield return new WaitUntil(consumable.IsReady);

        // ��Ÿ���� ���� ������ ������ ���� �������� ���õǾ� �ְ�, �������� ���������� ��ư Ȱ��ȭ
        if (selectedItem == refSlot && selectedItemIndex == refIndex && selectedItem != null && selectedItem.item != null)
        {
            // �� consumable�� ��Ÿ���� �������� Ȯ��
            bool canUse = true;
            foreach (var consume in selectedItem.item.consumables)
            {
                if (consume.isCooldown && !consume.IsReady())
                {
                    canUse = false;
                    break;
                }
            }
            if (selectedItem.item.itemType == ItemType.Consumable && canUse)
                useButton.SetActive(true);
        }
    }

    private IEnumerator UseConsumableWithCooldown(ItemDataConsumable consumable)
    {
        // ȿ�� ����
        switch (consumable.consumeType)
        {
            case ConsumableType.Health:
                condition.Heal(consumable.value); // ü��ȸ��-�������
                break;
            case ConsumableType.Stamina:
                condition.RecoverStamina(consumable.value); // ���¹̳� ȸ��-�������
                break;
            case ConsumableType.Speed:
            case ConsumableType.JumpPower:
                if (consumable.duration > 0f)
                    StartCoroutine(condition.ApplyBuffWithDuration(consumable.consumeType, consumable.value, consumable.duration));
                // ���ǵ�/������ ����-���ӽð� ����
                break;
        }

        // ��Ÿ�� ����
        if (consumable.isCooldown)
        {
            consumable.ResetCooldown();
            while (!consumable.IsReady())
            {
                consumable.UpdateCooldown(Time.deltaTime);
                yield return null;
            }
        }
    }
}
