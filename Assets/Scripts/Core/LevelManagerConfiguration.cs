using UnityEngine;

namespace Clubhouse.Games.FoodCatch.Core
{
    [CreateAssetMenu(fileName = "LevelManagerConfiguration", menuName = "Games/FoodCatch/LevelManagerConfiguration")]
    public class LevelManagerConfiguration : ScriptableObject
    {
        public LevelManager.Configuration configuration;
    }
}