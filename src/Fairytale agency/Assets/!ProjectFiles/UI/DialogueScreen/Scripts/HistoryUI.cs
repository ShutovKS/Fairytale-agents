#region

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.DialogueScreen
{
    public class HistoryUI : MonoBehaviour
    {
        public Action OnBackButtonClicked;

        [SerializeField] private GameObject _historyPhrasePrefab;
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private GameObject _historyGameObject;
        [SerializeField] private Button _backButton;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

        private readonly List<GameObject> _historyPhrases = new();

        public void Awake()
        {
            _backButton.onClick.AddListener(() => OnBackButtonClicked?.Invoke());
        }

        public void SetActivePanel(bool value)
        {
            _historyGameObject.SetActive(value);
        }

        public void CreateHistoryPhrase(string name, string text)
        {
            var historyPhraseInstantiate = Instantiate(_historyPhrasePrefab, _contentTransform);
            historyPhraseInstantiate.SetActive(true);
            _historyPhrases.Add(historyPhraseInstantiate);

            if (historyPhraseInstantiate.TryGetComponent(out HistoryPhraseUI historyPhraseUI))
            {
                historyPhraseUI.NameText.text = name;
                historyPhraseUI.TextText.text = text;
            }
            else
            {
                throw new Exception("No HistoryPhraseUI in instance historyPhrasePrefab");
            }

            var contentPanelRT = _contentTransform.GetComponent<RectTransform>();
            var panel = historyPhraseInstantiate.GetComponent<RectTransform>();

            var scrollSizeDelta = contentPanelRT.sizeDelta;
            scrollSizeDelta.y += panel.sizeDelta.y + _verticalLayoutGroup.spacing;
            contentPanelRT.sizeDelta = scrollSizeDelta;
        }

        public void ClearHistory()
        {
            foreach (var historyPhrase in _historyPhrases)
            {
                Destroy(historyPhrase);
            }

            _historyPhrases.Clear();
        }
    }
}