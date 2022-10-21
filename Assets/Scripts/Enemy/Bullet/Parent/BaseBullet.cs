using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] private float m_speed = 3f; //�e���i�ޑ��x
    [SerializeField] private float m_mdg = 0; //�e���i�ޕ����ւ̊p�x(deg)

    [SerializeField] private int m_GiveDamage = 1; //�v���C���[�ɗ^����_���[�W

    virtual protected void Update()
    {
        Move();
        DeleteBullet(5f, 6.5f);
    }

    virtual protected void Move()
    {
        float vX;//�e�̐i�ޗ�x
        float vY;//�e�̐i�ޗ�y
        vX = m_speed * Mathf.Cos(m_mdg * Mathf.Deg2Rad);
        vY = m_speed * Mathf.Sin(m_mdg * Mathf.Deg2Rad);

        transform.position += new Vector3(vX, vY) * Time.deltaTime;
    }

    public void SetSpeed(float speed)//���x�ݒ�
    {
        m_speed = speed;
    }
    public void PlusSpeed(float speed)//���x���Z
    {
        m_speed += speed * Time.deltaTime;
    }

    public void SetMainDeg(float mdg)//�p�x�ݒ�
    {
        m_mdg = mdg;
    }
    public void PlusMainDeg(float mdg)//�p�x���Z
    {
        m_mdg += mdg * Time.deltaTime;
    }
    public void SetGiveDamage(int giveDamage)//�_���[�W�ʐݒ�
    {
        m_GiveDamage = giveDamage;
    }
    public int GetGiveDamage()//�_���[�W�ʎ擾
    {
        return m_GiveDamage;
    }

    protected void DeleteBullet(float x, float y)//�͈͊O�ɏo���e�̍폜
    {
        if (this.transform.position.x < -x | this.transform.position.x > x | this.transform.position.y < -y | this.transform.position.y > y)
        {
            Destroy(this.gameObject);
        }
    }
}
