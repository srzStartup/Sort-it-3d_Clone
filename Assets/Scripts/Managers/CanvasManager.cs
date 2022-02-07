using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _inGameLevelText;
    [SerializeField] private TextMeshProUGUI _endLevelText;


}
