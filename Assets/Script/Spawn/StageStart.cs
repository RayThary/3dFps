using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStart : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("스테이지 시작");
            GameManager.instance.IsStageStarted = true;
        }
    }
}
