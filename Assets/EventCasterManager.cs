using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 挂在在caster上 用於描述事件
/// </summary>
public class EventCasterManager : IActorManagerInterface
{
    public string eventName;//事件名稱
    public bool active;
    public Vector3 offset = new Vector3(0, 0, 0.5f);
    private void Start()
    {
        if (am == null)
        {
            am = GetComponentInParent<ActorManager>();
        }
    }

}
