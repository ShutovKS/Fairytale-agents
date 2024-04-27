using UnityEngine;

namespace EvilSpirits
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform projectileSpawn;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float health = 100.0f;

        [SerializeField] private WaypointPatrol waypointPatrol;
        [SerializeField] private Observer observer;
        [SerializeField] private PlayerChase playerChase;

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
            observer.OnPlayerSpotted += () =>
            {
                _hasSpottedPlayer = true;
                waypointPatrol.enabled = false;
                playerChase.enabled = true;
                playerChase.SetPlayer(_playerTransform);
            };
        }

        public void TakeDamage(float value)
        {
            health -= value;

            if (health <= 0)
            {
                animator.SetBool(isPursuing, false);
                animator.SetBool(isDead, true);
                GetComponent<CapsuleCollider>().enabled = false;
                playerChase.enabled = false;
                transform.position = new Vector3(transform.position.x, 0.075f, transform.position.z);
                transform.Rotate(-90, 0, 0);
                GetComponent<Collider>().enabled = false;
                enabled = false;
            }
        }

        private void Update()
        {
            if (animator.GetBool(isDead) || health <= 0)
            {
                return;
            }


            if (_hasSpottedPlayer)
            {
                if (!(Vector3.Distance(transform.position, _playerTransform.position) <= 1f))
                {
                    playerChase.enabled = !animator.GetBool(isAttacking);
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