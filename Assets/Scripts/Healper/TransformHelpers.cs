using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers
{
    //擴展方法 讓您在現有類型中「加入」方法，而不需要建立新的衍生類型
    //在transform的實例調用，第一個參數就是實例本身
    public static void hihi(this Transform trans, string say)
    {

        Debug.Log(trans.name + "says" + say);
    }



    public static Transform DeepFind(this Transform parent, string targetName)
    {
        Transform tempTrans = null;
        //foreach Transform 遍歷子物體（因爲TF是IEnumerable）
        foreach (Transform child in parent)
        {

            if (child.name == targetName)//找到
            {
                return child;
            }
            else
            {
                tempTrans = DeepFind(child, targetName);//向下查詢   
                if (tempTrans != null)
                {
                    return tempTrans;
                }
            }

        }
        return null;
    }

}
