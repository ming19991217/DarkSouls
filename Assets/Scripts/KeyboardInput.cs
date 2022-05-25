using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    [Header("===== Key Settings =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";


    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    public string keyE;
    public string keyF;
    public string keyG;
    public string keyH;
    public string keyI;

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonE = new MyButton();
    public MyButton buttonF = new MyButton();
    public MyButton buttonG = new MyButton();
    public MyButton buttonH = new MyButton();
    public MyButton buttonI = new MyButton();

    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    [Header("===== Mouse Settings =====")]
    public bool mouseEnable = false;//滑鼠控制
    public float mouseSensitivitY = 1.0f;//靈敏度
    public float mouseSensitivitX = 1.0f;


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




    void Update()
    {
        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonC.Tick(Input.GetKey(keyC));
        buttonD.Tick(Input.GetKey(keyD));
        buttonE.Tick(Input.GetKey(keyE));
        buttonF.Tick(Input.GetKey(keyF));
        buttonG.Tick(Input.GetKey(keyG));
        buttonH.Tick(Input.GetKey(keyH));
        buttonI.Tick(Input.GetKey(keyI));

        // print(buttonA.IsExtending && buttonA.OnPressed);//判斷按兩下



        if (mouseEnable == true)//如果啓用鼠標 使用滑鼠移動鏡頭
        {
            Jup = Input.GetAxis("Mouse Y") * mouseSensitivitY;
            Jright = Input.GetAxis("Mouse X") * mouseSensitivitX;
        }
        else//使用鍵盤移動鏡頭
        {
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0 - ((Input.GetKey(keyJDown)) ? 1f : 0));//計算鏡頭上下輸入信號
            Jright = (Input.GetKey(keyJRight) ? 1.0f : 0 - ((Input.GetKey(keyJLeft)) ? 1f : 0));//計算鏡頭左右輸入信號
        }


        //目標的輸入值 輸入上下信號 上=1 下=-1
        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0f);
        //目標的輸入值 輸入水平信號 右=1 左=-1
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

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


        //跑A 跳AA 滾B C 防禦D  鎖定E 右攻擊F 左攻擊H

        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;//跑步信號輸入

        defense = buttonD.IsPressing;//防禦信號輸入

        jump = buttonA.OnReleased && buttonA.IsDelaying;//雙擊跳躍

        // attack = buttonC.OnPressed;
        action = buttonG.OnPressed;

        roll = buttonB.OnPressed;//後撤步 在按鈕按下計時時 就已經鬆手了

        lockOn = buttonE.OnPressed;//敵人鎖定

        rb = buttonF.OnPressed;//輕攻擊
        lb = buttonH.OnPressed;//輕攻擊
        rt = buttonG.OnPressed;//重攻擊
        lt = buttonI.OnPressed;//重攻擊


        // bool newJump = Input.GetKey(keyB);//跳躍信號
        // if (newJump != lastJump && newJump == true)//newjump為true 起跳
        // {
        //     jump = true;
        // }
        // else
        // {
        //     jump = false;
        // }
        // lastJump = newJump;



        // bool newAttack = Input.GetKey(keyC);//攻擊信號
        // if (newAttack != lastAttack && newAttack == true)//
        // {
        //     attack = true;
        // }
        // else
        // {
        //     attack = false;
        // }
        // lastAttack = newAttack;



    }


}
