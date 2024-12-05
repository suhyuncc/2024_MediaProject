using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Text_Click : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Dialogue_Manage.Instance.is_clicked = true;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Dialogue_Manage.Instance.is_clicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Dialogue_Manage.Instance.is_clicked = false;
    }

    
}
