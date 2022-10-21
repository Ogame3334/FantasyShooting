using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] private float m_speed = 3f; //弾が進む速度
    [SerializeField] private float m_mdg = 0; //弾が進む方向への角度(deg)

    [SerializeField] private int m_GiveDamage = 1; //プレイヤーに与えるダメージ

    virtual protected void Update()
    {
        Move();
        DeleteBullet(5f, 6.5f);
    }

    virtual protected void Move()
    {
        float vX;//弾の進む量x
        float vY;//弾の進む量y
        vX = m_speed * Mathf.Cos(m_mdg * Mathf.Deg2Rad);
        vY = m_speed * Mathf.Sin(m_mdg * Mathf.Deg2Rad);

        transform.position += new Vector3(vX, vY) * Time.deltaTime;
    }

    public void SetSpeed(float speed)//速度設定
    {
        m_speed = speed;
    }
    public void PlusSpeed(float speed)//速度加算
    {
        m_speed += speed * Time.deltaTime;
    }

    public void SetMainDeg(float mdg)//角度設定
    {
        m_mdg = mdg;
    }
    public void PlusMainDeg(float mdg)//角度加算
    {
        m_mdg += mdg * Time.deltaTime;
    }
    public void SetGiveDamage(int giveDamage)//ダメージ量設定
    {
        m_GiveDamage = giveDamage;
    }
    public int GetGiveDamage()//ダメージ量取得
    {
        return m_GiveDamage;
    }

    protected void DeleteBullet(float x, float y)//範囲外に出た弾の削除
    {
        if (this.transform.position.x < -x | this.transform.position.x > x | this.transform.position.y < -y | this.transform.position.y > y)
        {
            Destroy(this.gameObject);
        }
    }
}
