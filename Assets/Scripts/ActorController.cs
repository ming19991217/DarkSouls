using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 負責動作演繹
/// </summary>
public class ActorController : MonoBehaviour
{
    public GameObject modle;
    public CameraController camcon;
    public IUserInput pi;
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;//跑步乘數
    public float jumpVelocity = 5.0f;//跳躍高度
    public float rollHeight = 3.0f;//滾動高度
    public float rollVelocity = 5.0f;//滾動冲量
    public float jabMultiplier = 10f;//後撤部乘數

    [Space(10)]
    [Header("===== Friction Settings =====")]
    public PhysicMaterial frictionOne;//摩擦1
    public PhysicMaterial frictionZero;//摩擦0

    public Animator anim;
    private Rigidbody rb;
    private CapsuleCollider col;
    private Vector3 movingVec; //移動量 模長*forward*是否跑步
    private Vector3 thrustVec;//衝量
    private bool canAttack;//判斷是否可以攻擊

    // private float lerpTarget;//攻擊動畫圖層緩動
    private bool lockPlanar = false; //移動量不更新
    private bool trackDirection = false;
    private Vector3 deltaPos;//動畫根運動的移動量


    public bool leftIsShield = true;//左手舉盾
    public delegate void OnActionDelegate();//行動的委托格式
    public event OnActionDelegate OnAction;//OnActionDelegate事件


    void Awake()
    {
        anim = modle.GetComponent<Animator>();
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }


    void Update()
    {

        if (pi.lockOn)
        {

            camcon.LockUnlock();
        }
        if (camcon.lockState == false)
        {
            float targetRunMulti = (pi.run ? 2.0f : 1.0f);//判斷跑步的乘數
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.1f));//設定移動動畫+ 跑步的過度
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDVecz = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDVecz.z * ((pi.run) ? 2.0f : 1.0f));//前方為forward
            anim.SetFloat("right", localDVecz.x * ((pi.run) ? 2.0f : 1.0f));//右方為x
        }

        if (pi.roll)
        {
            anim.SetTrigger("roll");
        }



        //按下攻擊 并且 可以攻擊狀態
        if ((pi.rb || pi.lb) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) && canAttack)
        {
            if (pi.rb)//右手攻擊
            {
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");//攻擊動畫
            }
            else if (pi.lb && !leftIsShield)//左手攻擊 左手不是盾
            {
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");//攻擊動畫
            }
        }
        if (((pi.rt) || (pi.lt)) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL") && canAttack))
        {
            if (pi.rt)
            {

            }
            else //左手
            {
                if (!leftIsShield)//左手沒盾
                {
                    //做重攻擊
                }
                else//有盾
                {//盾反
                    anim.SetTrigger("counterBack");
                }
            }

        }



        //舉盾判斷
        if ((CheckState("ground") || CheckState("blocked")) && leftIsShield)//在地面 和 左手是盾的情況下
        {
            if (pi.defense)//判斷有沒有防禦
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 1);//權重為1

            }
            else//
            {
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);

            }

        }
        else
        {
            anim.SetBool("defense", false);
            anim.SetLayerWeight(anim.GetLayerIndex("defense"), 0);

        }

        //人物朝向
        if (camcon.lockState == false)
        {
            if (pi.inputEnable == true)
            {
                //模型轉向
                if (pi.Dmag > 0.1f)//forward值不可以為0
                {
                    modle.transform.forward = Vector3.Slerp(modle.transform.forward, pi.Dvec, 0.3f);//轉身過渡
                }

            }

            if (lockPlanar == false) //控制移動量的更新. 當按下跳躍時 movingVec保持舊有值不更新.如果在走路按跳,玩家會以走路往前,並播放跳躍動畫.當動畫結束時重新更新,該變數變爲0
                movingVec = pi.Dmag * modle.transform.forward * (pi.run ? runMultiplier : 1.0f);//移動量=輸入模場*模型向前向量*是否跑步

        }

        else
        {
            if (trackDirection == false)
            {
                modle.transform.forward = transform.forward;//模型看向PlayerHandel的forward
            }
            else
            {
                modle.transform.forward = movingVec.normalized;
            }


            if (lockPlanar == false)
            {
                movingVec = pi.Dvec * (pi.run ? runMultiplier : 1.0f);
            }
        }

        if (pi.action)//當行動按下
        {
            OnAction.Invoke();//執行OnAction事件
        }
    }



    private void FixedUpdate()
    {
        rb.position += deltaPos;//加上動畫根運動的移動量
        deltaPos = Vector3.zero;//更新完清零

        //rb.position += movingVec * Time.fixedDeltaTime * walkSpeed;
        rb.velocity = new Vector3(movingVec.x * walkSpeed, rb.velocity.y, movingVec.z * walkSpeed) + thrustVec;//移動+向上冲量
        thrustVec = Vector3.zero;//冲量默認為0
    }


    public bool CheckState(string stateName, string layerName = "Base Layer")//判斷當前動畫狀態
    {
        int layerIndex = anim.GetLayerIndex(layerName);//獲得動畫圖層的索引
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);//判斷名字是否符合
        return result;
    }
    public bool CheckStateTag(string tagName, string layerName = "Base Layer")//判斷當前動畫狀態
    {
        int layerIndex = anim.GetLayerIndex(layerName);//獲得動畫圖層的索引
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);//判斷標簽名字是否符合
        return result;
    }



    ///
    /// 以下皆爲Message
    ///

    public void OnJumpEnter()//jump動畫開始播放時調用
    {
        pi.inputEnable = false;//跳躍時輸入鎖死
        lockPlanar = true;//玩家的移動量保持不變
        thrustVec = new Vector3(0, jumpVelocity, 0);//給向上冲量
        trackDirection = true;
    }

    //  因爲jump結束還有fall動畫 不該在jump解鎖輸入
    // public void OnJumpExit()//jump動畫結束播放時調用
    // {
    //     pi.inputEnable = true;//跳完輸入解鎖
    //     lockPlanar = false;//玩家的移動量更新
    // }

    public void IsGround()//檢測器碰撞到地面
    {
        anim.SetBool("isGround", true);
    }

    public void IsNotGround()//檢測器滯空
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;//跳完輸入解鎖
        lockPlanar = false;//玩家的移動量更新
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()//進入下落狀態
    {
        pi.inputEnable = false;//輸入鎖死
        lockPlanar = true;//玩家的移動量不更新
    }

    public void OnRollEnter()//進入翻滾狀態
    {
        // thrustVec = new Vector3(0, rollVelocity, 0);
        trackDirection = true;
        thrustVec = modle.transform.forward * rollVelocity;//滾動移動量
        thrustVec.y = rollHeight; //滾動高度
        pi.inputEnable = false;//輸入鎖死
        lockPlanar = true;//玩家的移動量不更新

    }

    public void OnJabEnter()
    {

        pi.inputEnable = false;//輸入鎖死
        lockPlanar = true;//玩家的移動量不更新
    }

    public void OnJabUpdate()
    {
        //jabVelocity 是jab動畫的曲綫 根據曲綫來增加後跳的值
        thrustVec = modle.transform.forward * anim.GetFloat("jabVelocity") * jabMultiplier;//向模型的forward值後方移動
    }


    public void OnAttack1hAEnter()//進入攻擊動畫
    {
        pi.inputEnable = false;

        //lerpTarget = 1.0f;//lerp目標數 在update進行lerp


    }
    public void OnAttackExit()
    {
        modle.SendMessage("WeaponDisable");//攻擊結束 關閉col
    }

    public void OnAttack1hAUpdate()
    {
        // float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));//取得當前權重值（因爲取得權重需要圖層的index值，所以用getindex）
        // currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);//使用lerp過度權重值

        // anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);//當進入攻擊動作 權重為1

        thrustVec = modle.transform.forward * anim.GetFloat("attack1hAVelocity") * 2f;//攻擊向前
    }

    public void OnAttackIdleUpdate()
    {
        // float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));//取得當前權重值（因爲取得權重需要圖層的index值，所以用getindex）
        // currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.1f);//使用lerp過度權重值

        // anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);//當進入攻擊動作 權重為0


    }
    public void OnHitEnter()
    {
        pi.inputEnable = false;
        movingVec = Vector3.zero;
        modle.SendMessage("WeaponDisable");//攻擊結束 關閉col

    }

    public void OnDieEnter()
    {
        pi.inputEnable = false;
        movingVec = Vector3.zero;
        modle.SendMessage("WeaponDisable");//攻擊結束 關閉col

    }
    public void OnBlockedEnter()
    {
        pi.inputEnable = false;
    }

    public void OnStunnedEnter()
    {
        pi.inputEnable = false;
        movingVec = Vector3.zero;
    }
    public void OnCounterBack()
    {
        pi.inputEnable = false;
        movingVec = Vector3.zero;
    }
    public void OnUpdateRM(object _deltaPos)//從模型傳來根運動的移動量
    {
        // //print((Vector3)_deltaPos);
        if (CheckState("attack1hC"))
        {
            deltaPos += (0.9f * deltaPos + 0.1f * (Vector3)_deltaPos) / 1.0f;

        }
    }

    public void OnLockEnter()
    {
        pi.inputEnable = false;
        movingVec = Vector3.zero;
        modle.SendMessage("WeaponDisable");
    }



    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
    public void SetBool(string boolName, bool value)
    {
        anim.SetBool(boolName, value);
    }
}
