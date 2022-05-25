using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 負責管理武器開關 接收動畫事件
/// </summary>
public class WeaponManager : IActorManagerInterface
{
    //public ActorManager am;
    private Collider weaponColL;//武器碰撞器
    private Collider weaponColR;
    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;


    private void Awake()
    {
        //如果在左手是空指針 停止
        try
        {
            whL = transform.DeepFind("weaponHandleL").gameObject;
            wcL = BindWeaponController(whL);
            weaponColL = whL.GetComponentInChildren<Collider>();
        }
        catch (System.Exception)
        {
            //if there is no "weaponHandleL" or related object

        }

        try
        {
            whR = transform.DeepFind("weaponHandleR").gameObject;
            wcR = BindWeaponController(whR);
            weaponColR = whR.GetComponentInChildren<Collider>();
        }
        catch (System.Exception)
        {  //if there is no "weaponHandleR" or related object //i
        }






    }

    public void UpdateWeaponCollider(string side, Collider col)
    {
        if (side == "L")
        {
            weaponColL = col;
        }
        else if (side == "R")
        {
            weaponColR = col;
        }

    }

    //移除手上的武器
    public void UnloadWeapon(string side)
    {
        if (side == "L")
        {
            foreach (Transform tran in whL.transform)
            {
                weaponColL = null;
                wcL.wdata = null;
                Destroy(tran.gameObject);
            }

        }
        else if (side == "R")
        {
            foreach (Transform tran in whR.transform)
            {
                weaponColR = null;
                wcR.wdata = null;
                Destroy(tran.gameObject);
            }
        }
    }

    //綁定武器控制脚本，沒有則添加
    public WeaponController BindWeaponController(GameObject targetObj)
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if (tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }

    public void WeaponEnable()
    {
        //檢查是左手攻擊還是右手攻擊
        if (am.ac.CheckStateTag("attackL"))
        {
            weaponColL.enabled = true;//左手開啓碰撞
        }
        else
        {
            weaponColR.enabled = true;//右手開啓碰撞
        }


    }
    public void WeaponDisable()
    {

        weaponColL.enabled = false;//左手關碰撞
        weaponColR.enabled = false;//左手關碰撞


    }
    public void CounterBackEnable()
    {
        am.SetIsCounterBack(true);
    }
    public void CounterBackDisable()
    {
        am.SetIsCounterBack(false);
    }

    //切換雙手武器
    public void ChangeDualHands(bool dualOn)
    {
        am.ChangeDualHands(dualOn);
    }
}
