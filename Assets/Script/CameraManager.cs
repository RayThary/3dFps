using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private LayerMask firstPersonMask;
    [SerializeField] private LayerMask thirdPersonMask;

    //true¸é 1ÀÎÄª falseÀÏ¶© 3ÀÎÄª
    private bool isFirstPerson = true;
    private bool isSwitching = false;



    [SerializeField] private CinemachineVirtualCamera POVCam;

    [SerializeField] private CinemachineVirtualCamera ChangeCam;

    private CinemachineBrain mainCamera;

    void Start()
    {
        mainCamera = Camera.main.GetComponent<CinemachineBrain>();

        ChangeCam.Priority = 5;
        Camera.main.cullingMask = firstPersonMask.value;
        POVCam.Priority = 11;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isSwitching)
        {
            //StartCoroutine(camChange());

        }
    }

    private IEnumerator camChange(bool _isMelee)
    {
        isSwitching = true;

        if (_isMelee)
        {
            ChangeCam.Priority = 13;
            Camera.main.cullingMask = thirdPersonMask.value;

            yield return new WaitForSeconds(0.8f);

            ChangeCam.Priority = 5;
            POVCam.Priority = 9;
        }
        else
        {
            ChangeCam.Priority = 13;

            yield return new WaitForSeconds(0.3f);

            ChangeCam.Priority = 5;
            Camera.main.cullingMask = firstPersonMask.value;
            POVCam.Priority = 11;
        }

        isFirstPerson = !isFirstPerson;
        isSwitching = false;
    }

    public void weaponSwitched(Weapon _weapon)
    {
        bool isMelee = _weapon.IsMelee;
        if (isMelee == isFirstPerson)
        {
            StartCoroutine(camChange(isMelee));
        }
    }


}
