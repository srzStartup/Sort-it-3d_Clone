using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HolderInitializer : MonoBehaviour
{
    public static event EventHandler<HoldersReadyEventArgs> HoldersReady;

    [SerializeField] private List<Transform> holderTransforms;
    [SerializeField] private List<Material> ballMaterials;

    [SerializeField] private int holderSizeMax;

    private List<Holder> holderList;

    private void Awake()
    {
        holderList = new List<Holder>();
    }

    private void Start()
    {
        CreateHolders();
    }

    private void CreateHolders()
    {
        holderTransforms.ForEach(holder =>
        {
            List<Transform> ballsToBeQueued = holder.GetComponentsInChildren<Transform>().ToList();
            ballsToBeQueued = ballsToBeQueued.FindAll((ballToBeQueued) => !ballToBeQueued.Equals(holder));

            if (ballsToBeQueued.Count <= holderSizeMax)
            {
                ballsToBeQueued.Sort((previous, current) =>
                {
                    if (current.position.y > previous.position.y) return 1;
                    else return -1;
                });

                var index = holder.GetComponent<Clickable>().index;
                holderList.Add(new Holder(holder, index, ballsToBeQueued));
            }
        });

        HoldersReady?.Invoke(this, new HoldersReadyEventArgs(holderList, ballMaterials));
    }
}