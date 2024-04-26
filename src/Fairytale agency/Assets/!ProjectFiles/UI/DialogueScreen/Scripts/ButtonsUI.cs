#region

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.DialogueScreen
{
    public class ButtonsUI : MonoBehaviour
    {
        [SerializeField] private Button _historyButton;
        [SerializeField] private Button _speedUpButton;
        [SerializeField] private Button _autoButton;
        [SerializeField] private Button _furtherButton;

        [Space] [SerializeField] private Button _backButton;

        [Space] [SerializeField] private TextMeshProUGUI _historyButtonText;

        [SerializeField] private TextMeshProUGUI _skipButtonText;
        [SerializeField] private TextMeshProUGUI _autoButtonText;
        [SerializeField] private TextMeshProUGUI _furtherButtonText;

        public Action OnBackButtonClicked;
        public Action OnHistoryButtonClicked;
        public Action OnSpeedUpButtonClicked;
        public Action OnAutoButtonClicked;
        public Action OnFurtherButtonClicked;

        public void Awake()
        {
            _backButton.RegisterNewCallback(() => OnBackButtonClicked?.Invoke());
            _historyButton.RegisterNewCallback(() => OnHistoryButtonClicked?.Invoke());
            _speedUpButton.RegisterNewCallback(() => OnSpeedUpButtonClicked?.Invoke());
            _autoButton.RegisterNewCallback(() => OnAutoButtonClicked?.Invoke());
            _furtherButton.RegisterNewCallback(() => OnFurtherButtonClicked?.Invoke());
        }

        public void SetHistoryButtonText(string text)
        {
            _historyButtonText.SetText(text);
        }

        public void SetSkipButtonText(string text)
        {
            _skipButtonText.SetText(text);
        }

        public void SetAutoButtonText(string text)
        {
            _autoButtonText.SetText(text);
        }

        public void SetFurtherButtonText(string text)
        {
            _furtherButtonText.SetText(text);
        }
    }
}