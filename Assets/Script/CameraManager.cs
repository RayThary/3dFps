using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LayerMask firstPersonMask;
    [SerializeField] private LayerMask thirdPersonMask;

    //3¿Œƒ™Ω√¿€¿œ∂© false
    private bool isFirstPerson = false;
    private bool isSwitching = false;

    public CinemachineVirtualCamera POVCam;

    public CinemachineVirtualCamera ChangeCam;

    public CinemachineBrain mainCamera;

    void Start()
    {
        mainCamera = Camera.main.GetComponent<CinemachineBrain>();
        GameManager.instance.FirstPersonCheck = isFirstPerson;
        //1¿Œƒ™ Ω√¿€
        StartCoroutine(camChange());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isSwitching)
        {
            StartCoroutine(camChange());

        }
    }

    private IEnumerator camChange()
    {
        isSwitching = true;

        if (isFirstPerson)
        {
            ChangeCam.Priority = 13;
            Camera.main.cullingMask = thirdPersonMask.value;

            yield return new WaitForSeconds(1);

            ChangeCam.Priority = 5;
            POVCam.Priority = 9;
        }
        else
        {
            ChangeCam.Priority = 13;

            yield return new WaitForSeconds(1);

            ChangeCam.Priority = 5;
            Camera.main.cullingMask = firstPersonMask.value;
            POVCam.Priority = 11;
        }

        isFirstPerson = !isFirstPerson;
        GameManager.instance.FirstPersonCheck = isFirstPerson;
        isSwitching = false;
    }

}
