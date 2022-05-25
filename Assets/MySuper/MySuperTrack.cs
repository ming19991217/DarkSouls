using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0.3679245f, 0.1155537f)]
[TrackClipType(typeof(MySuperClip))]
[TrackBindingType(typeof(ActorManager))]
public class MySuperTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MySuperMixerBehaviour>.Create (graph, inputCount);
    }
}
