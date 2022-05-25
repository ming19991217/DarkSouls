using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 清除動畫Triger
/// </summary>
public class TriggerControl : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    public void ResetTrigger(string triggerName)//動畫事件 在某一幀將積累的trigger清除
    {
        anim.ResetTrigger(triggerName);
    }
}
