using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelCurve : BaseBullet
{
    private float m_plusSpeed; //加速で加算される速さ
    private float m_deg; //カーブする際の曲がり具合

    protected override void Update()
    {
        base.Update();
        base.PlusSpeed(m_plusSpeed);
        base.PlusMainDeg(m_deg);
    }

    public void SetPlusSpeedAndDeg(float speed, float deg)
    {
        m_plusSpeed = speed;
        m_deg = deg;
    }
}
