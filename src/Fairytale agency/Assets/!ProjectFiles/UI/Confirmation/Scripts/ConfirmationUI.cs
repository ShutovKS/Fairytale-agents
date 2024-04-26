#region

using UI.Base;
using UnityEngine;

#endregion

namespace UI.Confirmation
{
    public class ConfirmationUI : BaseScreen
    {
        [SerializeField] private Canvas canvas;

        [field: SerializeField] public ConfirmationButtonsUI Buttons { get; private set; }
        [field: SerializeField] public ConfirmationTextUI Text { get; private set; }

        public void SetActivePanel(bool value)
        {
            canvas.enabled = value;
        }
    }
}