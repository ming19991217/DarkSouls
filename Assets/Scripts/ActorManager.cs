using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 經由bm接收訊息，由am進行判斷，並命令ac進行動作
/// </summary>
public class ActorManager : MonoBehaviour
{
    public ActorController ac;

    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;

    [Header("=== Override Animator ===")]
    public AnimatorOverrideController oneHandAnim;
    public AnimatorOverrideController twoHandAnim;


    private void Awake()
    {
        ac = GetComponent<ActorController>();
        GameObject modle = ac.modle;
        GameObject sensor = null;
        try
        {
            sensor = transform.Find("sensor").gameObject;
        }
        catch (System.Exception)
        {
            //if there is no "sensor" 
        }



        //脚本互相綁定 
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(modle);
        sm = Bind<StateManager>(gameObject);
        dm = Bind<DirectorManager>(gameObject);
        im = Bind<InteractionManager>(sensor);

        ac.OnAction += DoAction;//訂閲事件

    }

    public void DoAction()//調用im中的overlapEcastms引用 
    {
        if (im.overlapEcastms.Count != 0)
        {
            if (im.overlapEcastms[0].active == true && !dm.IsPlaying())
            {
                if (im.overlapEcastms[0].eventName == "frontStab")//如果是正面次事件
                {
                    if (BattleManager.CheckAnglePlayer(ac.modle, im.overlapEcastms[0].am.gameObject, 45))
                    {
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].transform.TransformVector(im.overlapEcastms[0].offset);
                        ac.modle.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);
                        //導演播放正面次劇本 並傳入 劇本、攻擊者、受擊者
                        dm.PlayFrontStab("frontStab", this, im.overlapEcastms[0].am);
                    }

                }
                else if (im.overlapEcastms[0].eventName == "openBox")//如果是開箱事件
                {
                    //確認玩家與箱子面對面
                    if (BattleManager.CheckAnglePlayer(ac.modle, im.overlapEcastms[0].am.gameObject, 35))
                    {
                        //玩家位置進行偏移
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].transform.TransformVector(im.overlapEcastms[0].offset);
                        //玩家看向箱子
                        ac.modle.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);

                        //導演播放劇本 並傳入 劇本、攻擊者、受擊者
                        im.overlapEcastms[0].active = false;//箱子開過一次後 就無法再次開啓
                        dm.PlayFrontStab("openBox", this, im.overlapEcastms[0].am);

                    }
                }

                else if (im.overlapEcastms[0].eventName == "leverUp")//如果是開箱事件
                {
                    //確認玩家與箱子面對面
                    if (BattleManager.CheckAnglePlayer(ac.modle, im.overlapEcastms[0].am.gameObject, 35))
                    {
                        //玩家位置進行偏移
                        transform.position = im.overlapEcastms[0].am.transform.position + im.overlapEcastms[0].transform.TransformVector(im.overlapEcastms[0].offset);
                        //玩家看向箱子
                        ac.modle.transform.LookAt(im.overlapEcastms[0].am.transform, Vector3.up);

                        //導演播放劇本 並傳入 劇本、攻擊者、受擊者
                        // im.overlapEcastms[0].active = false;//箱子開過一次後 就無法再次開啓
                        dm.PlayFrontStab("leverUp", this, im.overlapEcastms[0].am);

                    }
                }



            }

        }
    }


    //綁定 傳進T類型 返還T類型 go脚本挂載的地方    這個類型一定是 IActorManagerInterface 或是他的子類
    private T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        T tempInstance;
        if (go == null)
        {
            return null;
        }
        tempInstance = go.GetComponent<T>();
        if (tempInstance == null)
        {
            tempInstance = go.AddComponent<T>();
        }
        tempInstance.am = this;
        return tempInstance;
    }

    public void TryDoDamage(WeaponController targetWc, bool attackValid, bool counterValid)//受到傷害 BM接收傷害 AM進行評估 去看SM的狀態
    {
        if (sm.isCounterBackSuccess)//盾反成功狀態
        {
            if (counterValid)
            {
                targetWc.wm.am.Stunned();//呼叫對方的AM做被盾反動作                
            }

        }
        else if (sm.isCounterBackFailure)//盾反失敗
        {
            if (attackValid)//確定攻擊成立 被打
            {
                HitOrDie(targetWc, false);
            }
        }

        else if (sm.isImmortal)//無敵狀態
        {
            //do nothing
        }
        else if (sm.isDefense)//如果是防禦狀態
        {
            Blocked();
        }
        else//沒防禦
        {
            if (attackValid)
            {
                HitOrDie(targetWc, true);
            }
        }
    }

    public void HitOrDie(WeaponController targetWc, bool doHitAnimation)
    {

        if (sm.HP <= 0)//已經死了
        {

        }
        else//扣血
        {
            sm.AddHP(-1 * targetWc.GetATK());
            if (sm.HP > 0)
            {
                if (doHitAnimation)
                {
                    Hit();
                }
                //do VFX 
            }
            else
            {
                Die();
            }

        }
    }
    public void Stunned()//被反擊動畫
    {
        ac.IssueTrigger("stunned");
    }
    public void Blocked()//舉盾動畫
    {
        ac.IssueTrigger("blocked");
    }
    public void Hit()//受傷動畫
    {
        ac.IssueTrigger("hit");
    }
    public void Die()//死亡動畫
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnable = false;
        if (ac.camcon.lockState == true)
        {
            ac.camcon.LockUnlock();
        }
        ac.camcon.enabled = false;
    }


    public void SetIsCounterBack(bool value)//在動畫事件改變counterBack的值
    {
        sm.isCounterBackEnable = value;
    }

    public void LockUnlockActorController(bool value)//進入directional 鎖定動畫機
    {
        ac.SetBool("lock", value);
    }


    //切換雙手武器
    public void ChangeDualHands(bool dualOn)
    {
        if (dualOn)
        {
            ac.anim.runtimeAnimatorController = twoHandAnim;
        }
        else
        {
            ac.anim.runtimeAnimatorController = oneHandAnim;
        }
    }
}
