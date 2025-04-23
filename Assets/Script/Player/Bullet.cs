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
        //지금은모든게머즐이들어가므로 나중에머즐없는경우에만 따로if추가할것
        transform.Rotate(new Vector3(3, 0, 0) * Time.deltaTime * 180);
    }

    private void removePooling()
    {
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
