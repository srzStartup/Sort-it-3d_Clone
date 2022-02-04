using System;
using System.Collections.Generic;
using UnityEngine;

public class HoldersReadyEventArgs : EventArgs
{
    public List<Transform> holders { get; }
    public List<Material> ballMaterials { get; }
    public float popupHeight { get; }
    public int holdersNeedToBeCompleted { get; }
    public int level { get; }

    public HoldersReadyEventArgs(List<Transform> holders, int level, int holdersNeedToBeCompleted, float popupHeight, List<Material> ballMaterials)
    {
        this.holders = holders;
        this.level = level;
        this.holdersNeedToBeCompleted = holdersNeedToBeCompleted;
        this.popupHeight = popupHeight;
        this.ballMaterials = ballMaterials;
    }
}