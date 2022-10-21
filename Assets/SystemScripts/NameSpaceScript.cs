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
            //t1Ç©ÇÁt2Ç÷ÇÃäpìx(Deg)ÇåvéZ
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
            //îCà”ÇÃ3ì_Çí ÇÈìÒéüä÷êîÇÃåWêîÇãÅÇﬂÇÈ
            public static float[] QuadCurve(Transform t1, Transform t2, Transform t3)
            {
                float[] c = new float[3];//åWêîa, b, c
                float[] tx = { t1.position.x, t2.position.x, t3.position.x };
                float[] ty = { t1.position.y, t2.position.y, t3.position.y };

                c[0] = ((ty[0] - ty[1]) * (tx[0] - tx[2]) - (ty[0] - ty[2]) * (tx[0] - tx[1])) / ((tx[0] - tx[1]) * (tx[0] - tx[2]) * (tx[1] - tx[2]));//aÇãÅÇﬂÇÈ
                c[1] = (ty[0] - ty[1]) / (tx[0] - tx[1]) - c[0] * (tx[0] + tx[1]);//bÇãÅÇﬂÇÈ
                c[2] = ty[0] - c[0] * tx[0] * tx[0] - c[1] * tx[0];//cÇãÅÇﬂÇÈ

                return c;
            }
            public static float[] QuadCurve(Vector3 t1, Vector3 t2, Vector3 t3)
            {
                float[] c = new float[3];//åWêîa, b, c
                float[] tx = { t1.x, t2.x, t3.x };
                float[] ty = { t1.y, t2.y, t3.y };

                c[0] = ((ty[0] - ty[1]) * (tx[0] - tx[2]) - (ty[0] - ty[2]) * (tx[0] - tx[1])) / ((tx[0] - tx[1]) * (tx[0] - tx[2]) * (tx[1] - tx[2]));//aÇãÅÇﬂÇÈ
                c[1] = (ty[0] - ty[1]) / (tx[0] - tx[1]) - c[0] * (tx[0] + tx[1]);//bÇãÅÇﬂÇÈ
                c[2] = ty[0] - c[0] * tx[0] * tx[0] - c[1] * tx[0];//cÇãÅÇﬂÇÈ

                return c;
            }
        }
    }
    //TalkÇ…ä÷Ç∑ÇÈä÷êîÇäiî[
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

    //PlayerÇ…ä÷Ç∑ÇÈä÷êîÇäiî[
    namespace Player
    {

    }

    //EnemyÇ…ä÷Ç∑ÇÈä÷êîÇäiî[
    namespace Enemy
    {
        public class EnemyBulletInfo// : MonoBehaviour
        {
            private GameObject bullet; //íeÇÃÉvÉåÉnÉu
            private Sprite image; //íeÇÃâÊëú
            private Vector3 launchPoint; //î≠éÀì_
            private float mdg; //íeÇÃêiÇﬁäpìx(âEå¸Ç´Ç™0ÅãÅA-180Å`180)
            private float speed; //íeÇÃë¨Ç≥

            private float difSpeed; //[Multi] ë¨ìxïœâª
            private int number; //[Multi][MultiLine][Circle] î≠éÀêî
            private float difDeg; //[MultiLine] äpìxÇÃà·Ç¢
            private float innerDiameter; //[Circle] ì‡åa

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
            //íeÇàÍî≠î≠éÀ
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
            //àŸÇ»ÇÈë¨ìxÇÃíeÇnumberå¬î≠éÀ
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
            //ï°êîÉâÉCÉìÇ≈î≠éÀ
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
            //â~å`Ç…numberå¬î≠éÀ
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

            //BulletStorageÇ©ÇÁíeÇéÊìæ
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