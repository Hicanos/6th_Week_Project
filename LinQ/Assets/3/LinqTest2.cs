using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LinqTest2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Show3();
    }

    public void Show1()
    {
        List<Weapon> weapons = WeaponDatabase.weapons.Where(w => w.Price > 3000).ToList();
        // ����� �� 3000�� �̻��� ����鸸 �����ͼ� ����Ʈ�� ��´�.

    }
    //��ũ���¹� 2��
    // �޼���
    // List
    // var ��� = �÷���

    public void Show2() 
    {
        List<string>weaponNames = WeaponDatabase.weapons.Select(w => w.Name).ToList();
        foreach(string name in weaponNames)
        {
            Debug.Log(name);
        }
    }

    public void Show3()
    {
        List<Weapon> weapons = WeaponDatabase.weapons.Where(w => w.Price > 1000).OrderBy(w=>w.Price).ToList();
        foreach (Weapon weapon in weapons)
        {
            Debug.Log(weapon.Name + " : " + weapon.Price);
        }
        //OrderBy�� ������� �����ϴ� �޼���
        //OrderByDescending�� ������������ �����ϴ� �޼���
    }

    public void Show4()
    {
        int weaponCont = WeaponDatabase.weapons.Count;
        int totalPrice = WeaponDatabase.weapons.Sum(w => w.Price);
        double averPrice = WeaponDatabase.weapons.Average(w => w.Price);

        Debug.Log($"������ �� ������ {weaponCont}��, ��ü ������ ������ {totalPrice}, �׸��� ��� ������ {averPrice.ToString("F1")}�Դϴ�.");
    }

    //GroupBy
    public void Show5()
    {
        IEnumerable<IGrouping<Grade,Weapon>> grouped =
            WeaponDatabase.weapons.GroupBy(w => w.Grade);
        // var grouped2 = WeaponDatabase.weapons.GroupBy(w => w.Grade);

        foreach(IGrouping<Grade, Weapon> group in grouped)
        {
            Debug.Log($"�׷� : {group.Key}");
            foreach (Weapon weapon in group)
            {
                Debug.Log($"���� �̸� : {weapon.Name}");
            }
        }
    }

    //take & skip
    public void Show6()
    {
        List<Weapon> top3Weapon = WeaponDatabase.weapons.Take(3).ToList();

        foreach(Weapon weapon in top3Weapon)
        {
            Debug.Log(weapon.Name);
        }
        // Take�� ó������ ��� �������� ���ϴ� �޼��� (������ �͵鸸 ��������)
        // Skip�� ��� �ǳʶٰ� �������� ���ϴ� �޼��� (�����ϰ� ��������)
    }

    public ItemUIManager uimanager;

    [ContextMenu("�׽�Ʈ ��ư �����")]
    // ContextMenu�� ����Ƽ���� ��Ŭ������ ������ �� �ִ� ��ư�� ������ִ� ��Ʈ����Ʈ
    public void TestButton()
    {
        uimanager.SetSortFunction(w => w.Where(w => w.Price > 1000).OrderBy(w => w.Price).ToList());
        uimanager.RefreshItemList();
    }
}
