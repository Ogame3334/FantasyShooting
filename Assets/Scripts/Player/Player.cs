using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ogame.System;
using Ogame.Player;
using Ogame.Talk;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject[] m_playerBullet;

    [SerializeField] private int m_HP; //HP
    [SerializeField] private byte m_Bomb; //ボム
    [SerializeField] private int m_Power; //パワー
    private bool m_DamageRecieve; //falseで無敵モード
    private int m_BulletWitch;

    private bool m_PlayPermission = true; //操作できるか
    private bool m_ShootPermission = true; //発射できるか
    private float m_ShootInterval = 0.1f;
    private float m_ShootTimer = 0;


    [SerializeField] private GameObject talkManager;
    [SerializeField] private TextAsset a;
    [SerializeField] private TextAsset b;

    private void Start()
    {
        m_HP = GameManager.instance.GetPlayerHP();
        m_Bomb = GameManager.instance.GetPlayerBomb();
        m_Power = GameManager.instance.GetPlayerPower();
        m_DamageRecieve = GameManager.instance.GetPlayerDamageRecieve();
        m_BulletWitch = 0;
    }

    private void Update()
    {
        if (m_PlayPermission)
        {
            Move();
            if (m_ShootPermission)
            {
                Shoot();
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //talkManager.GetComponent<TalkManager>().SetTalkScript(a);
            //talkManager.SetActive(true);
            Talk.StartUp(talkManager, TalkScript.Get("Ogame", "TestDialog"));

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //talkManager.GetComponent<TalkManager>().SetTalkScript(b);
            //talkManager.SetActive(true);
            //Talk.StartUp(talkManager, TalkScript.Get("Ogame", "TestDialog2"));
        }

        MoveClamp();
    }

    private void Move()
    {
        float x = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        float y = Mathf.Round(Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //低速移動
            if (x != 0 && y != 0)
            {
                transform.position += new Vector3(3f * x, 3f * y, 0) * Time.deltaTime / Mathf.Sqrt(2);
            }
            else
            {
                transform.position += new Vector3(3f * x, 3f * y, 0) * Time.deltaTime;
            }
        }
        else
        {
            //通常移動
            if (x != 0 && y != 0)
            {
                transform.position += new Vector3(5f * x, 5f * y, 0) * Time.deltaTime / Mathf.Sqrt(2);
            }
            else
            {
                transform.position += new Vector3(5f * x, 5f * y, 0) * Time.deltaTime;
            }
        }
    }
    private void MoveClamp()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.2f, 3.2f), Mathf.Clamp(transform.position.y, -4.5f, 4.5f), 0);
    }

    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Z) & m_ShootTimer <= 0)
        {
            Instantiate(m_playerBullet[m_BulletWitch], this.transform.position, Quaternion.Euler(0, 0, 90f));
            m_ShootTimer = m_ShootInterval;
        }
        m_ShootTimer -= Time.deltaTime;
    }



    //SetまたはGet
    public void SetShootInterval(float shootInterval)
    {
        m_ShootInterval = shootInterval;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyBullet") & m_DamageRecieve)
        {
            m_HP -= other.GetComponent<BaseBullet>().GetGiveDamage();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("PowerUpItem"))
        {
            m_Power++;
            GameManager.instance.SetPlayerPower(m_Power);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("BombUpItem"))
        {
            m_Bomb++;
            GameManager.instance.SetPlayerBomb(m_Bomb);
            Destroy(other.gameObject);
        }
    }
}