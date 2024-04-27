using System;
using System.Collections.Generic;
using Data.Dialogue;
using UnityEngine;

namespace Infrastructure.Services.Dialogue
{
    public class DialogueService : IDialogueService
    {
        public DialogueService()
        {
            var dialogues = Resources.LoadAll<Data.Dialogue.Dialogue>("Dialogues");
            foreach (var dialogue in dialogues)
            {
                _dialogues.Add(dialogue.DialogueID, dialogue);
            }

            var characters = Resources.LoadAll<Character>("Characters");
            foreach (var character in characters)
            {
                _characters.Add(character.CharacterType, character);
            }
        }

        public Language CurrentLanguage { get; set; }
        private readonly Dictionary<DialogueID, Data.Dialogue.Dialogue> _dialogues = new();
        private readonly Dictionary<CharacterType, Character> _characters = new();

        public Data.Dialogue.Dialogue GetDialogues(DialogueID id)
        {
            if (!_dialogues.TryGetValue(id, out var dialogue))
            {
                throw new Exception($"Нет диалога с ID: {id.ToString()}");
            }

            return dialogue;
        }

        public Character GetCharacter(CharacterType characterType)
        {
            if (!_characters.TryGetValue(characterType, out var character))
            {
                throw new Exception($"Нет акого персонажа с ID: {characterType.ToString()}");
            }

            return character;
        }
    }
}