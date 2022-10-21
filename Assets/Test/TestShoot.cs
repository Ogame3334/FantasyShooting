using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ogame.Enemy;
using Ogame.System;

public class TestShoot : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private float m_interval;
    private byte m_WitchExe = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine("PerfectFreezeControl");
        }
        PerfectFreeze();
        /*if (Input.GetKeyDown(KeyCode.C))
        {
            EnemyBulletInfo enemyBulletInfo = new EnemyBulletInfo(Bullet.Get(0), Bullet.GetImage(0, 1),
                this.transform.position, Calc.ToTargetDeg(this.transform, player.transform), 3f);

            Bullet.ShootCircle(enemyBulletInfo, 1f, 20);
            //Bullet.ShootMulti(enemyBulletInfo);
            //Bullet.ShootMultiLine(enemyBulletInfo);
            //Bullet.ShootCircle(enemyBulletInfo);

            //GameObject bullet = Shooter.One(Getter.Bullet(0), Getter.BulletImage(0, 0), this.transform, Finder.Deg(this.transform, player.transform), 3f);

            //bullet.GetComponent<AccelCurve>().SetPlusSpeedAndDeg(2f, 30f);

            //Shoot.Multi(Getter.Bullet(0), this.transform, 270, 3f, 1f, 5);

            //Shoot.MultiLine(Getter.Bullet(0), this.transform, Finder.Deg(this.transform, player.transform), 3f, 20f, 4);

            //GameObject[] bul = Shooter.Circle(Get.Bullet(0), Getter.BulletImage(0, 0), this.transform, 0, 3f, 1f, 24);
            for (int i = 0; i < 24; i++)
            {
                //bul[i].GetComponent<Curve>().SetChangeDeg(30f);
                //bul[i].GetComponent<SpriteRenderer>().sprite = BulletStorage.instance.a;
            }
        }*/

        /*if (Input.GetKey(KeyCode.X))
        {
            if (interval <= 0)
            {
                EnemyBulletInfo enemyBulletInfo = new EnemyBulletInfo(Bullet.Get(0), Bullet.GetImage(0, Random.Range(1, 10)),
                this.transform.position, Random.Range(-180f, 180f), Random.Range(2f, 5f));

                Bullet.ShootOne(enemyBulletInfo);

                interval = 0.01f;
            }

            interval -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (GameObject bullet in bullets)
            {
                bullet.GetComponent<BaseBullet>().SetSpeed(0);
                bullet.GetComponent<SpriteRenderer>().sprite = Bullet.GetImage(0, 0);
            }
        }
        if (Input.GetKey(KeyCode.V))
        {
            EnemyBulletInfo enemyBulletInfo = new EnemyBulletInfo(Bullet.Get(0), Bullet.GetImage(0, 7),
                this.transform.position, Calc.ToTargetDeg(this.transform, player.transform), 3f);

            if (interval <= 0)
            {
                Bullet.ShootMultiLine(enemyBulletInfo, 20f, 3);

                interval = 0.1f;
            }

            interval -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<SpriteRenderer>().sprite == Bullet.GetImage(0, 0))
                {
                    Destroy(bullet.GetComponent<Straight>());
                    bullet.AddComponent<Accel>();
                    bullet.GetComponent<Accel>().SetPlusSpeed(0.5f);
                    bullet.GetComponent<Accel>().SetSpeed(0);
                    bullet.GetComponent<Accel>().SetMainDeg(Random.Range(-180f, 180f));
                }
            }
        }



        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (GameObject bullet in bullets)
            {
                Destroy(bullet);
            }
        }*/
    }

    private void PerfectFreeze()
    {
        if (m_WitchExe == 1)
        {
            if (m_interval <= 0)
            {
                EnemyBulletInfo enemyBulletInfo = new EnemyBulletInfo(Bullet.Get(0), Bullet.GetImage(0, Random.Range(1, 10)),
                this.transform.position, Random.Range(-180f, 180f), Random.Range(2f, 5f));

                Bullet.ShootOne(enemyBulletInfo);

                m_interval = 0.01f;
            }

            m_interval -= Time.deltaTime;
        }
        else if (m_WitchExe == 2)
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<SpriteRenderer>().sprite == Bullet.GetImage(0, 0))
                {
                    bullet.GetComponent<BaseBullet>().SetSpeed(0);
                    bullet.GetComponent<Accel>().SetPlusSpeed(0);
                }
                else
                {
                    bullet.GetComponent<BaseBullet>().SetSpeed(0);
                    bullet.GetComponent<SpriteRenderer>().sprite = Bullet.GetImage(0, 0);
                }
            }
        }
        else if (m_WitchExe == 3)
        {
            EnemyBulletInfo enemyBulletInfo = new EnemyBulletInfo(Bullet.Get(0), Bullet.GetImage(0, 7),
            this.transform.position, Calc.ToTargetDeg(this.transform, player.transform), 3f);

            if (m_interval <= 0)
            {
                Bullet.ShootMultiLine(enemyBulletInfo, 20f, 3);

                m_interval = 0.1f;
            }

            m_interval -= Time.deltaTime;
        }
        else if (m_WitchExe == 4)
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<SpriteRenderer>().sprite == Bullet.GetImage(0, 0))
                {
                    if (bullet.GetComponent<Straight>())
                    {
                        Destroy(bullet.GetComponent<Straight>());
                        bullet.AddComponent<Accel>();
                        bullet.GetComponent<Accel>().SetSpeed(0);
                    }
                    bullet.GetComponent<Accel>().SetPlusSpeed(0.5f);
                    bullet.GetComponent<Accel>().SetMainDeg(Random.Range(-180f, 180f));
                }
            }
        }
    }

    IEnumerator PerfectFreezeControl()
    {
        m_WitchExe = 1;

        yield return new WaitForSeconds(3f);

        m_WitchExe = 2;

        yield return null;

        m_WitchExe = 0;

        yield return new WaitForSeconds(0.6f);

        m_WitchExe = 3;

        yield return new WaitForSeconds(0.6f);

        m_WitchExe = 4;

        yield return null;

        m_WitchExe = 0;
    }
}