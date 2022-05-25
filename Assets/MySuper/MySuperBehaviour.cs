using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySuperBehaviour : PlayableBehaviour
{
    public ActorManager am;
    public float myFloat;

    //PlayableDirector pd;//導演

    public override void OnPlayableCreate(Playable playable)
    {

    }


    public override void OnGraphStart(Playable playable)//序列開始播放
    {
        //pd = (PlayableDirector)playable.GetGraph().GetResolver();
    }




    public override void OnGraphStop(Playable playable)
    {
        //if (pd != null)
        //pd.playableAsset = null;

    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        try
        {
            am.LockUnlockActorController(false);

        }
        catch (System.Exception)
        {


        }
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        try
        {
            am.LockUnlockActorController(true);

        }
        catch (System.Exception)
        {


        }

    }
}
