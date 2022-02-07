using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/In-Game Event System")]
public class InGameEventSystem : ScriptableObject
{
    public UnityAction<int> MoveAuthRemainEvent;
    public UnityAction OutOfMoveEvent;

    public void RaiseMoveAuthRemainEvent(int remain)
    {
        MoveAuthRemainEvent?.Invoke(remain);
    }

    public void RaiseOutOfMoveEvent()
    {
        OutOfMoveEvent?.Invoke();
    }
}
