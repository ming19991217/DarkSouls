using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 互相作用管理 檢測特殊作用  并且加入事件方法 
/// 在此檢測 如果有可行動的事件 加入overlapList列表 如果玩家按下行動 執行event的方法
/// event調用overlap的引用 只需訂閲一次即可
/// </summary>
public class InteractionManager : IActorManagerInterface
{
    private CapsuleCollider interCol;

    //事件列表
    public List<EventCasterManager> overlapEcastms = new List<EventCasterManager>();

    private void Start()
    {
        GetComponent<CapsuleCollider>();
    }



    //玩家與特殊caster接觸，取得她身上的eventCasterManager
    private void OnTriggerEnter(Collider other)
    {
        EventCasterManager[] ecastms = other.GetComponents<EventCasterManager>();
        foreach (var ecastm in ecastms)
        {
            if (!overlapEcastms.Contains(ecastm))//列表沒有包含ecastm
            {
                overlapEcastms.Add(ecastm);//加入列表
            }
        }
    }

    //玩家離開特殊caster，移除事件
    private void OnTriggerExit(Collider other)
    {
        EventCasterManager[] ecastms = other.GetComponents<EventCasterManager>();
        foreach (var ecastm in ecastms)
        {
            if (overlapEcastms.Contains(ecastm))//列表有包含ecastm
            {
                overlapEcastms.Remove(ecastm);//移除列表
            }
        }
    }
}
