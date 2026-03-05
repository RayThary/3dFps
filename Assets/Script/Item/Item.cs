using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Gold,
        Ammo,
        Hp,
    }

    [SerializeField] private ItemType itemType;
    private Rigidbody rigid;

    private int amount = 1;
    private Unit unit;


    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float floatHeight = 0.2f;

    private bool groundCheck = false;
    private bool isInRange = false;
    private float startPosY;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInRange = true;
            unit = GameManager.instance.GetUnit;

        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rigid.isKinematic = true;
            groundCheck = true;
            startPosY = transform.position.y;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(PopUp());
    }
    private IEnumerator PopUp()
    {
        float rand = Random.Range(0, 1);
        int coinValue;
        if (rand < 0.45f)
            coinValue = 2;
        else if (rand < 0.80f)
            coinValue = 3;
        else
            coinValue = 4;

        amount = coinValue * 13;

        if (UIManager.instance != null && UIManager.instance.TestMode.isOn)
        {
            amount *= 2;
        }

        rigid.isKinematic = false;

        Vector3 dir = new Vector3(Random.Range(-1, 1), Random.Range(0.1f, 0.4f), Random.Range(-1, 1));

        rigid.AddForce(dir * 5f, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);
        rigid.isKinematic = true;
        startPosY = transform.position.y;
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {
        itemMoving();
        unitTracking();
    }

    private void itemMoving()
    {
        // 회전
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        // 위아래 움직임
        if (groundCheck)
        {
            float newY = startPosY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
    }
    private void unitTracking()
    {
        if (!isInRange)
            return;

        float dis = Vector3.Distance(transform.position, unit.transform.position);

        if (dis < 3)
        {
            switch (itemType)
            {
                case ItemType.Gold:
                    unit.Gold += amount;
                    break;

                case ItemType.Ammo:
                    Weapon curWeapon = unit.UnitWeapon;
                    bool addAmmo = curWeapon.AddAmmo();
                    if (!addAmmo)
                    {
                        unit.Gold += amount / 2;
                    }
                    break;
                case ItemType.Hp:
                    int hp = Mathf.FloorToInt(13 * Random.Range(0.8f, 1.3f));
                    unit.UnitHp = Mathf.Min(unit.UnitHp + hp, unit.CurrentStat.unitMaxHp);
                    break;
            }

            isInRange = false;
            groundCheck = false;
            unit = null;
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
        else
        {

            transform.position = Vector3.MoveTowards(transform.position, unit.transform.position, 20 * Time.deltaTime);
        }

    }
}
