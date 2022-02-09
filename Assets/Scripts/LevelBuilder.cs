using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private Transform _ballPrefab;
    [SerializeField] private Transform _holderPrefab;

    [Header("--- Holder Settings && Leveling ---")]
    [SerializeField] private Transform _holderParent;
    [SerializeField] private LevelStrategy _levelStrategy;
    [SerializeField] private float _popupHeight = 1.0f;
    [SerializeField] private int _level;

    [Header("--- Event Channels ---")]
    [SerializeField] private HolderEventSystem _holderEventChannel;

    [Header("Background Colors")]
    [SerializeField]
    private List<Color> _backgroundColors = new List<Color>();

    private const string _slotsNameStartsWith = "Slot_";

    private List<Transform> holderTransforms;
    private int ballsCount;

    private void Start()
    {
        CameraSettings();

        CollectHolders();

        Destroy(gameObject);
    }

    private void CollectHolders()
    {
        holderTransforms = _holderParent.GetComponentsInChildren<Transform>()
            .ToList()
            .FindAll(child => !child.Equals(_holderParent) && child.parent.Equals(_holderParent));

        float ballSize = _ballPrefab.GetComponent<Renderer>().bounds.size.y;
        float ballExtent = _ballPrefab.GetComponent<Renderer>().bounds.extents.y;

        foreach (Transform holderTransform in holderTransforms)
        {
            List<Transform> slots = CreateSlots(holderTransform, ballExtent, ballSize);

            List<Transform> balls = holderTransform.GetComponentsInChildren<Transform>()
                .ToList()
                .FindAll(child => !child.Equals(holderTransform) && !child.name.StartsWith(_slotsNameStartsWith));

            Holder holder = holderTransform.GetComponent<Holder>() ?? holderTransform.gameObject.AddComponent<Holder>();

            holder.order = holderTransforms.IndexOf(holderTransform);
            holder.slots = slots;
            holder.BallsToSlots(balls);
            holder.SetHolderEventChannel(_holderEventChannel);

            ballsCount += balls.Count;
        }

        int holdersNeedToBeCompleted = ballsCount / _levelStrategy.HolderSize;

        _holderEventChannel.RaiseHoldersReadyEvent(
            new LevelReadyEventArgs(
                holderTransforms,
                _level,
                holdersNeedToBeCompleted,
                _popupHeight,
                _levelStrategy
         ));
    }

    private List<Transform> CreateSlots(Transform parent, float startInLocal, float separation)
    {
        List<Transform> slots = new List<Transform>();
        float slotPositionY = startInLocal;

        for (int i = 0; i < _levelStrategy.HolderSize; i++)
        {
            GameObject emptyGameObject = new GameObject(_slotsNameStartsWith + i)
            {
                transform =
                    {
                        parent = parent,
                        localPosition = new Vector3(0, slotPositionY, 0)
                    }
            };
            slotPositionY += separation;
            slots.Add(emptyGameObject.transform);
        }

        return slots;
    }

    private void CameraSettings()
    {
        Camera mainCam = Camera.main;

        // Being sure PhysicsRaycaster Component added on camera.
        if (!mainCam.GetComponent<PhysicsRaycaster>())
        {
            mainCam.gameObject.AddComponent<PhysicsRaycaster>();
        }
        // Checking if EventSystem exists in the scene.
        if (!FindObjectOfType<EventSystem>())
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = _backgroundColors[Random.Range(0, _backgroundColors.Count)];

        Vector3 camPosOffset = _levelStrategy.camPositionOffset;
        mainCam.transform.position = new Vector3(
            _ground.position.x + camPosOffset.x,
            _ground.position.y + camPosOffset.y,
            _ground.position.z + camPosOffset.z
         );

        mainCam.transform.rotation = Quaternion.Euler(_levelStrategy.camRotationOffset);
    }
}
