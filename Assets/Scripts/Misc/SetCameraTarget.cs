using Character;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Misc
{
    public class SetCameraTarget : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        
        [Inject]
        private void Construct(PlayerController player) => virtualCamera.Follow = player.transform;
    }
}