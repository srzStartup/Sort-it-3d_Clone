using System;

using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    public static event EventHandler<int> ClickHolder;

    [SerializeField] private int _index;
    public int index => _index;


    public void OnPointerClick(PointerEventData eventData)
    {
        ClickHolder?.Invoke(this, _index);
    }
}
