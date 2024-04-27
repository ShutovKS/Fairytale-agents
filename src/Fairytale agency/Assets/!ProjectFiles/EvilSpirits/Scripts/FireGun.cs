using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EvilSpirits
{
    public class FireGun : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject gun1ProjectilePrefab;
        [SerializeField] private Transform projectileSpawn;
        [SerializeField] private float delayBetweenShots = 1f;

        private bool _isYouCanShoot = true;
        private static readonly int isFiring = Animator.StringToHash("isFiring");

        public void Shot()
        {
            if (_isYouCanShoot)
            {
                _isYouCanShoot = false;
                animator.SetBool(isFiring, true);
                Instantiate(gun1ProjectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
                StartCoroutine(Timer());
            }
            else
            {
                animator.SetBool(isFiring, false);
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(delayBetweenShots);
            _isYouCanShoot = true;
        }
    }
}