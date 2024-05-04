using System;
using UnityEngine;

namespace Data.Dialogue
{
    [Serializable]
    public class Phrase
    {
        [field: SerializeField] public CharacterType CharacterType { get; private set; }
        [field: SerializeField] public TextLocalization[] TextLocalization { get; private set; }
        [field: SerializeField] public Sprite Background { get; private set; }
        [field: SerializeField] public Event Event { get; private set; }
    }
}