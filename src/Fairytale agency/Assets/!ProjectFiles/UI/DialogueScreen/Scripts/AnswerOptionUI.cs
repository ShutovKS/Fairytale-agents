#region

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.DialogueScreen
{
    public class AnswerOptionUI : MonoBehaviour
    {
        [SerializeField] private Button _answerButton;
        [SerializeField] private TextMeshProUGUI _answerText;

        public void SetAnswerOption(string text, Action action)
        {
            _answerText.text = text;
            _answerButton.RegisterNewCallback(action);
        }
    }
}