using UnityEngine;
using Zenject;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Character.PlayerController player;
        [SerializeField] private Transform spawnPoint;

        public override void InstallBindings()
        {
            var playerInstance = Container.InstantiatePrefabForComponent<Character.PlayerController>(player);
            playerInstance.transform.position = spawnPoint.position;
            Container.Bind<Character.PlayerController>().FromInstance(playerInstance).AsSingle();
            Container.QueueForInject(player);
        }
    }
}