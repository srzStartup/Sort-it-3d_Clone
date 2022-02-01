using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float popupHeight;

    private List<Holder> holders;
    private List<Material> ballMaterials;

    private bool isBallSelected = false;
    private Transform lastSelectedBall;

    // z values has to set
    private void Awake()
    {
        HolderInitializer.HoldersReady += OnHoldersReady;
        Clickable.ClickHolder += OnClickHolder;
    }

    private void OnClickHolder(object sender, int order)
    {
        if (!isBallSelected)
        {
            var clickedHolder = holders.Find(holder => holder.order.Equals(order));
            var firstBall = clickedHolder.ballQueue.Peek();
            firstBall.position = new Vector3(firstBall.position.x, firstBall.position.y + popupHeight, firstBall.position.z);
            lastSelectedBall = firstBall;
            isBallSelected = true;
        }
        else
        {
            lastSelectedBall.position = new Vector3(lastSelectedBall.position.x,
                                                    lastSelectedBall.position.y - popupHeight,
                                                    lastSelectedBall.position.z);
            isBallSelected = false;
        }
    }

    private void Update()
    {
    }

    private void OnHoldersReady(object sender, HoldersReadyEventArgs eventArgs)
    {
        holders = eventArgs.holders;
        ballMaterials = eventArgs.ballMaterials;
    }
}
