using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 判斷按鈕按下的時機 長按 按兩下 
/// </summary>
public class MyButton
{
    public bool IsPressing = false;//是否按壓 儅玩家按下按鈕為true 鬆開為false
    public bool OnPressed = false;//只會在按下時反應
    public bool OnReleased = false;//只會在鬆開時反應
    public bool IsExtending = false;//在鬆開後開始計時的時段
    public bool IsDelaying = false;//按下按鈕1秒後，


    public float extendingDuration = 0.15f;//延長時間
    public float delayingDuration = 0.15f;//


    private bool curState = false;
    private bool lastState = false;

    private MyTimer extTimer = new MyTimer();//雙擊計時器
    private MyTimer delayTimer = new MyTimer();//長按計時器

    public void Tick(bool input)
    {

        extTimer.Tick();//每幀調用雙擊計時器
        delayTimer.Tick();//調用長按⏲

        curState = input;
        IsPressing = curState;

        //先將變量設爲false，後續在變動
        OnReleased = false;
        OnPressed = false;
        IsExtending = false;
        IsDelaying = false;

        if (curState != lastState)//如果當前狀態不等於上次狀態
            if (curState == true)//判斷是按下 而不是鬆開
            {
                OnPressed = true;//按下
                StartTimer(delayTimer, delayingDuration);//開始長按計時

            }
            else
            {
                OnReleased = true;//鬆開
                StartTimer(extTimer, extendingDuration);//鬆開后開始計時
            }

        lastState = curState;

        //檢測雙擊按鈕是不是開始延長計時 只有在開始計時才爲true
        if (extTimer.state == MyTimer.STATE.RUN)
            IsExtending = true;

        //檢測長按按鈕是不是開始計時
        if (delayTimer.state == MyTimer.STATE.RUN)
            IsDelaying = true;

    }

    private void StartTimer(MyTimer timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }

}
