using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 數值 狀態
/// </summary>
public class StateManager : IActorManagerInterface
{
    // public ActorManager am;
    public float HP = 15f;
    public float HPMax = 15f;
    public float ATK = 10.0f;

    [Header("1st order state flags")]//第一階層的狀態旗標
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounterBack;//盾反動畫
    public bool isCounterBackEnable;//盾反時機



    [Header("2nd order state flag")]
    public bool isAllowDefense;
    public bool isImmortal;//無敵狀態
    public bool isCounterBackSuccess;//盾反成功
    public bool isCounterBackFailure;//盾反失敗

    private void Start()
    {
        HP = HPMax;
    }

    private void Update()
    {
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isJab = am.ac.CheckState("jab");
        isAttack = am.ac.CheckStateTag("attackR") || am.ac.CheckStateTag("attackR");
        isHit = am.ac.CheckState("hit");
        isDie = am.ac.CheckState("die");
        isBlocked = am.ac.CheckState("blocked");
        isCounterBack = am.ac.CheckState("counterBack");


        isAllowDefense = isGround || isBlocked;//只有在ground跟被打才能防禦
        isDefense = isAllowDefense && am.ac.CheckState("defense1h", "defense");
        isImmortal = isRoll || isJab;
        isCounterBackSuccess = isCounterBackEnable;//盾反成功的時間段，am利用這個來判斷是否成功
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;//盾反失敗的時間段
    }

    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);

    }


}
