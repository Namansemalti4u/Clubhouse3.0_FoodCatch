using Clubhouse.Games.Utilities;
using Clubhouse.Helper;
using System;
using UnityEngine;
using random = UnityEngine.Random;

namespace Clubhouse.Games.FoodCatch.Core
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Serializable]
        public struct Configuration
        {
            public GameObject[] foodPrefab;
            public float minTimeInterval, maxTimeInterval;
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

        [SerializeField] private Transform spawnPoint;

        public void Init(Configuration a_configuration, Reference a_reference)
        {
            configuration = a_configuration;
            reference = a_reference;
            pool = new ObjectPoolManager<Food>[configuration.foodPrefab.Length];
            for (int i = 0; i < configuration.foodPrefab.Length; i++)
            {
                pool[i] = new ObjectPoolManager<Food>(configuration.foodPrefab[i].GetComponent<Food>(), reference.poolParent, 5);
            }
            // Create a spawn manager that spawns 12 items over 60 seconds
            spawnManager = new SpawnRateManager(12, CreateFood, 60);
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
            var food = pool[0].Get(reference.envParent);
            food.transform.position = spawnPoint.position;
            food.Init();
        }

        public void Despawn(Food food)
        {
            pool[0].Return(food);
        }
    }
}