using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 遙感輸入
/// </summary>
public class JoystickInput : IUserInput
{
    [Header("===== Joystick Settings")]
    public string axisX = "axisX";
    public string axisY = "axisY";
    public string axisJup = "axis6";
    public string axisJright = "axis3";
    public string btnA = "btn0";
    public string btnB = "btn1";
    public string btnC = "btn2";
    public string btnD = "btn3";
    public string btnLB = "btn4";
    public string btnLT = "btn6";
    public string btnRB = "btn5";
    public string btnRT = "btn7";


    public string btnJstick = "btn11";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRB = new MyButton();
    public MyButton buttonRT = new MyButton();


    public MyButton buttonJstick = new MyButton();

    // [Header("===== Output Signals =====")]

    // public float Dup;  //輸入上下的值 上=1 下=-1
    // public float Dright; //輸入左右的值 右=1 左=-1
    // public float Dmag; //輸入的模長
    // public Vector3 Dvec; //輸入的向量

    // public float Jup; //鏡頭輸入上下的值 上=1 下=-1
    // public float Jright; //鏡頭輸入左右的值 右=1 左=-1




    // public bool run;//跑步信號

    // public bool jump;//跳躍
    // public bool lastJump;//

    // public bool attack;//攻擊
    // public bool lastAttack;//

    // [Header("===== Others =====")]

    // public bool inputEnable = true;//輸入開關 將targetDup、right =0

    // private float targetDup;//目標的上下輸入值 1 or -1
    // private float targetDright;//目標的水平輸入值 1 or -1 
    // private float velocityDup; //SmoothDamp用
    // private float velocityDright; //SmoothDamp用


    private void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonC.Tick(Input.GetButton(btnC));
        buttonD.Tick(Input.GetButton(btnD));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonLT.Tick(Input.GetButton(btnLT));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonRT.Tick(Input.GetButton(btnRT));
        buttonJstick.Tick(Input.GetButton(btnJstick));



        Jup = -1 * Input.GetAxis(axisJup);//鏡頭上下輸入信號
        Jright = Input.GetAxis(axisJright);//鏡頭左右輸入信號

        //目標的輸入值 輸入上下信號 因爲是軸所以不用加加減減
        targetDup = Input.GetAxis(axisY);
        //目標的輸入值 輸入水平信號 
        targetDright = Input.GetAxis(axisX);

        if (!inputEnable) //如果輸入開關關閉
        {   //將輸入目標設爲0
            targetDup = 0;
            targetDright = 0;
        }

        //增加輸入的過渡
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);


        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));  //x平方+y平方 開根號  計算輸入長度
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;//模型的轉向向量


        //儅玩家按下按鈕 并且 isdelaying 的計時已經過了 或者 他在延長狀態（爲了讓跑接跳流暢）
        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        action = buttonC.OnPressed;
        defense = buttonB.IsPressing;

        // bool newJump = Input.GetButton(btnB);//跳躍信號
        // if (newJump != lastJump && newJump == true)//newjump為true 起跳
        // {
        //     jump = true;
        // }
        // else
        // {
        //     jump = false;
        // }
        // lastJump = newJump;
        jump = buttonA.OnPressed && buttonA.IsExtending;//由mybutton來統一處理輸入信號的時機



        // bool newAttack = Input.GetButton(btnC);//跳躍信號
        // if (newAttack != lastAttack && newAttack == true)//
        // {
        //     attack = true;
        // }
        // else
        // {
        //     attack = false;
        // }
        // lastAttack = newAttack;
        //attack = buttonC.OnPressed;
        rb = buttonRB.OnPressed;
        rt = buttonRT.OnPressed;
        lb = buttonLB.OnPressed;
        lt = buttonLT.OnPressed;
        lockOn = buttonJstick.OnPressed;//敵人鎖定

    }





}
