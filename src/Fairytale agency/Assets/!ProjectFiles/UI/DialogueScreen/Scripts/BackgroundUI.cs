using UnityEngine;
using UnityEngine.UI;

namespace UI.DialogueScreen
{
    public class BackgroundUI : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}