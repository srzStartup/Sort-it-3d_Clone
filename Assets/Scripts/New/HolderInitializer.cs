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

    private void Start()
    {
        CollectChilds();
    }

    private void CollectChilds()
    {
        holderTransforms = _holderParent.GetComponentsInChildren<Transform>()
            .ToList()
            .FindAll(child => !child.Equals(_holderParent));

        float ballSize = ballPrefab.GetComponent<Renderer>().bounds.size.y;
        float slotPositionY = ballSize;

        holderTransforms.ForEach(holderTransform =>
        {
            List<Transform> slots = new List<Transform>();
            for (int i = 0; i < holderSize; i++)
            {
                GameObject emptyGameObject = new GameObject($"Slot_{i}");
                emptyGameObject.transform.parent = holderTransform;
                emptyGameObject.transform.localPosition = new Vector3(0, slotPositionY, 0);
                slotPositionY += ballSize;
                slots.Add(emptyGameObject.transform);
            }

            List<Transform> balls = holderTransform.GetComponentsInChildren<Transform>()
                .ToList()
                .FindAll(child => !child.Equals(holderTransform) && !child.name.StartsWith("Slot_"));

            Holder holder = holderTransform.gameObject.AddComponent<Holder>();
            holder.order = holderTransforms.IndexOf(holderTransform);
            holder.slots = slots;
            holder.balls = balls;
        });

        HoldersReady?.Invoke(this, new HoldersReadyEventArgs(holderTransforms, ballMaterials));
    }
}
