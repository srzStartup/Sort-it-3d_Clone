using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Level Strategy")]
public class LevelStrategy : ScriptableObject
{
    public static event UnityAction<int> moveAuthRemainEvent;
    public static event UnityAction OutOfMoveEvent;

    [SerializeField] private int _holderSize;
    [Tooltip("If it is set to 0, player's moves will be ignored.")]
    [SerializeField] private int _maxMove;
    [SerializeField] private InGameEventSystem _inGameEventChannel;

    private int _moveCountdown;

    public bool considerMoveCount => _maxMove != 0;

    public int MaxMove => _maxMove;
    public int HolderSize => _holderSize;

    void OnEnable() => _moveCountdown = _maxMove;
    void OnDisable() => _moveCountdown = 0;

    public void MakeMove()
    {
        if (considerMoveCount)
        {
            _moveCountdown--;
            if (_moveCountdown == 0)
                _inGameEventChannel.RaiseOutOfMoveEvent();
            _inGameEventChannel.RaiseMoveAuthRemainEvent(_moveCountdown);
        }
    }
}
