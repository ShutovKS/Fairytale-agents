using System;
using System.Collections;
using UnityEngine;

namespace Mumu
{
    public class BoatMovement : MonoBehaviour
    {
        public event Action OnCompleted;

        [SerializeField] private Transform targetTransform;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float stoppingDistance = 0.1f;

        private void Start()
        {
            StartMovingToTarget();
        }

        private void StartMovingToTarget()
        {
            StartCoroutine(MoveToTarget());
        }

        private IEnumerator MoveToTarget()
        {
            while (Vector3.Distance(transform.position, targetTransform.position) > stoppingDistance)
            {
                var direction = (targetTransform.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                yield return null;
            }

            OnCompleted?.Invoke();
        }
    }
}