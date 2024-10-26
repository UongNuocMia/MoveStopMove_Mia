using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    public static float GetTimeCurrentAnim(Animator anim, string currentAnim)
    {
        float animTime = 0;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == currentAnim)
            {
                animTime = clip.length;
            }
        }
        return animTime;
    }

    public static T RandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(1, values.Length);
        return (T)values.GetValue(random);
    }
}