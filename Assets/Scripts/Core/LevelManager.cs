using Clubhouse.Games.Utilities;
using Clubhouse.Helper;
using System;
using UnityEngine;

namespace Clubhouse.Games.FoodCatch.Core
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Serializable]
        public struct Configuration
        {
            public GameObject[] foodPrefab;
            public int spawnCount;
            public float edibleRatio;
        }

        [Serializable]
        public struct Reference
        {
            public Transform envParent;
            public Transform poolParent;
        }

        private Configuration configuration;
        private Reference reference;

        private ObjectPoolManager<Food>[] pool;
        private SpawnRateManager spawnManager;

        public Transform spawnPoint, safePoint;

        public void Init(Configuration a_configuration, Reference a_reference)
        {
            configuration = a_configuration;
            reference = a_reference;
            pool = new ObjectPoolManager<Food>[configuration.foodPrefab.Length];
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = new ObjectPoolManager<Food>(configuration.foodPrefab[i].GetComponent<Food>(), reference.poolParent);
            }
            // Create a spawn manager that spawns given number of items over 55 seconds
            spawnManager = new SpawnRateManager(configuration.spawnCount, CreateFood, 55);
            spawnManager.Enable();
        }

        void Start()
        {

        }

        void Update()
        {
            spawnManager.Update(Time.deltaTime);
        }

        void OnDisable()
        {
            spawnManager.Disable();
        }

        private void CreateFood()
        {
            bool spawnEdible = UnityEngine.Random.value <= configuration.edibleRatio;
            int index = (int)(spawnEdible ? Food.FoodType.Edible : Food.FoodType.Inedible);
            var food = pool[index].Get(reference.envParent);
            food.transform.position = spawnPoint.position;
            food.Init();
        }

        public void Despawn(Food food)
        {
            pool[(int)food.foodType].Return(food);
        }
    }
}