using System;
using UnityEngine;
using random = UnityEngine.Random;


namespace Clubhouse.Games.FoodCatch.Core
{
    //using Gameplay;
    public class LevelManager
    {
        [Serializable]
        public struct Configuration
        {
            public GameObject foodPrefab;
            public float minGap, maxGap;
            public Sprite[] foodSprites;
            public PolygonCollider2D[] collider2Ds;
        }

        [Serializable]
        public struct Reference
        {
            public Transform envParent;
            public Transform poolParent;
        }

        private Configuration configuration;
        private Reference reference;

        private readonly ObjectPoolManager<Food> pool;
        private float currentPosition;

        public LevelManager(Configuration a_configuration, Reference a_reference)
        {
            configuration = a_configuration;
            reference = a_reference;

            pool = new ObjectPoolManager<Food>(configuration.foodPrefab.GetComponent<Food>(), reference.poolParent);

            currentPosition = 1f;
            CreateFood(currentPosition);
            for (int i = 1; i <= 3; i++)
            {
                GenerateNextFood();
            }
        }

        public void GenerateNextFood()
        {
            float gap = random.Range(configuration.minGap, configuration.maxGap);
            currentPosition += gap;
            CreateFood(currentPosition);
        }

        public Food CreateFood(float a_position)
        {
            var food = pool.Get(reference.envParent);
            int index = random.Range(0, configuration.foodSprites.Length);
            food.Init(a_position, configuration.foodSprites[index], configuration.collider2Ds[index]);
            return food;
        }

        public void Despawn(Food food)
        {
            pool.Return(food);
        }
    }
}