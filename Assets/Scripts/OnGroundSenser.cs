using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 檢測地面碰撞
/// </summary>
public class OnGroundSenser : MonoBehaviour
{
    public CapsuleCollider capcol;
    private Vector3 point1;
    private Vector3 point2;
    private float radius;
    public float offset = 0.3f;//讓檢測膠囊更小

    private void Awake()
    {
        radius = capcol.radius - 0.05f;//讓檢測膠囊更小

    }

    private void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius - offset); //脚底
        point2 = transform.position + transform.up * (capcol.height - offset) - transform.up * radius;//頭頂

        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));//膠囊檢測 地面
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }


}
