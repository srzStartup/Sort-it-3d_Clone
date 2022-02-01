using System;
using System.Collections.Generic;
using UnityEngine;

public class HoldersReadyEventArgs : EventArgs
{
    public List<Holder> holders { get; }
    public List<Material> ballMaterials { get; }

    public HoldersReadyEventArgs(List<Holder> holders, List<Material> ballMaterials)
    {
        this.holders = holders;
        this.ballMaterials = ballMaterials;
    }
}