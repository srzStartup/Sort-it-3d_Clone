using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelReadyEventArgs : EventArgs
{
    public List<Transform> holders { get; }
    public float popupHeight { get; }
    public int holdersNeedToBeCompleted { get; }
    public int level { get; }
    public LevelStrategy levelStrategy;

    public LevelReadyEventArgs(List<Transform> holders, int level, int holdersNeedToBeCompleted, float popupHeight, LevelStrategy levelStrategy)
    {
        this.holders = holders;
        this.level = level;
        this.holdersNeedToBeCompleted = holdersNeedToBeCompleted;
        this.popupHeight = popupHeight;
        this.levelStrategy = levelStrategy;
    }
}