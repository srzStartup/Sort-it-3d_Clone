using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/In-Game Event System")]
public class InGameEventSystem : ScriptableObject
{
    public UnityAction<int> MoveMadeEvent;
    public UnityAction OutOfMoveEvent;

    public UnityAction GameStartedEvent;
    public UnityAction GameEndedEvent;

    public UnityAction<int, bool, int> LevelStartedEvent;
    public UnityAction<int> LevelFailedEvent;
    public UnityAction<int> LevelCompletedEvent;

    public UnityAction<int> RestartLevelRequestedEvent;

    public void RaiseMoveMadeEvent(int remain)
    {
        MoveMadeEvent?.Invoke(remain);
    }

    public void RaiseOutOfMoveEvent()
    {
        OutOfMoveEvent?.Invoke();
    }

    public void RaiseLevelStartedEvent(int level, bool considerMoveCount, int maxMove)
    {
        LevelStartedEvent?.Invoke(level, considerMoveCount, maxMove);
    }

    public void RaiseLevelFailedEvent(int level)
    {
        LevelFailedEvent?.Invoke(level);
    }

    public void RaiseLevelCompletedEvent(int level)
    {
        LevelCompletedEvent?.Invoke(level);
    }

    public void RaiseGameEndedEvent()
    {
        GameEndedEvent?.Invoke();
    }

    public void RaiseGameStartedEvent()
    {
        GameStartedEvent?.Invoke();
    }

    public void RaiseRestartLevelRequestedEvent(int level)
    {
        RestartLevelRequestedEvent?.Invoke(level);
    }
}
