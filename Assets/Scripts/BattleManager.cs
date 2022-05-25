using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 處理戰鬥檢測 設定受擊範圍
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    //public ActorManager am;
    private CapsuleCollider defCol;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();//受傷範圍的碰撞器
        defCol.center = new Vector3(0, 1.0f, 0);
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;
    }



    private void OnTriggerEnter(Collider col)
    {

        //對方的刀看進來時 接收對方的wc
        WeaponController targetWc = col.GetComponentInParent<WeaponController>();
        if (targetWc == null)
            return;

        GameObject attacker = targetWc.wm.am.ac.modle;//攻擊者
        GameObject receiver = am.ac.modle;//受擊者


        if (col.tag == "Weapon")
        {
            //向am回傳 對方wc，判斷攻擊範圍是否合理，判斷玩家反擊範圍是否合理
            am.TryDoDamage(targetWc, CheckAngleTarget(receiver, attacker, 70), CheckAnglePlayer(receiver, attacker, 30));

        }
    }


    //判斷兩個是否面對面(判斷玩家反擊範圍是否OK)
    public static bool CheckAnglePlayer(GameObject player, GameObject target, float playerAngle)
    {
        //從玩家到目標的向量
        Vector3 counterDir = target.transform.position - player.transform.position;
        //玩家是否面相目標
        float counterAngle1 = Vector3.Angle(player.transform.forward, counterDir);
        //玩家與目標是否兩者面對面
        float counterAngle2 = Vector3.Angle(target.transform.forward, player.transform.forward);//should be closed to 180
        //玩家面相目標角度 < 指定角度 && 兩者面對面的角度 -180 絕對值 < 指定角度
        bool counterValid = (counterAngle1 < playerAngle && Mathf.Abs(counterAngle2 - 180) < playerAngle);
        return counterValid;
    }


    //判斷玩家是否在物件的前方(判斷攻擊範圍是否OK)
    public static bool CheckAngleTarget(GameObject player, GameObject target, float targetAngleLimit)
    {
        Vector3 attackingDir = player.transform.position - target.transform.position;

        float attackingAngle1 = Vector3.Angle(target.transform.forward, attackingDir);

        bool attackValid = (attackingAngle1 < targetAngleLimit);
        return attackValid;

    }
}
