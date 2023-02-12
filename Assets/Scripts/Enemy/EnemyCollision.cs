using Shared;
using UnityEngine;

namespace Enemy
{
    public class EnemyCollision : MonoBehaviour
    {
        private PlayerReference _playerReference;
        private EnemyController _enemyController;

        private void Awake()
        {
            _playerReference = FindObjectOfType<PlayerReference>();
            _enemyController = GetComponentInParent<EnemyController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.Equals(_playerReference.GetPlayerTransform()))
            {
                _enemyController.ChangeState(EnemyState.Chase);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.Equals(_playerReference.GetPlayerTransform()))
            {
                _enemyController.ChangeState(EnemyState.Idle);
            }
        }
    }
}