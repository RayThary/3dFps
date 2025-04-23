using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        HandGun,
        SubMachine,
    }
    [SerializeField] private BulletType bulletType;



    void Start()
    {
        Invoke("removePooling", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        bulletMuzzle();
    }

    private void bulletMuzzle()
    {
        //���������Ը����̵��Ƿ� ���߿�������°�쿡�� ����if�߰��Ұ�
        transform.Rotate(new Vector3(3, 0, 0) * Time.deltaTime * 180);
    }

    private void removePooling()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
