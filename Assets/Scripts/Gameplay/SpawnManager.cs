using Clubhouse.Games.Common;
using UnityEditor.Animations;
using UnityEngine;

namespace Clubhouse.Games.Gameplay
{
    public class SpawnManager : MonoBehaviour
    {
        private System.Action spawnEvent;
        private int spawnCount, maxExCount;
        [SerializeField] private float interval, spawnDelay;
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

        void PlayThrowAnim()
        {
            animator.Play("Throw");
            if (--spawnCount > 0)
                spawnTimer.ResetTimer();
            else
            {
                spawnTimer.Disable();
                spawnCount = Random.Range(1, maxExCount);
                Invoke(nameof(StartSpawning), spawnDelay);
            }
        }

        public void OnThrowEvent()
        {
            spawnEvent?.Invoke();
        }

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            spawnTimer?.Update(Time.deltaTime);
        }
    }
}
