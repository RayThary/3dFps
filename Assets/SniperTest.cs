using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTest : MonoBehaviour
{
    public Transform target;
    public LineRenderer line;

    public bool shot = false;
    public float timer;
    public float shotTime = 3;
    public int excludeLayer;
    void Start()
    {
        target = GameManager.instance.GetUnit.transform;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        excludeLayer = ~LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (shot)
        {
            StartCoroutine(sniping());
            shot = false;
        }
    }
    IEnumerator sniping()
    {
        float endTime = Time.time + shotTime;
        line.enabled = true;
        while (endTime >= Time.time)
        {

            Vector3 dir = target.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, excludeLayer))
            {
                Vector3 endPoint = hit.point;
                endPoint.y += 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, endPoint);

            }
            yield return null;
        }

        Debug.Log("น฿ป็");
        line.enabled = false;
    }
}
