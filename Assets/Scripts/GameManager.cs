using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float popupHeight;

    private List<Transform> holders;
    private List<Material> ballMaterials;

    private Holder activeHolder = null;

    // z values has to set
    private void Awake()
    {
        HolderInitializer.HoldersReady += OnHoldersReady;
        Clickable.ClickHolder += OnClickHolder;
    }

    private void OnClickHolder(Clickable sender, PointerEventData eventData)
    {
        //if (activeHolder == null)
        //{
        //    var clickedHolder = holders.Find(holder => holder.order.Equals(sender.index));

        //    if (clickedHolder.isEmpty) return;
        //    var selectedBall = clickedHolder.balls.First();
        //    // moving the ball was selected by popopHeight
        //    selectedBall.localPosition = new Vector3(selectedBall.localPosition.x, selectedBall.localPosition.y + popupHeight, selectedBall.localPosition.z);

        //    activeHolder = clickedHolder;
        //}
        //else
        //{
        //    // checking holder's order number if the same holder or not
        //    if (activeHolder.order.Equals(sender.index))
        //    {
        //        // if the same holder is selected then move the last selected ball in the previous position.
        //        Transform selectedBall = activeHolder.balls.First();
        //        selectedBall.position = new Vector3(selectedBall.position.x,
        //                                            selectedBall.position.y - popupHeight,
        //                                            selectedBall.position.z);
        //        activeHolder = null;
        //    }
        //    else
        //    {
        //        Holder selectedHolder = holders.Find(holder => holder.order.Equals(sender.index));
        //        if (!selectedHolder.HasAvailableSlot)
        //            return;
        //        Transform selectedBall = activeHolder.Pop();
        //        selectedHolder.Add(selectedBall);
        //        activeHolder = null;
        //    }
        //}
    }

    private void OnHoldersReady(object sender, HoldersReadyEventArgs eventArgs)
    {
        holders = eventArgs.holders;
        ballMaterials = eventArgs.ballMaterials;
    }
}
