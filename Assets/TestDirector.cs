using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TestDirector : MonoBehaviour
{

    public PlayableDirector pd;

    public Animator attacker; //綁定的動畫控制器
    public Animator victim;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            //=====動態更換演員============================
            //pd.playableAsset.outputs 為timeline所有軌道
            foreach (var track in pd.playableAsset.outputs)
            {
                if (track.streamName == "Attacker Animation")//判斷軌道名稱
                {
                    pd.SetGenericBinding(track.sourceObject, attacker);//設定綁定的animator
                }
                else if (track.streamName == "Victim Animation")//判斷軌道名稱
                {
                    pd.SetGenericBinding(track.sourceObject, victim);//設定綁定的animator
                }
            }
            //===========================================

            //重置timeline
            pd.time = 0;//將現在時間條為0
            pd.Stop();//停止現在的播放
            pd.Evaluate();//進行obj的計算

            pd.Play();
        }
    }
}
