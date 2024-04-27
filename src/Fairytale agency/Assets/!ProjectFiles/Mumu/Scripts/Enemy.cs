using System;
using System.Collections;
using UnityEngine;

namespace Mumu
{
    public class Enemy : MonoBehaviour
    {
        public Action OnDead;
        public Transform targetTransform;

        [SerializeField] private float frequencyMoving = 1f;
        [SerializeField] private float travelDistance = 1f;
        [SerializeField] private float damage;

        private void Start()
        {
            StartCoroutine(MoveTowardsTarget());
        }

        private IEnumerator MoveTowardsTarget()
        {
            while (true)
            {
                var direction = (targetTransform.position - transform.position).normalized;
                transform.position += direction * travelDistance;

                if ((targetTransform.position - transform.position).magnitude <= 1.5)
                {
                    StartCoroutine(Attack());
                    yield break;
                }

                yield return new WaitForSeconds(frequencyMoving);
            }
        }

        private IEnumerator Attack()
        {
            while (true)
            {
                var direction = (targetTransform.position - transform.position).normalized;
                transform.position += direction * travelDistance;

                for (var i = 0; i < 10; i++)
                {
                    yield return null;
                }

                transform.position -= direction * travelDistance;

                yield return new WaitForSeconds(frequencyMoving);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PaddleControl>(out var paddleControl))
            {
                OnDead?.Invoke();
                Destroy(gameObject);
            }

            if (other.TryGetComponent<Boat>(out var boat))
            {
                boat.TakeDamage(damage);
            }
        }
    }
}