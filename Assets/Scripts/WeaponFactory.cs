using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器工廠 需要從gm獲得武器資料庫
/// </summary>
public class WeaponFactory
{
    private DataBase weaponDB;

    //需要一個武器資料庫
    public WeaponFactory(DataBase _weaponDB)
    {
        weaponDB = _weaponDB;
    }


    //創建武器 從Resourse中獲得武器預製件
    public GameObject CreateWeapon(string weaponName, Vector3 pos, Quaternion rot)
    {
        GameObject prefab = Resources.Load(weaponName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, pos, rot);

        //對生成物件添加武器數值 並輸入數值
        WeaponData wdate = obj.AddComponent<WeaponData>();
        wdate.ATK = weaponDB.weaponDataBase[weaponName]["ATK"].f;

        return obj;
    }
    public Collider CreateWeapon(string weaponName, string side, WeaponManager wm)
    {
        WeaponController wc;
        if (side == "L")
        {
            wc = wm.wcL;
        }
        else if (side == "R")
        {
            wc = wm.wcR;
        }
        else
        {
            return null;
        }

        GameObject prefab = Resources.Load(weaponName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.parent = wc.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        WeaponData wdate = obj.AddComponent<WeaponData>();
        wdate.ATK = weaponDB.weaponDataBase[weaponName]["ATK"].f;
        wc.wdata = wdate;

        return obj.GetComponent<Collider>();
    }



}
