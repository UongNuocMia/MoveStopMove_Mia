using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScaleBooster : Booster
{
    protected override void OnInit()
    {
        base.OnInit();
        booster = BoosterType.WeaponScale;
    }
}
