using System;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenuScreen
{
    public class MainMenuScreen : BaseScreen
    {
        public event Action OnStartNewGameButtonClicked;
        public event Action OnLoadGameButtonClicked;
        public event Action OnExitButtonClicked;
        
        [SerializeField] private Button _startNewGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _exitButton;
        
        private new void Awake()
        {
            _startNewGameButton.onClick.AddListener(() => OnStartNewGameButtonClicked?.Invoke());
            _loadGameButton.onClick.AddListener(() => OnLoadGameButtonClicked?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitButtonClicked?.Invoke());
            
            base.Awake();
        }
        
        private new void OnDestroy()
        {
            _startNewGameButton.onClick.RemoveListener(() => OnStartNewGameButtonClicked?.Invoke());
            _loadGameButton.onClick.RemoveListener(() => OnLoadGameButtonClicked?.Invoke());
            _exitButton.onClick.RemoveListener(() => OnExitButtonClicked?.Invoke());
            
            base.OnDestroy();
        }
    }
}
