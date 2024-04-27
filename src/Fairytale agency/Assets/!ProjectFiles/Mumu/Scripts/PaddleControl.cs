using System.Collections;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Mumu
{
    public class PaddleControl : MonoBehaviour
    {
        [SerializeField] private PlayerInputActionReader _inputActionReader;
        [SerializeField] private Transform paddleTransform;
        [SerializeField] private Collider2D paddleCollider;
        [SerializeField] private float delayAttack;
        [SerializeField] private Renderer renderer;

        private bool _isAttack;

        private void Update()
        {
            AttackHandler();
        }

        private void AttackHandler()
        {
            if (_isAttack)
            {
                return;
            }

            var vector2 = _inputActionReader.MovementValue;
            switch (vector2.x, vector2.y)
            {
                case (0, 1):
                    StartCoroutine(Attack(0));
                    break;
                case (-1, 0):
                    StartCoroutine(Attack(90));
                    break;
                case (0, -1):
                    StartCoroutine(Attack(180));
                    break;
                case (1, 0):
                    StartCoroutine(Attack(270));
                    break;
            }
        }

        private IEnumerator Attack(float rotation)
        {
            _isAttack = true;
            paddleTransform.rotation = Quaternion.Euler(0, 0, rotation);
            paddleCollider.enabled = true;
            renderer.enabled = true;

            for (var i = 0; i < 20; i++)
            {
                yield return null;
            }

            paddleCollider.enabled = false;
            renderer.enabled = false;

            yield return new WaitForSeconds(delayAttack);
            _isAttack = false;
        }
    }
}