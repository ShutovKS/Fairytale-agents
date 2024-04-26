using UnityEngine;
using UnityEngine.Serialization;

namespace EvilSpirits
{
    public class FireGun : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject gun1ProjectilePrefab;
        [SerializeField] private Transform projectileSpawn;

        private bool _hasFired;
        private float _timer;
        private static readonly int isFiring = Animator.StringToHash("isFiring");
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if (_timer >= 1.0f || !_hasFired)
                {
                    animator.SetBool(isFiring, true);
                    Instantiate(gun1ProjectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
                    _hasFired = true;
                    _timer = 0;
                }
                else
                {
                    animator.SetBool(isFiring, false);
                }
            }

            _timer += Time.deltaTime;
        }
    }
}