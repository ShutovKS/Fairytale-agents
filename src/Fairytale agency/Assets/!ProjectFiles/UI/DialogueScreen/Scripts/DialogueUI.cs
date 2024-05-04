#region

using System;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.DialogueScreen
{
    public class DialogueUI : BaseScreen
    {
        public Action OnBackButtonClicked;

        [SerializeField] private Image imageBackground;
        [SerializeField] private Image avatarImage;
        [SerializeField] private TextMeshProUGUI authorNameText;
        [SerializeField] private TextMeshProUGUI textText;
        [SerializeField] private Button backButton;

        protected override void Awake()
        {
            backButton.RegisterNewCallback(() => OnBackButtonClicked?.Invoke());
            base.Awake();
        }

        public void SetAuthorName(string authorName)
        {
            authorNameText.text = authorName;
        }

        public void SetText(string text)
        {
            textText.text = text;
        }

        public void SetAvatar(Sprite sprite)
        {
            avatarImage.sprite = sprite;
        }

        public void SetImage(Sprite sprite)
        {
            imageBackground.sprite = sprite;
        }
    }
}