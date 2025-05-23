using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public int index; // 슬롯 인덱스
    public bool equipped;
    public int quantity;

    public Image coolDownImage; // 쿨타임 이미지

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        // 쿨타임 이미지 업데이트
        // 아이템에 쿨타임이 있는 경우 = 소모품
        // 인벤토리에서 아이템을 사용해서 코루틴이 작동되면, 실시간으로 쿨타임을 업데이트

        if (item != null && item.itemType == ItemType.Consumable && item.consumables != null)
        {
            bool anyCooldown = false;
            foreach (var consumable in item.consumables)
            {
                if (consumable.isCooldown)
                {
                    if (!consumable.IsReady())
                    {
                        anyCooldown = true;
                        coolDownImage.gameObject.SetActive(true);
                        coolDownImage.fillAmount = consumable.CurCooldown / consumable.cooldownTime;
                    }
                }
            }
            if (!anyCooldown)
                coolDownImage.gameObject.SetActive(false);
        }
        else
        {
            coolDownImage.gameObject.SetActive(false);
        }


    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
