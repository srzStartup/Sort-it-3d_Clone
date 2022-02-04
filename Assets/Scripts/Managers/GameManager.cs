using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI levelCompletedText;

    [SerializeField] private Color crossFadeColor;

    private int _level;
    private float _popupHeight;
    private Holder _activeHolder = null;
    private List<int> _completedHolderIndexes;
    private int _holdersNeedToBeCompleted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);

        EventManager.HoldersReady += OnHoldersReady;
        EventManager.HolderClicked += OnHolderClicked;
        EventManager.HolderCompleted += OnHolderCompleted;

        _panel.SetActive(false);
    }

    private void Update()
    {
        levelText.text = $"LEVEL {_level}";
    }

    private void OnHolderClicked(object sender, Holder holder)
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
                if (!holder.hasAvailableSlot) return;

                Transform selectedBall = _activeHolder.Pop();
                holder.Add(selectedBall);

                _activeHolder = null;
            }
        }
    }

    private void OnHolderCompleted(object sender, Holder holder)
    {
        _completedHolderIndexes.Add(holder.order);

        if (_holdersNeedToBeCompleted == _completedHolderIndexes.Count)
            //LevelCompleted?.Invoke(this, _level);
            OnLevelCompleted(_level);
    }

    private void OnHoldersReady(object sender, HoldersReadyEventArgs eventArgs)
    {
        _popupHeight = eventArgs.popupHeight;
        _completedHolderIndexes = new List<int>();
        _level = eventArgs.level;
        _holdersNeedToBeCompleted = eventArgs.holdersNeedToBeCompleted;
    }

    private void OnLevelCompleted(int level)
    {
        if (SceneManager.sceneCountInBuildSettings > level)
        {
            _panel.GetComponent<Image>()
                .color = crossFadeColor;
            levelCompletedText.text = $"LEVEL {level} COMPLETED!";

            levelText.gameObject.SetActive(false);

            _panel.SetActive(true);
        }
        else
        {
            Debug.Log("End of the game.");
        }
    }

    public void OnTapToContinueClicked()
    {
        _panel.SetActive(false);
        levelText.gameObject.SetActive(true);
        SceneManager.LoadScene(_level + 1);
    }
}
