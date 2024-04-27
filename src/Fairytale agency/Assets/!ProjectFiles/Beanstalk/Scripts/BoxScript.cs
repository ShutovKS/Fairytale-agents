using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Beanstalk
{
    public class BoxScript : MonoBehaviour
    {
        public Action OnLanded;
        public Action OnGameOver;

        private float _minX = -2.2f, _maxX = 2.2f;
        private bool _canMove;
        private float _moveSpeed = 2f;
        private Rigidbody2D _myBody;
        private bool _ignoreCollision;

        private void Awake()
        {
            _myBody = GetComponent<Rigidbody2D>();
            _myBody.gravityScale = 0.0f;
        }

        private void Start()
        {
            _canMove = true;

            if (Random.Range(0, 2) > 0)
            {
                _moveSpeed *= -1.0f;
            }
        }

        private void Update()
        {
            MoveBox();
        }

        private void MoveBox()
        {
            if (_canMove)
            {
                var temp = transform.position;

                temp.x += _moveSpeed * Time.deltaTime;

                if (temp.x > _maxX)
                {
                    _moveSpeed *= -1.0f;
                }
                else if (temp.x < _minX)
                {
                    _moveSpeed *= -1.0f;
                }

                transform.position = temp;
            }
        }

        public void DropBox()
        {
            _canMove = false;
            _myBody.gravityScale = Random.Range(2, 4);
        }

        private void OnCollisionEnter2D(Collision2D target)
        {
            if (_ignoreCollision)
            {
                return;
            }

            if (target.gameObject.CompareTag($"Platform") || target.gameObject.CompareTag($"Box"))
            {
                _ignoreCollision = true;
                OnLanded?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D target)
        {
            if (target.gameObject.CompareTag($"GameOver"))
            {
                OnGameOver?.Invoke();
            }
        }
    }
}