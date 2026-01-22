using Clubhouse.Games.Common;
using UnityEngine;

namespace Clubhouse.Games.Gameplay
{
    public class SpawnManager : MonoBehaviour
    {
        private System.Action OnComplete;
        [SerializeField] private int spawnCount, maxExCount;
        [SerializeField] private float interval = 0.5f;
        private Timer spawnTimer;

        public void Init(int maxCount, System.Action onComplete)
        {
            OnComplete = onComplete;
            maxExCount = maxCount + 1;
            spawnCount = Random.Range(1, maxExCount);
            spawnTimer = new Timer(interval, OnTimerComplete);
            StartSpawning();
        }

        private void StartSpawning()
        {
            spawnTimer.ResetTimer();
            spawnTimer.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            spawnTimer?.Update(Time.deltaTime);
        }

        public void OnTimerComplete()
        {
            if (--spawnCount > 0)
                spawnTimer.ResetTimer();
            else
            {
                spawnTimer.Disable();
                spawnCount = Random.Range(1, maxExCount);
                Invoke(nameof(StartSpawning), 5);
            }
            OnComplete?.Invoke();
        }
    }
}
