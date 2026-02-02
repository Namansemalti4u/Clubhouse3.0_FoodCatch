using Clubhouse.Games.Common;
using UnityEngine;

namespace Clubhouse.Games.Gameplay
{
    public class SpawnManager : MonoBehaviour
    {
        private System.Action spawnEvent;
        private int spawnCount;
        private int maxExCount;

        [SerializeField] private float interval = 2f;
        [SerializeField] private float minInterval = 0.4f;
        [SerializeField] private float speedUpRate = 0.1f;
        [SerializeField] private float spawnDelay = 1f;

        private Timer spawnTimer;
        private Animator animator;

        public void Init(int maxCount, System.Action spawnAction)
        {
            spawnEvent = spawnAction;
            maxExCount = maxCount + 1;
            spawnCount = Random.Range(1, maxExCount);
            spawnTimer = new Timer(interval, PlayThrowAnim);
            StartSpawning();
        }

        private void StartSpawning()
        {
            spawnTimer.ResetTimer();
            spawnTimer.Enable();
        }

        private void PlayThrowAnim()
        {
            animator.Play("Throw");

            if (--spawnCount > 0)
            {
                spawnTimer.ResetTimer();
            }
            else
            {
                spawnTimer.Disable();
                interval = Mathf.Max(minInterval, interval - speedUpRate);
                spawnTimer = new Timer(interval, PlayThrowAnim);
                spawnCount = Random.Range(1, maxExCount);
                Invoke(nameof(StartSpawning), spawnDelay);
            }
        }

        public void OnThrowEvent()
        {
            spawnEvent?.Invoke();
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            spawnTimer?.Update(Time.deltaTime);
        }
    }
}
