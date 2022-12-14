using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ogame.System;

namespace Ogame
{
    namespace System
    {
        public class Calc : MonoBehaviour //----------------------------------------Calc
        {
            //t1からt2への角度(Deg)を計算
            public static float ToTargetDeg(Transform t1, Transform t2)
            {
                Vector2 p1 = t1.position;
                Vector2 p2 = t2.position;
                Vector2 dt = p2 - p1;
                float deg = Mathf.Atan2(dt.y, dt.x) * Mathf.Rad2Deg;

                return deg;
            }
            public static float ToTargetDeg(Vector3 t1, Vector3 t2)
            {
                Vector2 p1 = t1;
                Vector2 p2 = t2;
                Vector2 dt = p2 - p1;
                float deg = Mathf.Atan2(dt.y, dt.x) * Mathf.Rad2Deg;

                return deg;
            }
            //任意の3点を通る二次関数の係数を求める
            public static float[] QuadCurve(Transform t1, Transform t2, Transform t3)
            {
                float[] c = new float[3];//係数a, b, c
                float[] tx = { t1.position.x, t2.position.x, t3.position.x };
                float[] ty = { t1.position.y, t2.position.y, t3.position.y };

                c[0] = ((ty[0] - ty[1]) * (tx[0] - tx[2]) - (ty[0] - ty[2]) * (tx[0] - tx[1])) / ((tx[0] - tx[1]) * (tx[0] - tx[2]) * (tx[1] - tx[2]));//aを求める
                c[1] = (ty[0] - ty[1]) / (tx[0] - tx[1]) - c[0] * (tx[0] + tx[1]);//bを求める
                c[2] = ty[0] - c[0] * tx[0] * tx[0] - c[1] * tx[0];//cを求める

                return c;
            }
            public static float[] QuadCurve(Vector3 t1, Vector3 t2, Vector3 t3)
            {
                float[] c = new float[3];//係数a, b, c
                float[] tx = { t1.x, t2.x, t3.x };
                float[] ty = { t1.y, t2.y, t3.y };

                c[0] = ((ty[0] - ty[1]) * (tx[0] - tx[2]) - (ty[0] - ty[2]) * (tx[0] - tx[1])) / ((tx[0] - tx[1]) * (tx[0] - tx[2]) * (tx[1] - tx[2]));//aを求める
                c[1] = (ty[0] - ty[1]) / (tx[0] - tx[1]) - c[0] * (tx[0] + tx[1]);//bを求める
                c[2] = ty[0] - c[0] * tx[0] * tx[0] - c[1] * tx[0];//cを求める

                return c;
            }
        }
    }
    //Talkに関する関数を格納
    namespace Talk
    {
        public class Talk : MonoBehaviour
        {
            public static void StartUp(GameObject talkManager, TextAsset textAsset)
            {
                talkManager.GetComponent<TalkManager>().SetTalkScript(textAsset);
                talkManager.SetActive(true);
            }
        }
        public class TalkScript : MonoBehaviour
        {
            public static TextAsset Get(string path, string scriptName)
            {
                return AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/Talk/" + path + "/" + scriptName + ".txt");
            }
        }
    }

    //Playerに関する関数を格納
    namespace Player
    {

    }

    //Enemyに関する関数を格納
    namespace Enemy
    {
        public class EnemyBulletInfo// : MonoBehaviour
        {
            private GameObject bullet; //弾のプレハブ
            private Sprite image; //弾の画像
            private Vector3 launchPoint; //発射点
            private float mdg; //弾の進む角度(右向きが0°、-180〜180)
            private float speed; //弾の速さ

            private float difSpeed; //[Multi] 速度変化
            private int number; //[Multi][MultiLine][Circle] 発射数
            private float difDeg; //[MultiLine] 角度の違い
            private float innerDiameter; //[Circle] 内径

            public EnemyBulletInfo(GameObject bullet, Sprite image, Vector3 launchPoint, float mdg, float speed)
            {
                this.bullet = bullet;
                this.image = image;
                this.launchPoint = launchPoint;
                this.mdg = mdg;
                this.speed = speed;
                this.difSpeed = 0;
                this.number = 0;
                this.difDeg = 0;
                this.innerDiameter = 0;
            }

            public GameObject GetBullet()
            {
                return bullet;
            }
            public void SetBullet(GameObject bullet)
            {
                this.bullet = bullet;
            }

            public Sprite GetBulletImage()
            {
                return image;
            }
            public void SetBulletImage(Sprite image)
            {
                this.image = image;
            }

            public Vector3 GetBulletLaunchPoint()
            {
                return launchPoint;
            }
            public void SetBulletTransform(Vector3 launchPoint)
            {
                this.launchPoint = launchPoint;
            }

            public float GetBulletMainDeg()
            {
                return mdg;
            }
            public void SetBulletMainDeg(float mdg)
            {
                this.mdg = mdg;
            }

            public float GetBulletSpeed()
            {
                return speed;
            }
            public void SetBulletSpeed(float speed)
            {
                this.speed = speed;
            }

            public float GetBulletDifSpeed()
            {
                return difSpeed;
            }
            public void SetBulletDifSpeed(float difSpeed)
            {
                this.difSpeed = difSpeed;
            }

            public int GetBulletNumber()
            {
                return number;
            }
            public void SetBulletNumber(int number)
            {
                this.number = number;
            }

            public float GetBulletDifDeg()
            {
                return difDeg;
            }
            public void SetBulletDifDeg(float difDeg)
            {
                this.difDeg = difDeg;
            }

            public float GetBulletInnerDiameter()
            {
                return innerDiameter;
            }
            public void SetBulletInnerDiameter(float innerDiameter)
            {
                this.innerDiameter = innerDiameter;
            }
        }
        public class Bullet : MonoBehaviour //----------------------------------------Bullet
        {
            //弾を一発発射
            public static GameObject ShootOne(EnemyBulletInfo enemyBulletInfo)
            {
                GameObject bullet = enemyBulletInfo.GetBullet();
                Sprite image = enemyBulletInfo.GetBulletImage();
                Vector3 launchPoint = enemyBulletInfo.GetBulletLaunchPoint();
                float mdg = enemyBulletInfo.GetBulletMainDeg();
                float speed = enemyBulletInfo.GetBulletSpeed();

                GameObject bul = Instantiate(bullet, launchPoint, Quaternion.identity);
                bul.GetComponent<BaseBullet>().SetSpeed(speed);
                bul.GetComponent<BaseBullet>().SetMainDeg(mdg);
                bul.GetComponent<SpriteRenderer>().sprite = image;

                return bul;
            }
            public static GameObject ShootOne(GameObject bullet, Sprite image, Vector3 launchPoint, float mdg, float speed)
            {
                GameObject bul = Instantiate(bullet, launchPoint, Quaternion.identity);
                bul.GetComponent<BaseBullet>().SetSpeed(speed);
                bul.GetComponent<BaseBullet>().SetMainDeg(mdg);
                bul.GetComponent<SpriteRenderer>().sprite = image;

                return bul;
            }
            //異なる速度の弾をnumber個発射
            public static GameObject[] ShootMulti(EnemyBulletInfo enemyBulletInfo, float difSpd, int number)
            {
                GameObject bullet = enemyBulletInfo.GetBullet();
                Sprite image = enemyBulletInfo.GetBulletImage();
                Vector3 launchPoint = enemyBulletInfo.GetBulletLaunchPoint();
                float mdg = enemyBulletInfo.GetBulletMainDeg();
                float speed = enemyBulletInfo.GetBulletSpeed();

                GameObject[] bul = new GameObject[number];

                for (int i = 0; i < number; i++)
                {
                    bul[i] = Instantiate(bullet, launchPoint, Quaternion.identity);
                    bul[i].GetComponent<BaseBullet>().SetSpeed(speed);
                    bul[i].GetComponent<BaseBullet>().SetMainDeg(mdg);
                    bul[i].GetComponent<SpriteRenderer>().sprite = image;
                    speed += difSpd;
                }

                return bul;
            }
            public static GameObject[] ShootMulti(GameObject bullet, Sprite image, Vector3 launchPoint, float mdg, float speed, float difSpd, int number)
            {
                GameObject[] bul = new GameObject[number];

                for (int i = 0; i < number; i++)
                {
                    bul[i] = Instantiate(bullet, launchPoint, Quaternion.identity);
                    bul[i].GetComponent<BaseBullet>().SetSpeed(speed);
                    bul[i].GetComponent<BaseBullet>().SetMainDeg(mdg);
                    bul[i].GetComponent<SpriteRenderer>().sprite = image;
                    speed += difSpd;
                }

                return bul;
            }
            //複数ラインで発射
            public static GameObject[] ShootMultiLine(EnemyBulletInfo enemyBulletInfo, float difDeg, int number)
            {
                GameObject bullet = enemyBulletInfo.GetBullet();
                Sprite image = enemyBulletInfo.GetBulletImage();
                Vector3 launchPoint = enemyBulletInfo.GetBulletLaunchPoint();
                float mdg = enemyBulletInfo.GetBulletMainDeg();
                float speed = enemyBulletInfo.GetBulletSpeed();

                GameObject[] bul = new GameObject[number];

                if (number % 2 == 1)
                {
                    for (int i = -((number - 1) / 2); i <= (number - 1) / 2; i++)
                    {
                        int j = 0;
                        bul[j] = Instantiate(bullet, launchPoint, Quaternion.identity);
                        bul[j].GetComponent<BaseBullet>().SetSpeed(speed);
                        bul[j].GetComponent<BaseBullet>().SetMainDeg(mdg + difDeg * i / 2);
                        bul[j].GetComponent<SpriteRenderer>().sprite = image;
                        j++;
                    }
                }
                else
                {
                    for (int i = -(number - 1); i <= (number - 1); i += 2)
                    {
                        int j = 0;
                        bul[j] = Instantiate(bullet, launchPoint, Quaternion.identity);
                        bul[j].GetComponent<BaseBullet>().SetSpeed(speed);
                        bul[j].GetComponent<BaseBullet>().SetMainDeg(mdg + difDeg * i / 2);
                        bul[j].GetComponent<SpriteRenderer>().sprite = image;
                        j++;
                    }
                }

                return bul;
            }
            public static GameObject[] ShootMultiLine(GameObject bullet, Sprite image, Vector3 launchPoint, float mdg, float speed, float difDeg, int number)
            {
                GameObject[] bul = new GameObject[number];

                if (number % 2 == 1)
                {
                    for (int i = -((number - 1) / 2); i <= (number - 1) / 2; i++)
                    {
                        int j = 0;
                        bul[j] = Instantiate(bullet, launchPoint, Quaternion.identity);
                        bul[j].GetComponent<BaseBullet>().SetSpeed(speed);
                        bul[j].GetComponent<BaseBullet>().SetMainDeg(mdg + difDeg * i / 2);
                        bul[j].GetComponent<SpriteRenderer>().sprite = image;
                        j++;
                    }
                }
                else
                {
                    for (int i = -(number - 1); i <= (number - 1); i += 2)
                    {
                        int j = 0;
                        bul[j] = Instantiate(bullet, launchPoint, Quaternion.identity);
                        bul[j].GetComponent<BaseBullet>().SetSpeed(speed);
                        bul[j].GetComponent<BaseBullet>().SetMainDeg(mdg + difDeg * i / 2);
                        bul[j].GetComponent<SpriteRenderer>().sprite = image;
                        j++;
                    }
                }

                return bul;
            }
            //円形にnumber個発射
            public static GameObject[] ShootCircle(EnemyBulletInfo enemyBulletInfo, float innerDiameter, int number)
            {
                GameObject bullet = enemyBulletInfo.GetBullet();
                Sprite image = enemyBulletInfo.GetBulletImage();
                Vector3 launchPoint = enemyBulletInfo.GetBulletLaunchPoint();
                float mdg = enemyBulletInfo.GetBulletMainDeg();
                float speed = enemyBulletInfo.GetBulletSpeed();

                float difdeg = 360f / number;
                GameObject[] bul = new GameObject[number];

                for (int i = 0; i < number; i++)
                {
                    bul[i] = Instantiate(bullet, launchPoint + new Vector3(innerDiameter * Mathf.Cos((mdg + difdeg * i) * Mathf.Deg2Rad), innerDiameter * Mathf.Sin((mdg + difdeg * i) * Mathf.Deg2Rad)), Quaternion.identity);
                    bul[i].GetComponent<BaseBullet>().SetSpeed(speed);
                    bul[i].GetComponent<BaseBullet>().SetMainDeg(mdg + difdeg * i);
                    bul[i].GetComponent<SpriteRenderer>().sprite = image;
                }

                return bul;
            }
            public static GameObject[] ShootCircle(GameObject b, Sprite image, Vector3 launchPoint, float mdg, float speed, float innerDiameter, int number)
            {
                GameObject[] bullet = new GameObject[number];
                float difdeg = 360f / number;

                for (int i = 0; i < number; i++)
                {
                    bullet[i] = Instantiate(b, launchPoint + new Vector3(innerDiameter * Mathf.Cos((mdg + difdeg * i) * Mathf.Deg2Rad), innerDiameter * Mathf.Sin((mdg + difdeg * i) * Mathf.Deg2Rad)), Quaternion.identity);
                    bullet[i].GetComponent<BaseBullet>().SetSpeed(speed);
                    bullet[i].GetComponent<BaseBullet>().SetMainDeg(mdg + difdeg * i);
                    bullet[i].GetComponent<SpriteRenderer>().sprite = image;
                }

                return bullet;
            }

            //BulletStorageから弾を取得
            public static GameObject Get(int i)
            {
                return BulletStorage.instance.GetBullet(i);
            }
            public static Sprite GetImage(int i, int j)
            {
                return BulletStorage.instance.GetBulletImage(i, j);
            }
        }

        public class Movement : MonoBehaviour //----------------------------------------Movement
        {
            public static Vector3 Quad(float[] c, float t)
            {
                Vector3 point;
                point = new Vector3(t, c[0] * t * t + c[1] * t + c[2], 0);

                return point;
            }
        }
    }
}

namespace TMPro
{
    public static class TextMeshProUGUIExtensions
    {
        public static void SetOutline(this TextMeshProUGUI tmp, float outlineWitdh, float dilateRate)
        {
            tmp.outlineWidth = outlineWitdh;
            tmp.materialForRendering.SetFloat("_FaceDilate", dilateRate);
            tmp.UpdateFontAsset();
        }
    }
}