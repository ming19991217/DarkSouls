using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 利用OnAnimatorIK調整骨頭旋轉量
/// </summary>
public class LeftArmAnimFix : MonoBehaviour
{

    private Animator anim;
    private ActorController ac;
    public Vector3 a;
    private void Awake()
    {
        ac = GetComponentInParent<ActorController>();
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)//在animator中改變骨骼IK
    {
        if (ac.leftIsShield)
        {
            if (anim.GetBool("defense") == false)
            {
                Transform leftLowArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);//獲取骨頭
                leftLowArm.localEulerAngles += a;//加上旋轉量
                anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowArm.localEulerAngles));//重新賦值骨頭旋轉量

            }
        }
    }
}
