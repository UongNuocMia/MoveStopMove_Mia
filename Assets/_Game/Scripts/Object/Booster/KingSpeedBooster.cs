using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSpeedBooster : Booster
{
    protected override void OnInit()
    {
        base.OnInit();
        booster = BoosterType.KingSpeed;
    }
}
