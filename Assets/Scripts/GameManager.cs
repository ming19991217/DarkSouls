using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WeaponManager testWM;

    private static GameManager instance;
    private DataBase weaponDB;//存有武器信息數值
    private WeaponFactory weaponFact;//武器工廠 負責生成武器


    private void Awake()
    {
        CheckSingle();
        CheckGameObject();

    }

    void Start()
    {
        InitWeaponDB();
        InitWeaponFactory();



        testWM.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Falchion", "R", testWM));
        testWM.ChangeDualHands(false);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Sword"))
        {
            testWM.UnloadWeapon("R");
            testWM.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Sword", "R", testWM));
            testWM.ChangeDualHands(false);

        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "Falchion"))
        {
            testWM.UnloadWeapon("R");
            testWM.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Falchion", "R", testWM));
            testWM.ChangeDualHands(true);

        }
        if (GUI.Button(new Rect(10, 90, 150, 30), "Mace"))
        {
            testWM.UnloadWeapon("R");
            testWM.UpdateWeaponCollider("R", weaponFact.CreateWeapon("Mace", "R", testWM));
            testWM.ChangeDualHands(false);

        }
        if (GUI.Button(new Rect(10, 130, 150, 30), "Unload"))
        {
            testWM.UnloadWeapon("R");
            testWM.ChangeDualHands(false);

        }


    }



    //初始化資料庫類 讀取武器數值
    private void InitWeaponDB()
    {
        weaponDB = new DataBase();
    }


    //初始化武器工廠
    private void InitWeaponFactory()
    {
        weaponFact = new WeaponFactory(weaponDB);
    }




    private void CheckSingle()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(this);
    }

    private void CheckGameObject()
    {
        //是不是挂在正確的物件
        if (tag == "GM")
        {
            return;
        }
        Destroy(this);
    }

    // void Start()
    // {
    //     //(從硬盤到内存)
    //     GameObject prefab = Resources.Load("Weapon_1") as GameObject;
    //     Instantiate(prefab, Vector3.zero, Quaternion.identity);

    //     //Resources.UnloadUnusedAssets();  不是必要的
    // }

}
