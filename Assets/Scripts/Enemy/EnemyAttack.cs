using System.Collections;
using Shared;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private int damage;
        private EnemyController _enemyController;
        private PlayerReference _playerReference;

        private void Awake()
        {
            _playerReference = FindObjectOfType<PlayerReference>();
            _enemyController = GetComponent<EnemyController>();
            _enemyController.OnStateChangedEvent += OnStateChanged;
        }

        private void OnStateChanged(EnemyState newState)
        {
            if (newState == EnemyState.Attack)
            {
                LookAtPlayer();
            }
        }

        private void LookAtPlayer()
        {
            var playerPos = _playerReference.GetPlayerTransform().position;
            playerPos.y = transform.position.y;
            StartCoroutine(SmoothLookAt_c(playerPos, Vector3.up, .3f));
        }

        IEnumerator SmoothLookAt_c(Vector3 worldPoint, Vector3 upAxis, float duration)
        {
            Quaternion startRot = transform.rotation;
            Quaternion endRot = Quaternion.LookRotation(worldPoint - transform.position, upAxis);
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(startRot, endRot, t / duration);
                yield return null;
            }

            transform.rotation = endRot;
        }

        //animation event
        private void Damage()
        {
            //TODO: check if within attack range or something
            if (_playerReference.GetPlayerTransform().TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }

        private void OnDestroy() => _enemyController.OnStateChangedEvent -= OnStateChanged;
    }
}