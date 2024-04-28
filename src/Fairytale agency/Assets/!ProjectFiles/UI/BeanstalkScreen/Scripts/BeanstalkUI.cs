using TMPro;
using UnityEngine;

namespace UI.BeanstalkScreen
{
    public class BeanstalkUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI built;
        [SerializeField] private TextMeshProUGUI left;

        public void SetLeft(int value) => left.text = $"{value}";
        public void SetBuilt(int value) => built.text = $"{value}";
    }
}