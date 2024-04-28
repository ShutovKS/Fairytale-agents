using System;
using UnityEngine;
using UnityEngine.UI;

public class FinalUI : MonoBehaviour
{
    public event Action OnExitButtonClicked;

    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
    }
}