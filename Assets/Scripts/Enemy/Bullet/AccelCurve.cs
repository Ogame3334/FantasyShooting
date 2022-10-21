using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelCurve : BaseBullet
{
    private float m_plusSpeed; //‰Á‘¬‚Å‰ÁZ‚³‚ê‚é‘¬‚³
    private float m_deg; //ƒJ[ƒu‚·‚éÛ‚Ì‹È‚ª‚è‹ï‡

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
