using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ogame.System;
using Ogame.Enemy;

public class MoveQuadCurve : BaseEnemyMoveGraph
{
    private float m_x = -5f;


    protected override void MoveEnemy()
    {
        m_x += Time.deltaTime;
        transform.position = Movement.Quad(Calc.QuadCurve(new Vector2(-2f, 3f), new Vector2(0, 0), new Vector2(4f, 2f)), m_x * base.GetMoveSpeed());
    }
    protected override void DeleteThis()
    {
        if (this.transform.position.x < -5.5f | this.transform.position.x > 5.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
