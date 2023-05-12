using Character;
using UnityEngine;

namespace Enemy
{
    public class EnemyCollision : MonoBehaviour
    {
        private EnemyState _enemyState;

        private void Awake() => _enemyState = GetComponentInParent<EnemyState>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _))
            {
                _enemyState.ChangeState(EnemyStates.Chase);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _))
            {
                _enemyState.ChangeState(EnemyStates.Patrol);
            }
        }
    }
}