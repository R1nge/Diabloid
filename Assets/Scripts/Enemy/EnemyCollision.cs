using Character;
using UnityEngine;

namespace Enemy
{
    public class EnemyCollision : MonoBehaviour
    {
        private EnemyController _enemyController;

        private void Awake() => _enemyController = GetComponentInParent<EnemyController>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _))
            {
                _enemyController.ChangeState(EnemyState.Chase);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _))
            {
                _enemyController.ChangeState(EnemyState.Patrol);
            }
        }
    }
}