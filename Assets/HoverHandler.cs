using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public AlternativeWeapon alternativeWeapon;
    public AlternativeWeaponHandler alternativeWeaponHandler;

    private GraphicsManager vision;
    private bool abort = false;
    private float timeToValidate = .3f;

    void Start()
    {
        vision = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GraphicsManager>();
    }

    IEnumerator Validate()
    {
        yield return new WaitForSeconds(vision.ponderate(timeToValidate));

        if (abort) { abort = false; }
        else
        {
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
