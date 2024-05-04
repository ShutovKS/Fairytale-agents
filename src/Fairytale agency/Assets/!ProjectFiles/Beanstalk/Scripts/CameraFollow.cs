using UnityEngine;

namespace Beanstalk
{
    public class CameraFollow : MonoBehaviour
    {
        [HideInInspector] public Vector3 targetPos;
        private readonly float _smoothMove = 1.0f;

        private void Start()
        {
            targetPos = transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, _smoothMove * Time.deltaTime);
        }
    }
}