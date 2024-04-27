using UnityEngine;

namespace Mumu
{
    public class TurnTowardsTarget : MonoBehaviour
    {
        public Transform targetTransform;

        [SerializeField] private float rotationOffset;

        private void Update()
        {
            Vector2 direction = targetTransform.position - transform.position;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        }
    }
}