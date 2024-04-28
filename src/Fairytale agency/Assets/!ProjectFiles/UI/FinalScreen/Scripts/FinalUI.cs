using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.FinalScreen.Scripts
{
    public class FinalUI : MonoBehaviour
    {
        public event Action OnExitButtonClicked;

        [SerializeField] private Button exitButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
        }
    }
}