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

    public int index; // ���� �ε���
    public bool equipped;
    public int quantity;

    public Image coolDownImage; // ��Ÿ�� �̹���

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        // ��Ÿ�� �̹��� ������Ʈ
        // �����ۿ� ��Ÿ���� �ִ� ��� = �Ҹ�ǰ
        // �κ��丮���� �������� ����ؼ� �ڷ�ƾ�� �۵��Ǹ�, �ǽð����� ��Ÿ���� ������Ʈ

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
