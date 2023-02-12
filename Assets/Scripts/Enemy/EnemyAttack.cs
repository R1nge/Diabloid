using System.Collections;
using Shared;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private int damage;
        private bool _canAttack = true;
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
                Attack();
            }
        }

        private void Attack()
        {
            if (!_canAttack) return;
            StartCoroutine(Attack_c());
            LookAtPlayer();
        }

        private void LookAtPlayer()
        {
            var playerPos = _playerReference.GetPlayerTransform().position;
            playerPos.y = transform.position.y;
            var rotationAngle = Quaternion.LookRotation(playerPos - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationAngle, Time.deltaTime * 25);
        }

        private IEnumerator Attack_c()
        {
            _canAttack = false;
            yield return new WaitForSeconds(5);
            _canAttack = true;
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