using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 資料庫 負責讀取Resourse文件 並轉換為jsonOBJ
/// </summary>
public class DataBase
{
    private string weaponDatabaseFileName = "weaponData";
    //readonly 只讀
    public readonly JSONObject weaponDataBase;


    //創建時讀取文件並轉換爲jsonobj
    public DataBase()
    {
        //從Resourse讀取文件
        TextAsset weaponContent = Resources.Load(weaponDatabaseFileName) as TextAsset;
        //實例jsonObj
        weaponDataBase = new JSONObject(weaponContent.text);
    }

}
