using Data.Dialogue;

namespace Infrastructure.Services.Dialogue
{
    public interface IDialogueService
    {
        Language CurrentLanguage { get; set; }

        Data.Dialogue.Dialogue GetDialogues(DialogueID id);
        Character GetCharacter(CharacterType characterType);
    }
}