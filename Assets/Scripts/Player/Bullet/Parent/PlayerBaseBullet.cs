using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseBullet : MonoBehaviour
{
    [SerializeField] private float m_speed = 3f; //弾が進む速度
    [SerializeField] private float m_mdg = 90f; //弾が進む方向への角度(deg)

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
    public float GetSpeed()//速度設定
    {
        return m_speed;
    }
    public void PlusSpeed(float speed)//速度加算
    {
        m_speed += speed * Time.deltaTime;
    }

    public void SetMainDeg(float mdg)//角度設定
    {
        m_mdg = mdg;
    }
    public float GetMainDeg()//角度設定
    {
        return m_mdg;
    }
    public void PlusMainDeg(float mdg)//角度加算
    {
        m_mdg += mdg * Time.deltaTime;
    }

    protected void DeleteBullet(float x, float y)//範囲外に出た弾の削除
    {
        if (this.transform.position.x < -x | this.transform.position.x > x | this.transform.position.y < -y | this.transform.position.y > y)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
