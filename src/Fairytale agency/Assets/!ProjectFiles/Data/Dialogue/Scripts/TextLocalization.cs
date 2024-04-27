using System;
using UnityEngine;

namespace Data.Dialogue
{
    [Serializable]
    public class TextLocalization
    {
        [field: SerializeField] public Language Language { get; private set; }
        [field: SerializeField, TextArea(2,6)] public string Text { get; private set; }
    }
}