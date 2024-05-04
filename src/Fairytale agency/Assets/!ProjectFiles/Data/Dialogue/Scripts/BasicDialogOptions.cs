using UnityEngine;

namespace Data.Dialogue
{
    [CreateAssetMenu(fileName = "BasicDialogOptions", menuName = "Data/Dialogue System/BasicDialogOptions", order = 0)]
    public class BasicDialogOptions : ScriptableObject
    {
        [field: SerializeField] public float secondsDelayDefault = 0.05f;
        [field: SerializeField] public float delayAfterDialogueEnds = 3f;
    }
}