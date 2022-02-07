using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI levelCompletedText;

    [SerializeField] private HolderEventSystem _holderEventChannel;
    [SerializeField] private ParticleEventSystem _particleEventChannel;
    [SerializeField] private InGameEventSystem _inGameEventChannel;

    [SerializeField] private Color crossFadeColor;

    private int _currentLevel;
    private bool _isOver;
    private bool _isFailed;

    public override void Awake()
    {
        base.Awake();

        _holderEventChannel.HoldersCompletedInLevel += OnHoldersCompletedInLevel;
        _holderEventChannel.HoldersReady += OnHoldersReady;

        _inGameEventChannel.MoveAuthRemainEvent += OnMoveAuthRemain;
        _inGameEventChannel.OutOfMoveEvent += OnOutOfMove;

        _panel.SetActive(false);
    }

    private void OnOutOfMove()
    {
        _isFailed = true;

        _panel.GetComponent<Image>()
            .color = crossFadeColor;
        levelCompletedText.text = $"LEVEL {_currentLevel} FAILED!";

        levelText.gameObject.SetActive(false);

        StartCoroutine(WaitForHolderCompletion(.5f));
    }

    private void OnMoveAuthRemain(int remain)
    {
        Debug.Log(remain);
    }

    private void OnDestroy()
    {
        _holderEventChannel.HoldersCompletedInLevel -= OnHoldersCompletedInLevel;
        _holderEventChannel.HoldersReady -= OnHoldersReady;
    }

    private void OnHoldersReady(LevelReadyEventArgs e)
    {
        _currentLevel = e.level;
    }

    private void Start()
    {
        _isOver = false;
    }

    private void Update()
    {
        levelText.text = $"LEVEL {_currentLevel}";
    }

    private void OnHoldersCompletedInLevel(int level)
    {
        StartCoroutine(WaitForParticlePlay(.5f));

        if (SceneManager.sceneCountInBuildSettings > level)
        {
            _panel.GetComponent<Image>()
                .color = crossFadeColor;
            levelCompletedText.text = $"LEVEL {level} COMPLETED!";

            levelText.gameObject.SetActive(false);

            StartCoroutine(WaitForHolderCompletion(1.2f));
        }
        else
        {
            _isOver = true;

            _panel.GetComponent<Image>()
                .color = crossFadeColor;
            levelCompletedText.text = $"End of the Game.";

            levelText.gameObject.SetActive(false);

            StartCoroutine(WaitForHolderCompletion(.5f));
        }
    }

    public void OnTapToContinueClicked()
    {
        _particleEventChannel.RaiseCongratsParticleStop();

        if (_isOver)
        {
            Application.Quit();
            return;
        }

        if (_isFailed)
        {
            return;
        }

        _panel.SetActive(false);
        levelText.gameObject.SetActive(true);
        SceneManager.LoadScene($"Level{_currentLevel + 1}");
    }

    IEnumerator WaitForHolderCompletion(float duration)
    {
        yield return new WaitForSeconds(duration);
        _panel.SetActive(true);
    }

    IEnumerator WaitForParticlePlay(float duration)
    {
        yield return new WaitForSeconds(duration);
        _particleEventChannel.RaiseCongratsParticlePlay();
    }
}
