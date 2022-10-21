using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : BaseBullet
{
    private float m_deg; //ƒJ[ƒu‚·‚éÛ‚Ì‹È‚ª‚è‹ï‡

    protected override void Update()
    {
        base.Update();
        base.PlusMainDeg(m_deg);
    }

    public void SetChangeDeg(float deg)
    {
        m_deg = deg;
    }
}
