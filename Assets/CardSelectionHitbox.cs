using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectionHitbox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseover;
    public bool Mouseover => mouseover && !CardSelectionHitboxGlobalCooldownManager.CoolingDown;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseover = false;
    }
}
