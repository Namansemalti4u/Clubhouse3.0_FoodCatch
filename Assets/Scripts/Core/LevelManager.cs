using Clubhouse.Games.Gameplay;
using Clubhouse.Games.Utilities;
using Clubhouse.Helper;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
            public Sprite[] edibles, inedibles;
        }

        [Serializable]
        public struct Reference
        {
            public Transform envParent;
            public Transform poolParent;
        }

        private Configuration config;
        private Reference reference;

        private ObjectPoolManager<Food>[] pool;

        public Transform safePoint;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private SpawnManager spawnManager;

        public void Init(Configuration configuration, Reference a_ref)
        {
            config = configuration;
            reference = a_ref;
            pool = new ObjectPoolManager<Food>[config.foodPrefab.Length];
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = new ObjectPoolManager<Food>(config.foodPrefab[i].GetComponent<Food>(), reference.poolParent);
            }

            spawnManager.Init(config.spawnCount, CreateFood);
        }

        public void CreateFood()
        {
            bool spawnEdible = Random.value <= config.edibleRatio;
            int index = (int)(spawnEdible ? Food.FoodType.Edible : Food.FoodType.Inedible);
            var food = pool[index].Get(reference.envParent);
            var foodSprite = spawnEdible ? config.edibles[Random.Range(0, config.edibles.Length)] : config.inedibles[Random.Range(0, config.inedibles.Length)];
            food.Init(spawnPoint.position, foodSprite);
        }

        public void Despawn(Food food)
        {
            pool[(int)food.foodType].Return(food);
        }

        private void OnDisable()
        {
            if (spawnManager != null)
                spawnManager.enabled = false;
        }
    }
}