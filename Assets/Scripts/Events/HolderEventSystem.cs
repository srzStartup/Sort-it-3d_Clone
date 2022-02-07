using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Holder Event Channel")]
public class HolderEventSystem : ScriptableObject
{
    public UnityAction<int> HoldersCompletedInLevel;
    public UnityAction<LevelReadyEventArgs> HoldersReady;
    public UnityAction<Holder> HolderClicked;
    public UnityAction<Holder> HolderCompleted;

    public void RaiseHoldersCompletedInLevelEvent(int level)
    {
        HoldersCompletedInLevel?.Invoke(level);
    }

    public void RaiseHoldersReadyEvent(LevelReadyEventArgs eventArgs)
    {
        HoldersReady?.Invoke(eventArgs);
    }

    public void RaiseHolderClickedEvent(Holder holder)
    {
        HolderClicked?.Invoke(holder);
    }

    public void RaiseHolderCompletedEvent(Holder holder)
    {
        HolderCompleted?.Invoke(holder);
    }
}
