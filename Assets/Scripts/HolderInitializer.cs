using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HolderInitializer : MonoBehaviour
{
    public static event EventHandler<HoldersReadyEventArgs> HoldersReady;

    [SerializeField] private Transform _holderParent;
    [SerializeField] private int holderSize;
    [SerializeField] private Transform ballPrefab;
    [SerializeField] private List<Material> ballMaterials;

    private List<Transform> holderTransforms;
    private const string slotsName = "Slot_";

    private void Start()
    {
        CollectChildren();
    }

    private void CollectChildren()
    {
        holderTransforms = _holderParent.GetComponentsInChildren<Transform>()
            .ToList()
            .FindAll(child => !child.Equals(_holderParent) && child.parent.Equals(_holderParent));

        float ballSize = ballPrefab.GetComponent<Renderer>().bounds.size.y;
        float slotPositionY = ballSize;

        holderTransforms.ForEach(holderTransform =>
        {
            List<Transform> slots = new List<Transform>();
            for (int i = 0; i < holderSize; i++)
            {
                if (i % holderSize == 0) slotPositionY = ballSize;
                GameObject emptyGameObject = new GameObject($"{slotsName}{i}")
                {
                    transform =
                    {
                        parent = holderTransform,
                        localPosition = new Vector3(0, slotPositionY, 0)
                    }
                };
                slotPositionY += ballSize;
                slots.Add(emptyGameObject.transform);
            }

            List<Transform> balls = holderTransform.GetComponentsInChildren<Transform>()
                .ToList()
                .FindAll(child => !child.Equals(holderTransform) && !child.name.StartsWith(slotsName));

            Holder holder = holderTransform.GetComponent<Holder>() ?? holderTransform.gameObject.AddComponent<Holder>();
            holder.order = holderTransforms.IndexOf(holderTransform);
            holder.slots = slots;
            holder.BallsToSlots(balls);
        });

        HoldersReady?.Invoke(this, new HoldersReadyEventArgs(holderTransforms, ballMaterials));

        Destroy(this);
        Destroy(gameObject);
    }
}
