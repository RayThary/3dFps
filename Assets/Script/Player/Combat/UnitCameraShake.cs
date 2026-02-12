using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCameraShake
{
    private CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;

    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float dashFOV = 75f;

    public void SetUp(CinemachineVirtualCamera _cam)
    {
        cam = _cam;
        noise = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartDashEffect()
    {
        cam.m_Lens.FieldOfView = dashFOV;

        noise.m_AmplitudeGain = 0.7f;
        noise.m_FrequencyGain = 1.3f;
    }

    public void EndDashEffect()
    {
        cam.m_Lens.FieldOfView = normalFOV;

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
