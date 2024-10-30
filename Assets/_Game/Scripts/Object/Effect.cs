using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : GameUnit
{
    [SerializeField] private ParticleSystem thisEffect;
    private float timeExist = 0;

    public ParticleSystem ThisEffect => thisEffect;
    private void Start()
    {
        timeExist = thisEffect.main.duration;
    }

    private void Update()
    {
        timeExist -= Time.deltaTime;
        if (timeExist <= 0)
            SimplePool.Despawn(this);
    }
}
