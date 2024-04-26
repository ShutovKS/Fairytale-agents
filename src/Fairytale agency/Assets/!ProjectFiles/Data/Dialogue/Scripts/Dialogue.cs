using UnityEngine;

namespace Data.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Data/Dialogue System/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [field: SerializeField] public DialogueID DialogueID { get; private set; }
        [field: SerializeField] public Phrase[] Phrases { get; private set; }
    }
}