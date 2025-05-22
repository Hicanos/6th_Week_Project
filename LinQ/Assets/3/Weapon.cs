using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public enum WeaponType
{ Sword, Bow, Gun, Lance }

public enum Grade
{ Common, Rare, Elite, Unique, Epic, Legendary }

[System.Serializable]
public class Weapon 
{
    public string Name;
    public WeaponType Type;
    public Grade Grade;
    public int AttackPower;
    public int RequiredLevel;
    public int Price;
}



public static class WeaponDatabase
{
    public static readonly List<Weapon> weapons =    new List<Weapon>
    {
        new Weapon { Name = "불의 검", Type = WeaponType.Sword, Grade = Grade.Epic, AttackPower = 120, RequiredLevel = 10, Price = 3000 },
        new Weapon { Name = "얼음 활", Type = WeaponType.Bow, Grade = Grade.Rare, AttackPower = 85, RequiredLevel = 8, Price = 2000 },
        new Weapon { Name = "드래곤 랜스", Type = WeaponType.Lance, Grade = Grade.Legendary, AttackPower = 150, RequiredLevel = 15, Price = 5000 },
        new Weapon { Name = "은총", Type = WeaponType.Gun, Grade = Grade.Common, AttackPower = 60, RequiredLevel = 5, Price = 1200 },
        new Weapon { Name = "황금 활", Type = WeaponType.Bow, Grade = Grade.Epic, AttackPower = 110, RequiredLevel = 12, Price = 3500 },
        new Weapon { Name = "폭풍의 검", Type = WeaponType.Sword, Grade = Grade.Unique, AttackPower = 100, RequiredLevel = 11, Price = 2800 }
    };
}