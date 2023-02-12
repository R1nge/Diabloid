using System;
using System.Collections;
using Shared;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float attackRange;
        private PlayerReference _playerReference;
        private EnemyState _currentState;
        private Health _health;

        public event Action<EnemyState> OnStateChangedEvent;

        private void Awake()
        {
            _playerReference = FindObjectOfType<PlayerReference>();
            _health = GetComponent<Health>();
            _health.OnDiedEvent += Die;
        }

        public void ChangeState(EnemyState state)
        {
            _currentState = state;
            OnStateChangedEvent?.Invoke(_currentState);
        }

        private bool IsInAttackRange()
        {
            var playerPos = _playerReference.GetPlayerTransform().position;
            var distance = Vector3.Distance(playerPos, transform.position);
            return distance <= attackRange;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }
        }
        

        private void Chase()
        {
            ChangeState(IsInAttackRange() ? EnemyState.Attack : EnemyState.Chase);
        }

        private void Attack()
        {
            if (_playerReference.GetPlayerTransform().TryGetComponent(out Health health))
            {
                if (health.IsDead())
                {
                    ChangeState(EnemyState.Idle);
                    return;
                }
            }

            ChangeState(IsInAttackRange() ? EnemyState.Attack : EnemyState.Chase);
        }

        private void Die(int health)
        {
            ChangeState(EnemyState.Dead);
            StartCoroutine(Die_c());
        }

        private IEnumerator Die_c()
        {
            yield return new WaitForSeconds(.1f);
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (component is Transform) continue;
                if (component is Animator) continue;
                Destroy(component);
            }
        }

        private void OnDestroy() => _health.OnDiedEvent -= Die;
    }
}