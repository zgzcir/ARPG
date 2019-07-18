using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UIListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,IPointerClickHandler
{
    public Action<PointerEventData> OnClickDown;
    public Action<object> OnClick;
    public Action<PointerEventData> OnCLickUp;
    public Action<PointerEventData> OnClickDrag;

    public object args;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnClickDown != null)
        {
            OnClickDown(eventData);
        }
    }
    public  void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null) 
        {
                     OnClick(args); 
                 }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnCLickUp != null)
        {
            OnCLickUp(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnClickDrag != null)
        {
            OnClickDrag(eventData);
        }
    }


}