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
        [Space] [SerializeField] private Button _backButton;

        public Action OnBackButtonClicked;

        public void Awake()
        {
            _backButton.RegisterNewCallback(() => OnBackButtonClicked?.Invoke());
        }
    }
}