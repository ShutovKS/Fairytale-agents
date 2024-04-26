using UnityEngine;

namespace Data.Dialogue
{
    [CreateAssetMenu(fileName = "Character", menuName = "Data/Dialogue System/Character", order = 0)]
    public class Character : ScriptableObject
    {
        [field: SerializeField] public CharacterType CharacterType { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Avatar { get; private set; }
    }
}