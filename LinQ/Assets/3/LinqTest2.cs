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
        // 무기들 중 3000원 이상인 무기들만 가져와서 리스트에 담는다.

    }
    //링크쓰는법 2개
    // 메서드
    // List
    // var 결과 = 컬렉션

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
        //OrderBy는 순서대로 정렬하는 메서드
        //OrderByDescending은 내림차순으로 정렬하는 메서드
    }

    public void Show4()
    {
        int weaponCont = WeaponDatabase.weapons.Count;
        int totalPrice = WeaponDatabase.weapons.Sum(w => w.Price);
        double averPrice = WeaponDatabase.weapons.Average(w => w.Price);

        Debug.Log($"무기의 총 개수는 {weaponCont}개, 전체 아이템 가격은 {totalPrice}, 그리고 평균 가격은 {averPrice.ToString("F1")}입니다.");
    }

    //GroupBy
    public void Show5()
    {
        IEnumerable<IGrouping<Grade,Weapon>> grouped =
            WeaponDatabase.weapons.GroupBy(w => w.Grade);
        // var grouped2 = WeaponDatabase.weapons.GroupBy(w => w.Grade);

        foreach(IGrouping<Grade, Weapon> group in grouped)
        {
            Debug.Log($"그룹 : {group.Key}");
            foreach (Weapon weapon in group)
            {
                Debug.Log($"무기 이름 : {weapon.Name}");
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
        // Take는 처음부터 몇개를 가져올지 정하는 메서드 (설정한 것들만 가져오기)
        // Skip은 몇개를 건너뛰고 가져올지 정하는 메서드 (제외하고 가져오기)
    }

    public ItemUIManager uimanager;

    [ContextMenu("테스트 버튼 실행기")]
    // ContextMenu는 유니티에서 우클릭으로 실행할 수 있는 버튼을 만들어주는 어트리뷰트
    public void TestButton()
    {
        uimanager.SetSortFunction(w => w.Where(w => w.Price > 1000).OrderBy(w => w.Price).ToList());
        uimanager.RefreshItemList();
    }
}
