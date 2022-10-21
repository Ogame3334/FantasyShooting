using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseBullet : MonoBehaviour
{
    [SerializeField] private float m_speed = 3f; //’e‚ªi‚Ş‘¬“x
    [SerializeField] private float m_mdg = 90f; //’e‚ªi‚Ş•ûŒü‚Ö‚ÌŠp“x(deg)

    virtual protected void Update()
    {
        Move();
        DeleteBullet(5f, 6.5f);
    }

    virtual protected void Move()
    {
        float vX;//’e‚Ìi‚Ş—Êx
        float vY;//’e‚Ìi‚Ş—Êy
        vX = m_speed * Mathf.Cos(m_mdg * Mathf.Deg2Rad);
        vY = m_speed * Mathf.Sin(m_mdg * Mathf.Deg2Rad);

        transform.position += new Vector3(vX, vY) * Time.deltaTime;
    }

    public void SetSpeed(float speed)//‘¬“xİ’è
    {
        m_speed = speed;
    }
    public float GetSpeed()//‘¬“xİ’è
    {
        return m_speed;
    }
    public void PlusSpeed(float speed)//‘¬“x‰ÁZ
    {
        m_speed += speed * Time.deltaTime;
    }

    public void SetMainDeg(float mdg)//Šp“xİ’è
    {
        m_mdg = mdg;
    }
    public float GetMainDeg()//Šp“xİ’è
    {
        return m_mdg;
    }
    public void PlusMainDeg(float mdg)//Šp“x‰ÁZ
    {
        m_mdg += mdg * Time.deltaTime;
    }

    protected void DeleteBullet(float x, float y)//”ÍˆÍŠO‚Éo‚½’e‚Ìíœ
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
