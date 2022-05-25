using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    public PlayableDirector pd;

    [Header("===== Timeline assets =====")]
    public TimelineAsset frontStab;//正面次劇本
    public TimelineAsset openBox;//開寶箱
    public TimelineAsset leverUp;//拉拉桿
    [Header("===== Assets Settings =====")]
    public ActorManager attacker;
    public ActorManager victim;



    void Start()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;//初始播放false



    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            pd.Play();
        }
    }

    public bool IsPlaying()
    {
        if (pd.state == PlayState.Playing)//檢查播放狀態
        {
            return true;
        }
        return false;
    }

    //播放正面刺劇本
    public void PlayFrontStab(string timelineName, ActorManager attacker, ActorManager victim)
    {
        // if (pd.playableAsset != null)//手上有劇本 正在播放
        //     return;


        if (timelineName == "frontStab")//如果劇本是正面此
        {
            pd.playableAsset = Instantiate(frontStab);//賦值劇本

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;



            foreach (var track in timeline.GetOutputTracks())//遍歷所有軌道
            {
                if (track.name == "Attacker Script")
                {
                    pd.SetGenericBinding(track, attacker);//綁定
                    foreach (var clip in track.GetClips())//遍歷軌道中所有的clip
                    {
                        MySuperClip mySuperClip = (MySuperClip)clip.asset;//取得clip實例
                        MySuperBehaviour mySuperBehaviour = mySuperClip.template;//取得behaviour
                        mySuperBehaviour.myFloat = 777;//改動behaviour裡的變量
                        mySuperClip.am.exposedName = System.Guid.NewGuid().ToString();//更改初始值
                        //Reference =字典 設定clip的ExposedReference 必須使用這個方法 直接賦值無法成功 將am傳進clip
                        pd.SetReferenceValue(mySuperClip.am.exposedName, attacker);
                    }
                }
                else if (track.name == "Victim Script")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())//遍歷軌道中所有的clip
                    {
                        MySuperClip mySuperClip = (MySuperClip)clip.asset;//取得clip實例
                        MySuperBehaviour mySuperBehaviour = mySuperClip.template;//取得behaviour
                        mySuperBehaviour.myFloat = 666;//改動behaviour裡的變量
                        mySuperClip.am.exposedName = System.Guid.NewGuid().ToString();//更改初始值


                        pd.SetReferenceValue(mySuperClip.am.exposedName, victim);
                    }
                }
                else if (track.name == "Attacker Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Victim Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }

            pd.Evaluate();
            pd.Play();
        }

        else if (timelineName == "openBox")
        {
            pd.playableAsset = Instantiate(openBox);
            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())//遍歷所有軌道
            {
                if (track.name == "Player Script")
                {
                    pd.SetGenericBinding(track, attacker);//綁定
                    foreach (var clip in track.GetClips())//遍歷軌道中所有的clip
                    {
                        MySuperClip mySuperClip = (MySuperClip)clip.asset;//取得clip實例
                        MySuperBehaviour mySuperBehaviour = mySuperClip.template;//取得behaviour
                        mySuperClip.am.exposedName = System.Guid.NewGuid().ToString();//更改初始值
                        //Reference =字典 設定clip的ExposedReference 必須使用這個方法 直接賦值無法成功 將am傳進clip
                        pd.SetReferenceValue(mySuperClip.am.exposedName, attacker);
                    }
                }
                else if (track.name == "Box Script")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())//遍歷軌道中所有的clip
                    {
                        MySuperClip mySuperClip = (MySuperClip)clip.asset;//取得clip實例
                        MySuperBehaviour mySuperBehaviour = mySuperClip.template;//取得behaviour
                        mySuperClip.am.exposedName = System.Guid.NewGuid().ToString();//更改初始值

                        pd.SetReferenceValue(mySuperClip.am.exposedName, victim);
                    }
                }
                else if (track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Box Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }

            pd.Evaluate();
            pd.Play();
        }
        else if (timelineName == "leverUp")
        {

            pd.playableAsset = Instantiate(leverUp);
            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())//遍歷所有軌道
            {
                if (track.name == "Player Script")
                {
                    pd.SetGenericBinding(track, attacker);//綁定
                    foreach (var clip in track.GetClips())//遍歷軌道中所有的clip
                    {
                        MySuperClip mySuperClip = (MySuperClip)clip.asset;//取得clip實例
                        MySuperBehaviour mySuperBehaviour = mySuperClip.template;//取得behaviour
                        mySuperClip.am.exposedName = System.Guid.NewGuid().ToString();//更改初始值
                        //Reference =字典 設定clip的ExposedReference 必須使用這個方法 直接賦值無法成功 將am傳進clip
                        pd.SetReferenceValue(mySuperClip.am.exposedName, attacker);
                    }
                }
                else if (track.name == "Lever Script")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())//遍歷軌道中所有的clip
                    {
                        MySuperClip mySuperClip = (MySuperClip)clip.asset;//取得clip實例
                        MySuperBehaviour mySuperBehaviour = mySuperClip.template;//取得behaviour
                        mySuperClip.am.exposedName = System.Guid.NewGuid().ToString();//更改初始值

                        pd.SetReferenceValue(mySuperClip.am.exposedName, victim);
                    }
                }
                else if (track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Lever Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }
            }

            pd.Evaluate();
            pd.Play();

        }
    }
}
