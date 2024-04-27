using System;
using Infrastructure.Services.Input;
using UnityEngine;

namespace EvilSpirits
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float speedMove = 6.0f;
        [SerializeField] private float speedRotate = 5.0f;
        [SerializeField] private float rotateSmoothing = 2.0f;

        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerInputActionReader inputActionReader;
        [SerializeField] private FireGun fireGun;

        private Vector2 _mouseLook;
        private Vector2 _smoothV;
        private bool _isFiring;

        public void TakeDamage(float value)
        {
            health -= value;
        }

        private void FixedUpdate()
        {
            Rotate();
            Move();
        }

        private void Move()
        {
            Vector3 moveDirection = new Vector3(0, 0, inputActionReader.MovementValue.y);
            if (controller.isGrounded)
            {
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speedMove;
            }

            moveDirection.y -= gravity * Time.fixedDeltaTime;
            controller.Move(moveDirection * Time.fixedDeltaTime);
        }

        private void Rotate()
        {
            Vector2 inputMouseAxis = Vector2.Scale(inputActionReader.MovementValue, new Vector2(
                speedRotate * rotateSmoothing, speedRotate * rotateSmoothing));

            _smoothV.x = Mathf.Lerp(_smoothV.x, inputMouseAxis.x, 1f / rotateSmoothing);
            _mouseLook += _smoothV;

            controller.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, controller.transform.up);
        }

        private void OnEnable()
        {
            inputActionReader.Jump += fireGun.Shot;
        }

        private void OnDisable()
        {
            inputActionReader.Jump -= fireGun.Shot;
        }
    }
}