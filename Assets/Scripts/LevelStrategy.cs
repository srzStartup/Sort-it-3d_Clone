using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Level Strategy")]
public class LevelStrategy : ScriptableObject
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 _cameraPositionOffset = new Vector3(0, 13.75f, -13.63f);
    [SerializeField] private Vector3 _cameraRotationOffset = new Vector3(35.0f, .0f, .0f);

    [Header("--- Level ---")]
    [SerializeField] private int _holderSize;
    [Tooltip("If it is set to 0, player's moves will be ignored.")]
    [SerializeField] private int _maxMove;

    [Header("--- Event Channels ---")]
    [SerializeField] private InGameEventSystem _inGameEventChannel;

    private int _moveCountdown;

    public bool considerMoveCount => _maxMove != 0;

    public Vector3 camPositionOffset => _cameraPositionOffset;
    public Vector3 camRotationOffset => _cameraRotationOffset;

    public int MaxMove => _maxMove;
    public int HolderSize => _holderSize;
    
    void OnEnable()
    {
        _moveCountdown = _maxMove;
        _inGameEventChannel.LevelStartedEvent += OnLevelStarted;
    }
    void OnDisable()
    {
        _moveCountdown = 0;
        _inGameEventChannel.LevelStartedEvent -= OnLevelStarted;

    }

    private void OnLevelStarted(int level, bool considerMoveCount, int maxMove)
    {
        _moveCountdown = _maxMove;
    }

    public void MakeMove()
    {
        if (considerMoveCount)
        {
            if (_moveCountdown == 0)
            {
                _inGameEventChannel.RaiseOutOfMoveEvent();
            }
            else
            {
                _moveCountdown--;
                _inGameEventChannel.RaiseMoveMadeEvent(_moveCountdown);
            }
        }
    }
}
