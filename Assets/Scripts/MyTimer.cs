
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 計時器
/// </summary>
public class MyTimer
{
    public enum STATE
    {
        IDLE, RUN, FINISH
    }
    public STATE state = STATE.IDLE;
    public float duration = 1.0f;//計時時間
    public float elapsedTime = 0;//過去時間

    public void Tick()
    {
        if (state == STATE.IDLE)
        {

        }
        else if (state == STATE.RUN)
        {
            elapsedTime += Time.deltaTime;//計時纍加
            if (elapsedTime >= duration)//時間到了
            {
                state = STATE.FINISH;//狀態改爲finish
            }
        }
        else if (state == STATE.FINISH)
        {

        }
        else
        {
            Debug.Log("MyTimer error");
        }
    }

    public void Go()//開始計時
    {
        elapsedTime = 0;
        state = STATE.RUN;

    }

}
