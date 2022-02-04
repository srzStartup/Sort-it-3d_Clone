using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBuilder : MonoBehaviour
{
    public static event EventHandler<HoldersReadyEventArgs> HoldersReady;

    [SerializeField] private Transform _ground;
    [SerializeField] private Vector3 _cameraPositionOffset = new Vector3(0, 13.75f, -13.63f);
    [SerializeField] private Vector3 _cameraRotationOffset = new Vector3(35.0f, .0f, .0f);
    [SerializeField] private CameraClearFlags _camClearFlags;
    [SerializeField] private Color _camBackgroundColor;
    [SerializeField] private Transform _holderParent;
    [SerializeField] private int holderSize;
    [SerializeField] private float popupHeight;
    [SerializeField] private Transform ballPrefab;
    [SerializeField] private List<Material> ballMaterials;
    [SerializeField] private int level;
    [SerializeField] private string slotsName = "Slot_";

    private List<Transform> holderTransforms;
    private int ballsCount;

    private void Start()
    {
        if (!Camera.main.GetComponent<PhysicsRaycaster>())
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        if (!FindObjectOfType<EventSystem>())
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        CameraSettings();

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

            ballsCount += balls.Count;
        });

        int holdersNeedToBeCompleted = CalculateHoldersNeedToBeCompleted(holderSize, ballsCount);

        HoldersReady?.Invoke(this, new HoldersReadyEventArgs(holderTransforms, level, holdersNeedToBeCompleted, popupHeight, ballMaterials));

        Destroy(gameObject);
    }

    private void CameraSettings()
    {
        Camera _cam = Camera.main;
        _cam.transform.position = new Vector3(_ground.position.x + _cameraPositionOffset.x,
                                              _ground.position.y + _cameraPositionOffset.y,
                                              _ground.position.z + _cameraPositionOffset.z);
        _cam.transform.rotation = Quaternion.Euler(_cameraRotationOffset);

        if (!_camClearFlags.Equals(CameraClearFlags.Nothing))
        {
            _cam.clearFlags = _camClearFlags;
            _cam.backgroundColor = _camBackgroundColor;
        }
    }

    private int CalculateHoldersNeedToBeCompleted(int holderSize, int ballsCount)
    {
        return ballsCount / holderSize;
    }
}
