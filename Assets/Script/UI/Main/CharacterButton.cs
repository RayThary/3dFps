using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("¿Ã¶ó¿È");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("³ª°¨");
    }

    private void Start()
    {
        

    }
}
