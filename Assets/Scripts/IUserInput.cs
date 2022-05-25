using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 是鍵盤和搖桿的父類 ，abstract使這個類無法被實體化
/// </summary>
public abstract class IUserInput : MonoBehaviour
{

    [Header("===== Output Signals =====")]

    public float Dup;  //輸入上下的值 上=1 下=-1
    public float Dright; //輸入左右的值 右=1 左=-1
    public float Dmag; //輸入的模長
    public Vector3 Dvec; //輸入的向量

    public float Jup; //鏡頭輸入上下的值 上=1 下=-1
    public float Jright; //鏡頭輸入左右的值 右=1 左=-1




    public bool run;//跑步信號
    public bool defense;//防禦信號
    public bool action;//行動

    public bool jump;//跳躍
    public bool lastJump;//

    //public bool attack;//攻擊
    public bool lastAttack;//
    public bool roll;//後撤步
    public bool lockOn;//鎖定敵人
    public bool lb;
    public bool lt;
    public bool rb;
    public bool rt;

    [Header("===== Others =====")]

    public bool inputEnable = true;//輸入開關 將targetDup、right =0

    protected float targetDup;//目標的上下輸入值 1 or -1
    protected float targetDright;//目標的水平輸入值 1 or -1 
    protected float velocityDup; //SmoothDamp用
    protected float velocityDright; //SmoothDamp用


    protected Vector2 SquareToCircle(Vector2 input)//消除斜走問題
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

    protected void UpdateDmagDvec(float Dup2, float Dright2)
    {
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));  //x平方+y平方 開根號  計算輸入長度
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;//模型的轉向向量

    }



}
