using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private int m_playerHP = 300; //Playerの体力
    private byte m_playerBomb = 0; //Playerのボム残量
    private int m_playerPower = 0; //Playerのﾊﾟﾜｰｰｰｰｰ!!!
    private bool m_playerDamageRecieve = true;


    //Player関連
    public void SetPlayerHP(int hP)
    {
        m_playerHP = hP;
    }
    public int GetPlayerHP()
    {
        return m_playerHP;
    }
    public void SetPlayerBomb(byte bomb)
    {
        m_playerBomb = bomb;
    }
    public byte GetPlayerBomb()
    {
        return m_playerBomb;
    }
    public void SetPlayerPower(int power)
    {
        m_playerPower = power;
    }
    public int GetPlayerPower()
    {
        return m_playerPower;
    }
    public void SetPlayerDamageRecieve(bool damageRecieve)
    {
        m_playerDamageRecieve = damageRecieve;
    }
    public bool GetPlayerDamageRecieve()
    {
        return m_playerDamageRecieve;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
