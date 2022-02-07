using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HolderManager : Singleton<HolderManager>
{
    [SerializeField] private HolderEventSystem _holderEventChannel;

    private int _level;
    private float _popupHeight;
    private Holder _activeHolder = null;
    private List<int> _completedHolderIndexes;
    private int _holdersNeedToBeCompleted;
    private LevelStrategy _levelStrategy;

    public override void Awake()
    {
        base.Awake();

        _holderEventChannel.HoldersReady += OnHoldersReady;
        _holderEventChannel.HolderClicked += OnHolderClicked;
        _holderEventChannel.HolderCompleted += OnHolderCompleted;
    }

    private void OnDestroy()
    {
        _holderEventChannel.HoldersReady -= OnHoldersReady;
        _holderEventChannel.HolderClicked -= OnHolderClicked;
        _holderEventChannel.HolderCompleted -= OnHolderCompleted;
    }

    private void OnHolderCompleted(Holder holder)
    {
        _completedHolderIndexes.Add(holder.order);

        if (_holdersNeedToBeCompleted == _completedHolderIndexes.Count)
            _holderEventChannel.RaiseHoldersCompletedInLevelEvent(_level);
    }

    private void OnHolderClicked(Holder holder)
    {
        if (_completedHolderIndexes.Contains(holder.order)) return;

        if (_activeHolder == null)
        {
            if (holder.isEmpty) return;

            holder.Popup(_popupHeight);
            _activeHolder = holder;
        }
        else
        {
            if (_activeHolder.Equals(holder))
            {
                holder.BackDown();
                _activeHolder = null;
            }
            else
            {
                if (!holder.hasAvailableSlot)
                {
                    _activeHolder.BackDown();
                    _activeHolder = null;
                    return;
                }

                Transform selectedBall = _activeHolder.Pop();
                holder.Add(selectedBall);

                _activeHolder = null;
                _levelStrategy.MakeMove();
            }
        }
    }

    private void OnHoldersReady(LevelReadyEventArgs eventArgs)
    {
        _popupHeight = eventArgs.popupHeight;
        _completedHolderIndexes = new List<int>();
        _level = eventArgs.level;
        _holdersNeedToBeCompleted = eventArgs.holdersNeedToBeCompleted;
        _levelStrategy = eventArgs.levelStrategy;
    }
}
