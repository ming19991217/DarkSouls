using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
/// <summary>
/// 混合特性
/// </summary>
public class MySuperMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        ActorManager trackBinding = playerData as ActorManager;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount();

        float tempSum = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<MySuperBehaviour> inputPlayable = (ScriptPlayable<MySuperBehaviour>)playable.GetInput(i);
            MySuperBehaviour input = inputPlayable.GetBehaviour();

            // Use the above variables to process each frame of this playable.
            //tempSum += input.myFloat * inputWeight;//與當前的片段權重相乘 獲得過度效果

        }

    }
}
