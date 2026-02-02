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
    Food.FoodType foodType = spawnEdible ? Food.FoodType.Edible : Food.FoodType.Inedible;

    Food food = pool[(int)foodType].Get(reference.envParent);

    Sprite foodSprite = spawnEdible
        ? config.edibles[Random.Range(0, config.edibles.Length)]
        : config.inedibles[Random.Range(0, config.inedibles.Length)];

    food.Init(spawnPoint.position, foodSprite);

    // Apply visual changes ONLY for inedible food
    if (!spawnEdible && food.transform.childCount > 0)
    {
        Transform firstChild = food.transform.GetChild(0);
        SpriteRenderer sr = firstChild.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = foodSprite;
            sr.color = Color.red;
            firstChild.localScale = new Vector3(1.12f, 1.12f,1.12f);
        }
    }
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