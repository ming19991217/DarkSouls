using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 修正啓用 根運動 造成的移動偏移
/// </summary>
public class RootMotionControl : MonoBehaviour
{
    //當我們啓用《套用根運動》 他會使用美術設定好的移動量，去改變模型的移動。
    //但會與上層的PlayerHandle分開，造成模型脫離膠囊碰撞體

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()//當animator計算完整個動畫框架後調用此方法，每幀調用。用此來修正動畫的移動量
    {
        //當anim算完整個動畫，傳回一個動畫的移動量（還沒使用這個值）
        //print(anim.deltaPosition);

        SendMessageUpwards("OnUpdateRM", (object)anim.deltaPosition);//向PlayerHandle傳送動畫的移動量
    }
}
