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

    [Header("--- Camera Settings ---")]
    [SerializeField] private Vector3 _cameraPositionOffset = new Vector3(0, 13.75f, -13.63f);
    [SerializeField] private Vector3 _cameraRotationOffset = new Vector3(35.0f, .0f, .0f);
    [SerializeField] private CameraClearFlags _camClearFlags;
    [SerializeField] private Color _camBackgroundColor;

    [Header("--- Holder Settings ---")]
    [SerializeField] private Transform _holderParent;
    [SerializeField] private LevelStrategy _levelStrategy;
    [SerializeField] private float _popupHeight;
    [SerializeField] private string _slotsNameStartsWith = "Slot_";

    [Header("--- Leveling ---")]
    [SerializeField] private int _level;

    [Header("--- Event Channels ---")]
    [SerializeField] private HolderEventSystem _holderEventChannel;
    [SerializeField] private ParticleEventSystem _particleEventChannel;

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

        float ballSize = _ballPrefab.GetComponent<Renderer>().bounds.size.y;
        float slotPositionY = ballSize;

        holderTransforms.ForEach(holderTransform =>
        {
            List<Transform> slots = new List<Transform>();
            for (int i = 0; i < _levelStrategy.HolderSize; i++)
            {
                if (i % _levelStrategy.HolderSize == 0) slotPositionY = ballSize;
                GameObject emptyGameObject = new GameObject($"{_slotsNameStartsWith}{i}")
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
                .FindAll(child => !child.Equals(holderTransform) && !child.name.StartsWith(_slotsNameStartsWith));

            Holder holder = holderTransform.GetComponent<Holder>() ?? holderTransform.gameObject.AddComponent<Holder>();
            holder.order = holderTransforms.IndexOf(holderTransform);
            holder.slots = slots;
            holder.BallsToSlots(balls);
            holder.SetHolderEventChannel(_holderEventChannel);

            ballsCount += balls.Count;
        });

        int holdersNeedToBeCompleted = ballsCount / _levelStrategy.HolderSize;

        _holderEventChannel.RaiseHoldersReadyEvent(new LevelReadyEventArgs(holderTransforms,
                                                                           _level,
                                                                           holdersNeedToBeCompleted,
                                                                           _popupHeight,
                                                                           _levelStrategy));

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
            _cam.backgroundColor = _camBackgroundColor != null ? _camBackgroundColor : Random.ColorHSV(.0f, 1.0f, .0f, 1.0f, .0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
