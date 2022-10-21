using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ogame.System;
using Ogame.Enemy;

public abstract class BaseEnemyMoveGraph : MonoBehaviour
{
    private float m_speed = 1f; //Enemy‚ÌˆÚ“®‘¬“x
    private bool m_dir; //ˆÚ“®‚·‚éx²•ûŒüB0‚ª³

    virtual protected void Update()
    {
        MoveEnemy();
        DeleteThis();
    }

    virtual protected void MoveEnemy()
    {

    }

    virtual protected void DeleteThis()
    {
        
    }


    public void SetMoveSpeed(float speed)
    {
        m_speed = speed;
    }
    public float GetMoveSpeed()
    {
        return m_speed;
    }
    public void SetMoveDirection(bool dir)
    {
        m_dir = dir;
    }
}
