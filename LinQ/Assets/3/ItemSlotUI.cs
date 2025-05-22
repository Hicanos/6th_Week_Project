using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Text titleText;
    public Text attackText;
    public Text priceText;
    public Image background;

    
    private  Color commonColor = new Color32(200, 200, 200, 255); // 연회색
    private  Color rareColor = new Color32(74, 144, 226, 255);  // 파랑
    private  Color eliteColor = new Color32(39, 174, 96, 255);   // 녹색
    private  Color uniqueColor = new Color32(230, 126, 34, 255);  // 주황
    private  Color epicColor = new Color32(155, 89, 182, 255);  // 보라
    private  Color legendaryColor = new Color32(241, 196, 15, 255);  // 금색

    public void SetItem(Weapon weapon)
    {
        titleText.text = weapon.Name;
        attackText.text = $"공격력: {weapon.AttackPower}";
        priceText.text = $"가격: {weapon.Price} G";

        switch (weapon.Grade)
        {
            case Grade.Common:
                background.color = commonColor;
                break;
            case Grade.Rare:
                background.color = rareColor;
                break;
            case Grade.Elite:
                background.color = eliteColor;
                break;
            case Grade.Unique:
                background.color = uniqueColor;
                break;
            case Grade.Epic:
                background.color = epicColor;
                break;
            case Grade.Legendary:
                background.color = legendaryColor;
                break;
            default:
                background.color = Color.white;
                break;
        }
    }
}
