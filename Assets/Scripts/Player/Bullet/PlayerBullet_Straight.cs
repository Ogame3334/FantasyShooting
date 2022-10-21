using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet_Straight : PlayerBaseBullet
{
    protected override void Update()
    {
        base.Update();
        ChangeRotation();
    }

    private void ChangeRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, GetMainDeg());
    }
}
