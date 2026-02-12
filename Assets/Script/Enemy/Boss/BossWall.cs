using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossWall : MonoBehaviour
{
    [SerializeField] private Transform wallMesh;
    [SerializeField] private float spawnScaleTime = 0.5f;

    private Vector3 targetTransform;
    [SerializeField]private Transform dangerZone;
    [SerializeField] private float warningTime = 2f; 
    private Transform inCircle;

    private BoxCollider box;

    [SerializeField] private float damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance.GetUnit.TakeDamge(damage);
        }
    }
    
    private void Awake()
    {
        inCircle = dangerZone.GetChild(0);
        box = GetComponent<BoxCollider>();
    }

    private void Update()
    {
      
    }
   
    public void SetStart(Vector3 _targetTrs)
    {
        targetTransform = _targetTrs;
        transform.position = _targetTrs;
        StartCoroutine(DangerRoutine());
    }

    private IEnumerator DangerRoutine()
    {
        dangerZone.gameObject.SetActive(true);

        inCircle.localScale = Vector3.zero;
        Vector3 innerTargetScale = Vector3.one;

        float t = 0f;
        while (t < warningTime)
        {
            t += Time.deltaTime;
            float lerp = t / warningTime;
            inCircle.localScale = Vector3.Lerp(Vector3.zero, innerTargetScale, lerp);
            yield return null;
        }

        dangerZone.gameObject.SetActive(false);
        StartCoroutine(RockRoutine());
        StartCoroutine(HitTriggerRoutine());
        
    }

    private IEnumerator HitTriggerRoutine()
    {
        box.enabled = true;
        yield return new WaitForSeconds(spawnScaleTime - 0.1f);
        box.enabled = false;
    }

    private IEnumerator RockRoutine()
    {
        float time = 0f;
        transform.position = new Vector3(targetTransform.x, -10, targetTransform.z);
        wallMesh.gameObject.SetActive(true);
        Vector3 startPos = transform.position;

        while (time < spawnScaleTime)
        {
            time += Time.deltaTime;
            float lerp = time / spawnScaleTime;
            transform.position = Vector3.Lerp(startPos, targetTransform, lerp);
            yield return null;
        }

    }

    public void RemoveWall()
    {
        StartCoroutine(removeWall());
    }
   private IEnumerator removeWall()
    {
        float time = 0f;
        Vector3 originVec = transform.position;

        while (time < 1)
        {
            time += Time.deltaTime;
            float shake = Mathf.Sin(time * 40f) * 0.1f;
            transform.position = originVec + new Vector3(shake, 0, 0);
            yield return null;
        }
        PoolingManager.Instance.RemovePoolingObject(gameObject);
    }
}
