using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelCurve : BaseBullet
{
    private float m_plusSpeed; //�����ŉ��Z����鑬��
    private float m_deg; //�J�[�u����ۂ̋Ȃ���

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
