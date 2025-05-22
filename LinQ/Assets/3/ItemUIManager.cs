using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemUIManager : MonoBehaviour
{


    public GameObject itemPrefab;
    public Transform itemParent;

    private List<Weapon> allWeapons;

 
    private Func<List<Weapon>, List<Weapon>> sortFunction;

    void Start()
    {
        allWeapons = WeaponDatabase.weapons;

   
        sortFunction = weapons => weapons.OrderBy(w => w.Name).ToList();

        RefreshItemList();
    }

    public void SetSortFunction(Func<List<Weapon>, List<Weapon>> func)
    {
        sortFunction = func;
        RefreshItemList();
    }

    public void RefreshItemList()
    {
        List<Weapon> sorted;

        if (sortFunction != null)
        {
            sorted = sortFunction(allWeapons);
        }
        else
        {
            sorted = allWeapons;
        }

        ShowItems(sorted);
    }

    private void ShowItems(List<Weapon> weapons)
    {
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Weapon weapon in weapons)
        {
            GameObject go = Instantiate(itemPrefab, itemParent);
            go.GetComponent<ItemSlotUI>().SetItem(weapon);
        }
    }


}
