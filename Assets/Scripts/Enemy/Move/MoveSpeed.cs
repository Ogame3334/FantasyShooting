using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeed : BaseEnemyMove
{
    [SerializeField] private float m_speed;

    protected override void Start()
    {
        base.Start();
        base.SetMoveTime(MoveTimeCalc());
    }

    protected override void Update()
    {
        base.Update();
    }

    private float[] MoveTimeCalc()
    {
        Vector3 startPoint = GetStartPoint();
        Vector3[] throughPoint = GetThroughPoint();
        float[] moveTime = new float[throughPoint.Length];

        moveTime[0] = Vector3.Distance(startPoint, throughPoint[0]) / m_speed;
        for (int i = 1; i < throughPoint.Length; i++)
        {
            moveTime[i] = Vector3.Distance(throughPoint[i - 1], throughPoint[i]) / m_speed;
        }

        return moveTime;
    }
}
