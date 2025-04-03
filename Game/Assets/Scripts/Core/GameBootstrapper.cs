using Services.SoundServices;
using UnityEngine;

namespace Core
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameInitializer _gameInitializer;
        
        private void Awake()
        {
            _gameInitializer = new GameInitializer();
            _gameInitializer.Init();
        }
    }
}