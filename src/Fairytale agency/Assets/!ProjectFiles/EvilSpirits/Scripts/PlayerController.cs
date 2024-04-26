using UnityEngine;
using UnityEngine.Serialization;

namespace EvilSpirits
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 6.0f;
        [SerializeField] private float jumpSpeed = 8.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float mouseSensitivity = 5.0f;
        [SerializeField] private float mouseSmoothing = 2.0f;
        [SerializeField] private float health = 100f;
        [SerializeField] private bool isFiring;
        [SerializeField] private CharacterController controller;
        
        private Vector3 _moveDirection = Vector3.zero;
        private Vector2 _mouseLook;
        private Vector2 _inputMouseAxis = new Vector3(0.0f, 0.0f);
        private Vector2 _smoothV = new Vector3(0.0f, 0.0f, 0.0f);
        private float _timer = 0.0f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void TakeDamage(float value)
        {
            health -= value;
        }

        private void FixedUpdate()
        {
            _inputMouseAxis.Set(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            _inputMouseAxis = Vector2.Scale(_inputMouseAxis, new Vector2(mouseSensitivity * mouseSmoothing,
                mouseSensitivity * mouseSmoothing));

            _smoothV.x = Mathf.Lerp(_smoothV.x, _inputMouseAxis.x, 1f / mouseSmoothing);
            _smoothV.y = Mathf.Lerp(_smoothV.y, _inputMouseAxis.y, 1f / mouseSmoothing);
            _mouseLook += _smoothV;
            _mouseLook.y = Mathf.Clamp(_mouseLook.y, -90.0f, 90.0f);

            transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.up);
            controller.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, controller.transform.up);

            if (controller.isGrounded)
            {
                _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                _moveDirection = transform.TransformDirection(_moveDirection);
                _moveDirection *= speed;

                if (Input.GetButton("Jump"))
                {
                    _moveDirection.y = jumpSpeed;
                }
            }

            _moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(_moveDirection * Time.deltaTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown("escape"))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}