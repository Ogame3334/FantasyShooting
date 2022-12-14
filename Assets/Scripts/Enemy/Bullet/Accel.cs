using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accel : BaseBullet
{
    private float m_plusSpeed; //加速で加算される速さ

    protected override void Update()
    {
        base.Update();
        base.PlusSpeed(m_plusSpeed);
    }

    public void SetPlusSpeed(float speed)
    {
        m_plusSpeed = speed;
    }
}
