using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private HolderEventSystem _holderEventChannel;
    [SerializeField] private InGameEventSystem _inGameEventChannel;

    private int _currentLevel;
    private LevelStrategy _currentLevelStrategy;
    public bool isStarted { get; private set; }
    public bool isEnded { get; private set; }
    public bool isFailed { get; private set; }

    public override void Awake()
    {
        base.Awake();

        _holderEventChannel.HoldersCompletedInLevel += OnHoldersCompletedInLevel;
        _holderEventChannel.HoldersReady += OnHoldersReady;

        _inGameEventChannel.OutOfMoveEvent += OnOutOfMove;
        _inGameEventChannel.RestartLevelRequestedEvent += OnRestartLevelRequested;
    }

    private void Start()
    {
        isEnded = false;
        isStarted = true;
        _inGameEventChannel.RaiseGameStartedEvent();
    }

    private void OnDestroy()
    {
        _holderEventChannel.HoldersCompletedInLevel -= OnHoldersCompletedInLevel;
        _holderEventChannel.HoldersReady -= OnHoldersReady;

        _inGameEventChannel.OutOfMoveEvent -= OnOutOfMove;
        _inGameEventChannel.RestartLevelRequestedEvent -= OnRestartLevelRequested;
    }

    private void OnHoldersReady(LevelReadyEventArgs e)
    {
        _currentLevel = e.level;
        _currentLevelStrategy = e.levelStrategy;
        _inGameEventChannel.RaiseLevelStartedEvent(e.level, e.levelStrategy.considerMoveCount, e.levelStrategy.MaxMove);
    }

    private void OnHoldersCompletedInLevel(int level)
    {
        if (SceneManager.sceneCountInBuildSettings > level)
        {
            _inGameEventChannel.RaiseLevelCompletedEvent(level);
        }
        else
        {
            isEnded = true;
            _inGameEventChannel.RaiseGameEndedEvent();
        }
    }

    private void OnOutOfMove()
    {
        isFailed = true;
        _inGameEventChannel.RaiseLevelFailedEvent(_currentLevel);
    }

    private void OnRestartLevelRequested(int level)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _inGameEventChannel.RaiseLevelStartedEvent(level, _currentLevelStrategy.considerMoveCount, _currentLevelStrategy.MaxMove);
    }
}
