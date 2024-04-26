#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.DialogueScreen
{
    public class PersonAvatarUI : MonoBehaviour
    {
        [SerializeField] private GameObject _avatarGO;
        [SerializeField] private Image _avatarImage;

        public void SetActionAvatar(bool value)
        {
            _avatarGO.SetActive(value);
        }

        public void SetAvatar(Sprite sprite)
        {
            _avatarImage.sprite = sprite;
        }
    }
}