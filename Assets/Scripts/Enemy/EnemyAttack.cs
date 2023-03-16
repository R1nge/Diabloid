﻿using System.Collections;
using Character;
using Shared;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

namespace Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private int damage;
        private EnemyController _enemyController;
        private PlayerController _playerController;
        
        [Inject]
        private void Construct(PlayerController player) => _playerController = player;

        private void Awake()
        {
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
            var playerPos = _playerController.transform.position;
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
            if (_playerController.TryGetComponent(out Health health))
            {
                health.TakeDamage(damage);
            }
        }

        private void OnDestroy() => _enemyController.OnStateChangedEvent -= OnStateChanged;
    }
}