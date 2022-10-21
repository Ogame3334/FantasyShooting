using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStorage : MonoBehaviour
{
    public static BulletStorage instance = null;

    //’e‚ÌƒvƒŒƒnƒu
    [SerializeField] private GameObject[] bullets;

    //’e‚Ì‰æ‘œ
    [SerializeField] private ChildArray[] Images;

    public GameObject GetBullet(int i)
    {
        return bullets[i];
    }

    public Sprite GetBulletImage(int i, int j)
    {
        return Images[i].GetBulletImage(j);
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

[System.Serializable]
public class ChildArray
{
    [SerializeField] private Sprite[] bullet;

    public Sprite GetBulletImage(int j)
    {
        return bullet[j];
    }
}