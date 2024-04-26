using System;
using UnityEngine;

namespace EvilSpirits
{
    public class ImpController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform projectileSpawn;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float health = 100.0f;

        private GameObject _collidingObject;
        private Transform _playerTransform;

        private Vector3 _targetDirection;
        private bool _hasSpottedPlayer;
        private float _angleToPlayer;
        private float _attackTimer;

        private static readonly int isPursuing = Animator.StringToHash("isPursuing");
        private static readonly int isAttacking = Animator.StringToHash("isAttacking");
        private static readonly int isDead = Animator.StringToHash("isDead");

        private void Start()
        {
            _playerTransform = FindAnyObjectByType<PlayerController>().transform;
        }

        public void TakeDamage(float value)
        {
            health -= value;

            if (health <= 0)
            {
                animator.SetBool(isPursuing, false);
                animator.SetBool(isDead, true);
                GetComponent<CapsuleCollider>().enabled = false;
                controller.enabled = false;
            }
        }

        private void Update()
        {
            transform.LookAt(new Vector3(_playerTransform.position.x, 0.0f, _playerTransform.position.z));
            _targetDirection = _playerTransform.position - transform.position;
            _angleToPlayer = (Vector3.Angle(_targetDirection, transform.forward));

            if (animator.GetBool(isDead) || health <= 0)
            {
                return;
            }

            //transform.LookAt(transform.position + playerPosition.transform.rotation * Vector3.forward, 
            //                 playerPosition.transform.rotation * Vector3.up);

            //If the player has been spotted begin pursuit
            if (_angleToPlayer is >= -90 and <= 90 && !_hasSpottedPlayer)
            {
                animator.SetBool(isPursuing, true);
                _hasSpottedPlayer = true;
            }

            if (_hasSpottedPlayer)
            {
                //If the player is less than a certain distance, stop pursuit.
                if (Vector3.Distance(transform.position, _playerTransform.position) <= 1f)
                {
                    animator.SetBool(isPursuing, false);
                }
                else
                {
                    animator.SetBool(isPursuing, true);

                    if (!animator.GetBool(isAttacking))
                    {
                        controller.Move(transform.forward * 1.0f * Time.deltaTime);
                        //transform.position += transform.forward * 1.0f * Time.deltaTime;
                    }
                }

                if ((Vector3.Distance(transform.position, _playerTransform.position) <= 20f) &&
                    (_attackTimer >= 5f))
                {
                    animator.SetBool(isPursuing, false);
                    animator.SetBool(isAttacking, true);

                    //StartCoroutine(WaitOnAttack());
                }
                else
                {
                    animator.SetBool(isAttacking, false);
                }
            }

            _attackTimer += Time.deltaTime;
        }

        private void Attack()
        {
            Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            animator.SetBool(isAttacking, false);
            _attackTimer = 0f;
        }
    }
}