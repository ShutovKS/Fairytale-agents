#region

using UI.Base;
using UnityEngine;

#endregion

namespace UI.DialogueScreen
{
    public class DialogueUI : BaseScreen
    {
        [SerializeField] private Canvas canvas;

        [field: SerializeField] public AnswerOptionsUI Answers { get; private set; }
        [field: SerializeField] public PersonAvatarUI Person { get; private set; }
        [field: SerializeField] public DialogueTextUI DialogueText { get; private set; }
        [field: SerializeField] public ButtonsUI Buttons { get; private set; }
        [field: SerializeField] public HistoryUI History { get; private set; }

        public void SetActivePanel(bool value)
        {
            canvas.enabled = value;
        }
    }
}