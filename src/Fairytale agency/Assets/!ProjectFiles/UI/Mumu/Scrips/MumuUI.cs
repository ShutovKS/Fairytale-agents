using TMPro;
using UnityEngine;

namespace UI.Mumu.Scrips
{
    public class MumuUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthPoints;
        [SerializeField] private TextMeshProUGUI left;
        [SerializeField] private TextMeshProUGUI destroyed;

        public void SetHealthPoints(int value) => healthPoints.text = $"{value}";
        public void SetLeft(int value) => left.text = $"{value}";
        public void SetDestroyed(int value) => destroyed.text = $"{value}";
    }
}