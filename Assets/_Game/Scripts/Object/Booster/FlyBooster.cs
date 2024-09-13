using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBooster : Booster
{
    protected override void OnInit()
    {
        base.OnInit();
        booster = Enum.BoosterEnum.Fly;
    }
}
