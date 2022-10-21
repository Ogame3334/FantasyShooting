using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private int m_playerHP = 300; //Player‚Ì‘Ì—Í
    private byte m_playerBomb = 0; //Player‚Ìƒ{ƒ€Žc—Ê
    private int m_playerPower = 0; //Player‚ÌÊßÜ°°°°°!!!
    private bool m_playerDamageRecieve = true;


    //PlayerŠÖ˜A
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
