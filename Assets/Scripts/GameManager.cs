using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float popupHeight;

    private List<Holder> holders;
    private List<Material> ballMaterials;

    private Holder activeHolder = null;

    // z values has to set
    private void Awake()
    {
        HolderInitializer.HoldersReady += OnHoldersReady;
        Clickable.ClickHolder += OnClickHolder;
    }

    private void OnClickHolder(object sender, int order)
    {
        if (activeHolder != null)
        {
            var clickedHolder = holders.Find(holder => holder.order.Equals(order));
            var selectedBall = clickedHolder.balls.First();
            selectedBall.position = new Vector3(selectedBall.position.x, selectedBall.position.y + popupHeight, selectedBall.position.z);
            activeHolder = clickedHolder;
        }
        else
        {
            Debug.Log(activeHolder.order);
            if (activeHolder.order.Equals(order))
            {
                Transform selectedBall = activeHolder.balls.First();
                selectedBall.position = new Vector3(selectedBall.position.x,
                                                    selectedBall.position.y - popupHeight,
                                                    selectedBall.position.z);
                activeHolder = null;
            }
            else
            {
                Holder selectedHolder = holders.Find(holder => holder.order.Equals(order));
                if (selectedHolder.balls.Count.Equals(3))
                    return;
                Transform selectedBall = activeHolder.balls.First();
                // TODO: ball needs to be moved to selectedHolder.
            }
        }
    }

    private void OnHoldersReady(object sender, HoldersReadyEventArgs eventArgs)
    {
        holders = eventArgs.holders;
        ballMaterials = eventArgs.ballMaterials;
    }
}
