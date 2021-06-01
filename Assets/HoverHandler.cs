using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public AlternativeWeapon alternativeWeapon;
    public AlternativeWeaponHandler alternativeWeaponHandler;

    private bool abort = false;
    private float timeToValidate = .5f;

    IEnumerator Validate()
    {
        yield return new WaitForSeconds(timeToValidate);

        if (abort) { abort = false; }
        else
        {
            Debug.Log("Triggered");
            alternativeWeapon.Trigger();
            alternativeWeaponHandler.Exit();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        abort = false;
        StartCoroutine(Validate());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        abort = true;
    }
}
