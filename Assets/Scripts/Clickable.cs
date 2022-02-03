using System;

using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    public delegate void ClickedEventHandler(Clickable sender, PointerEventData eventData);
    public static event ClickedEventHandler ClickHolder;

    [SerializeField] protected int _index;
    public int index => _index;


    public void OnPointerClick(PointerEventData eventData)
    {
        ClickHolder?.Invoke(this, eventData);
    }
}
