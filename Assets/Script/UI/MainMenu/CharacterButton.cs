using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
    }

    void OnDisable()
    {
        canvasGroup.alpha = 0.5f;
    }

    [SerializeField] private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
    }
}
