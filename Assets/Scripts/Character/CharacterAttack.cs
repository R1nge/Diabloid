using System.Collections;
using Shared;
using UnityEngine;

namespace Character
{
    public class CharacterAttack : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private int damage;
        [SerializeField] private float attackInterval;
        [SerializeField] private float attackDistance;
        [SerializeField] private LayerMask ignoreLayer;
        private bool _canAttack = true;
        private CharacterAnimator _characterAnimator;

        private void Awake() => _characterAnimator = GetComponent<CharacterAnimator>();

        public void TryAttack(Ray mousePosition)
        {
            if (!_canAttack) return;
            
            if (Physics.Raycast(mousePosition, out var hit, rayDistance, ~ignoreLayer))
            {
                if (hit.transform.TryGetComponent(out Health health))
                {
                    if (Vector3.Distance(transform.position, hit.point) <= attackDistance)
                    {
                        if (health.IsDead()) return;
                        LookAtTarget(health.transform.position);
                        _characterAnimator.PlayAttackAnimation();
                        health.TakeDamage(damage);
                        StartCoroutine(Attack_c());
                    }
                }
            }
        }
        
        private void LookAtTarget(Vector3 targetPos)
        {
            targetPos.y = transform.position.y;
            StartCoroutine(SmoothLookAt_c(targetPos, Vector3.up, .3f));
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

        private IEnumerator Attack_c()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackInterval);
            _canAttack = true;
        }
    }
}