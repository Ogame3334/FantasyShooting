using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ogame.System;

public class PlayerBullet_Chase : PlayerBaseBullet
{
    protected override void Update()
    {
        base.Update();
        base.SetMainDeg(Calc.ToTargetDeg(this.transform.position, FindNearestEnemy()));
    }

    private Vector3 FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Vector3 nearestEnemy = this.transform.position + new Vector3(0, 10f, 0);
        float distance = 100f;

        foreach (GameObject enemy in enemies)
        {
            float enemyDis = Vector3.Distance(this.transform.position, enemy.transform.position);
            bool isinScreen = enemy.transform.position.x <= 4f & enemy.transform.position.x >= -4f & enemy.transform.position.y <= 5.2f & enemy.transform.position.y >= -5.2f;

            if (isinScreen)
            {
                if (distance > enemyDis)
                {
                    distance = enemyDis;
                    nearestEnemy = enemy.transform.position;
                }
            }
        }


        return nearestEnemy;
    }
}
