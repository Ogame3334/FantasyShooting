using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ogame.System;
using Ogame.Enemy;

public abstract class BaseEnemyMove : MonoBehaviour
{
    [SerializeField] private Vector3 m_startPoint;
    [SerializeField] private Vector3[] m_throughPoint;
    [SerializeField] private float[] m_moveTime;

    private int m_ExeNumber= 0;
    private float m_timer = 0;

    virtual protected void Start()
    {
        this.transform.position = m_startPoint;
    }

    virtual protected void Update()
    {
        Move();
        ExeNumberControl();
    }

    virtual protected void Move()
    {
        float mdg;
        float speed = 0;

        if (m_ExeNumber == 0)
        {
            mdg = Calc.ToTargetDeg(m_startPoint, m_throughPoint[0]);
            speed = Vector3.Distance(m_startPoint, m_throughPoint[0]) / m_moveTime[0];
        }
        else
        {
            mdg = Calc.ToTargetDeg(m_throughPoint[m_ExeNumber - 1], m_throughPoint[m_ExeNumber]);
            speed = Vector3.Distance(m_throughPoint[m_ExeNumber - 1], m_throughPoint[m_ExeNumber]) / m_moveTime[m_ExeNumber];
        }

        float vX;
        float vY;
        vX = speed * Mathf.Cos(mdg * Mathf.Deg2Rad);
        vY = speed * Mathf.Sin(mdg * Mathf.Deg2Rad);

        this.transform.position += new Vector3(vX, vY, 0) * Time.deltaTime;
    }

    protected void ExeNumberControl()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_moveTime[m_ExeNumber])
        {
            m_ExeNumber++;
            m_timer = 0;
        }
    }

    protected void SetMoveTime(float[] moveTime)
    {
        m_moveTime = moveTime;
    }
    protected Vector3 GetStartPoint()
    {
        return m_startPoint;
    }
    protected Vector3[] GetThroughPoint()
    {
        return m_throughPoint;
    }
}
