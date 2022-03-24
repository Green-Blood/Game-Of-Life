using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameOfLifeGenerator gameOfLifeGenerator;

    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;

    [SerializeField] private TMP_InputField xField;
    [SerializeField] private TMP_InputField yField;
    [SerializeField] private TMP_InputField zField;
    [SerializeField] private TMP_InputField changeSpeedField;

    [SerializeField] private GameObject uiPanel;

    private void Awake()
    {
        startButton.onClick.AddListener(StartClick);
        stopButton.onClick.AddListener(StopClick);
    }

    private void StartClick()
    {
        int.TryParse(xField.text, out int xText);
        int.TryParse(yField.text, out int yText);
        int.TryParse(zField.text, out int zText);
        float.TryParse(changeSpeedField.text, out float changeSpeed);

        gameOfLifeGenerator.OnStart(new Vector3(xText, yText, zText), changeSpeed);
        uiPanel.gameObject.SetActive(false);
    }

    private void StopClick()
    {
        gameOfLifeGenerator.OnStop();
        uiPanel.gameObject.SetActive(true);
    }
}