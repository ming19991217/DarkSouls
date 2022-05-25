using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySuperClip : PlayableAsset, ITimelineClipAsset
{
    public MySuperBehaviour template = new MySuperBehaviour();
    public ExposedReference<ActorManager> am;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MySuperBehaviour>.Create(graph, template);
        MySuperBehaviour clone = playable.GetBehaviour();

        clone.am = am.Resolve(graph.GetResolver());
        return playable;
    }
}
