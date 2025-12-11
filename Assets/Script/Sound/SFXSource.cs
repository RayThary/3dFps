using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSource : MonoBehaviour
{
    private void OnDisable()
    {
        SoundManager.instance.removePooling(gameObject);
    }
}
