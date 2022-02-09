using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _inLevelTextBackground;
    [SerializeField] private TextMeshProUGUI _endLevelText;
    [SerializeField] private TextMeshProUGUI _moveRemainText;
    [SerializeField] private Button _endLevelButton;

    [Header("--- Event Channels ---")]
    [SerializeField] private InGameEventSystem _inGameEventChannel;
    [SerializeField] private ParticleEventSystem _particleEventChannel;

    private int _currentLevel;
    private bool _isLevelFailed;
    private bool _isGameEnded;
    private bool _isLevelCompleted;

    public TextMeshProUGUI inLevelText => _inLevelTextBackground.transform.Find("InLevelText").GetComponent<TextMeshProUGUI>();

    public override void Awake()
    {
        base.Awake();

        _inGameEventChannel.GameStartedEvent += OnGameStarted;
        _inGameEventChannel.GameEndedEvent += OnGameEnded;

        _inGameEventChannel.LevelFailedEvent += OnLevelFailed;
        _inGameEventChannel.LevelCompletedEvent += OnLevelCompleted;
        _inGameEventChannel.LevelStartedEvent += OnLevelStarted;
    }

    private void OnDestroy()
    {
        _inGameEventChannel.GameStartedEvent -= OnGameStarted;
        _inGameEventChannel.GameEndedEvent -= OnGameEnded;

        _inGameEventChannel.LevelFailedEvent -= OnLevelFailed;
        _inGameEventChannel.LevelCompletedEvent -= OnLevelCompleted;
        _inGameEventChannel.LevelStartedEvent -= OnLevelStarted;
    }

    private void OnMoveMade(int remain)
    {
        _moveRemainText.text = "Remains: " + remain;
    }

    private void OnGameStarted()
    {
        _isGameEnded = false;

        SetAlpha(.39f);
        _panel.SetActive(false);
        inLevelText.gameObject.SetActive(true);
        _endLevelText.gameObject.SetActive(false);
    }

    private void OnLevelStarted(int level, bool considerMoveCount, int maxMove)
    {
        _isLevelFailed = false;
        _isLevelCompleted = false;

        if (!considerMoveCount)
        {
            _inGameEventChannel.MoveMadeEvent -= OnMoveMade;
        }
        else
        {
            _inGameEventChannel.MoveMadeEvent += OnMoveMade;
            _moveRemainText.text = "Remains: " + maxMove;
        }

        _currentLevel = level;

        inLevelText.text = $"LEVEL {_currentLevel}";
        _panel.SetActive(false);
        _inLevelTextBackground.gameObject.SetActive(true);
    }

    private void OnGameEnded()
    {
        _isGameEnded = true;

        _endLevelText.text = "End of the Game.";
        _endLevelButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Quit";
        _endLevelText.gameObject.SetActive(true);
        _inLevelTextBackground.gameObject.SetActive(false);

        StartCoroutine(WaitForHolderCompletion(.5f));
    }

    private void OnLevelCompleted(int level)
    {
        _isLevelCompleted = true;

        _moveRemainText.gameObject.SetActive(false);
        _endLevelButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Tap to Continue";
        _endLevelText.text = $"LEVEL {level} COMPLETED!";
        _inLevelTextBackground.gameObject.SetActive(false);
        _endLevelText.gameObject.SetActive(true);

        StartCoroutine(WaitForHolderCompletion(1.2f));
    }

    private void OnLevelFailed(int level)
    {
        _isLevelFailed = true;

        _moveRemainText.gameObject.SetActive(false);
        _inLevelTextBackground.gameObject.SetActive(false);
        _endLevelText.text = $"LEVEL {level} FAILED!";
        _endLevelButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Try Again";
        _endLevelText.gameObject.SetActive(true);

        StartCoroutine(WaitForHolderCompletion(.5f));
    }

    public void OnRestartButtonClicked()
    {
        _inGameEventChannel.RaiseRestartLevelRequestedEvent(_currentLevel);
    }

    public void OnEndLevelButtonClicked()
    {
        if (_isGameEnded)
        {
            Application.Quit();
        }

        if (_isLevelFailed)
        {
            _inGameEventChannel.RaiseRestartLevelRequestedEvent(_currentLevel);
        }

        if (_isLevelCompleted)
        {
            SceneManager.LoadScene($"Level{_currentLevel + 1}");
        }

        _particleEventChannel.RaiseCongratsParticleStopRequestEvent();
    }

    IEnumerator WaitForHolderCompletion(float duration)
    {
        yield return new WaitForSeconds(duration);
        _panel.SetActive(true);
    }

    private void SetAlpha(float alpha)
    {
        Color currentColor = Camera.main.backgroundColor;
        currentColor.a = alpha;
        _panel.GetComponent<Image>().color = currentColor;
    }
}
