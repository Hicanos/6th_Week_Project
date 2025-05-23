using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // 인벤토리 슬롯 배열

    public GameObject inventoryWindow; // 인벤토리 UI창
    public Transform slotPanel; // 슬롯 패널
    public Transform dropPosition; // 아이템 드랍 위치

    [Header("Selected Item")]
    private ItemSlot selectedItem; // 선택된 아이템 슬롯
    private int selectedItemIndex; // 선택된 아이템 인덱스
    public TextMeshProUGUI selectedItemName; // 선택된 아이템 이름/설명/스탯이름/스탯값
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    [Header("UI")]
    //UI버튼
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex; // 현재 장착된 아이템 인덱스

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

    // PlayerController 먼저 수정

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData; // 플레이어가 획득한 아이템 데이터

        //아이템이 스택이 가능한 경우
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

    //아이템 슬롯 UI 업데이트
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

    // 아이템 버리기(버린 아이템을 드랍 위치에 생성)
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    //아이템 선택
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName; // 선택된 아이템 이름
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty; 
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].consumeType.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // 쿨타임이 적용중인지 즉시 판단
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

        // 쿨타임이 적용중인 소비 아이템이면, 쿨타임 종료 후 버튼 활성화를 위해 코루틴 시작
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            foreach (var consumable in selectedItem.item.consumables)
            {
                if (consumable.isCooldown && !consumable.IsReady())
                {
                    // 이미 코루틴이 돌고 있어도 중복 실행은 무방(쿨타임 끝나면 버튼만 활성화)
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
                    return; // 쿨타임이 남아있으면 사용 불가
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

    // 쿨타임이 끝나면 useButton을 다시 활성화하는 코루틴 (선택된 아이템이 동일할 때만)
    IEnumerator UseItemCooldownForSelection(ItemDataConsumable consumable, ItemSlot refSlot, int refIndex)
    {
        yield return new WaitUntil(consumable.IsReady);

        // 쿨타임이 끝난 시점에 여전히 같은 아이템이 선택되어 있고, 아이템이 남아있으면 버튼 활성화
        if (selectedItem == refSlot && selectedItemIndex == refIndex && selectedItem != null && selectedItem.item != null)
        {
            // 각 consumable이 쿨타임이 끝났는지 확인
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
        // 효과 적용
        switch (consumable.consumeType)
        {
            case ConsumableType.Health:
                condition.Heal(consumable.value); // 체력회복-즉시적용
                break;
            case ConsumableType.Stamina:
                condition.RecoverStamina(consumable.value); // 스태미나 회복-즉시적용
                break;
            case ConsumableType.Speed:
            case ConsumableType.JumpPower:
                if (consumable.duration > 0f)
                    StartCoroutine(condition.ApplyBuffWithDuration(consumable.consumeType, consumable.value, consumable.duration));
                // 스피드/점프력 증가-지속시간 적용
                break;
        }

        // 쿨타임 시작
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
