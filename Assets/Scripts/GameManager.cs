using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float popupHeight;

    //private List<Transform> holders;
    //private List<Material> ballMaterials;

    private Holder activeHolder = null;
    private List<int> completedHolderIndexes;

    private void Awake()
    {
        HolderInitializer.HoldersReady += OnHoldersReady;

        Holder.ClickHolder += OnClickHolder;
        Holder.HolderCompleted += OnHolderCompleted;
    }

    private void OnClickHolder(Holder holder, PointerEventData eventData)
    {
        if (completedHolderIndexes.Contains(holder.order)) return;

        if (activeHolder == null)
        {
            if (holder.isEmpty) return;

            holder.Peek(popupHeight);
            activeHolder = holder;
        }
        else
        {
            if (activeHolder.Equals(holder))
            {
                holder.BackDown();
                activeHolder = null;
            }
            else
            {
                if (!holder.hasAvailableSlot) return;

                Transform selectedBall = activeHolder.Pop();
                holder.Add(selectedBall);

                activeHolder = null;
            }
        }
    }

    private void OnHolderCompleted(Holder sender)
    {
        completedHolderIndexes.Add(sender.order);
    }

    private void OnHoldersReady(object sender, HoldersReadyEventArgs eventArgs)
    {
        //holders = eventArgs.holders;
        //ballMaterials = eventArgs.ballMaterials;
        completedHolderIndexes = new List<int>();
    }
}
