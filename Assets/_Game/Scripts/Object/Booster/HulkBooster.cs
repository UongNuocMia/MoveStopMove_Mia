using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HulkBooster : Booster
{
    protected override void OnInit()
    {
        base.OnInit();
        booster = EBoosterType.Hulk;
    }
}
