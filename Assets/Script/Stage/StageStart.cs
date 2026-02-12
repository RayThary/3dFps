using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStart : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Invoke("stageStart", 0.4f);
        }
    }

    private void stageStart()
    {
        GameManager.instance.IsStageStarted = true;
    }

}
