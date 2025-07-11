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
    //보상룸쪽 or 엔딩룸쪽에도 GameManager.instance.IsStageStarted = false; 를통해서 스테이지넘어가기전 초기화필요

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
