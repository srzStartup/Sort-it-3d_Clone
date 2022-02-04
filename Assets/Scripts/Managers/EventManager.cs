using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event EventHandler<HoldersReadyEventArgs> HoldersReady;
    //public static event EventHandler<int> LevelCompleted;
    public static event EventHandler<Holder> HolderClicked;
    public static event EventHandler<Holder> HolderCompleted;

    private void Awake()
    {
        LevelBuilder.HoldersReady += HoldersReady;
        //GameManager.LevelCompleted += LevelCompleted;
        Holder.HolderClicked += HolderClicked;
        Holder.HolderCompleted += HolderCompleted;
    }
}
