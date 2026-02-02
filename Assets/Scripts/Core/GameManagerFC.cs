using Clubhouse.Games.Common;
using Clubhouse.Tools.VisualEffects;
using UnityEngine;

namespace Clubhouse.Games.FoodCatch.Core
{
    public class GameManagerFC : Common.GameManager<GameManagerFC>
    {
        #region Data Structures
        [System.Serializable]
        public struct Configuration
        {
            public LevelManagerConfiguration levelManagerConfiguration;
        }

        public const string CAUGHT = "Caught";
        public const string DROP = "Drop";
        public const string WRONG = "Wrong";
        #endregion

        #region Fields
        [Header("Game Specific Data")]
        [SerializeField] private Configuration configuration;
        [SerializeField] private LevelManager.Reference levelManagerReference;
        private VfxManager.VfxPlayParams vfxPlayParams;
        [SerializeField] private Transform VFXPosition;
        #endregion

        #region Unity Methods

        public override void StartGame()
        {
            base.StartGame();
            LevelManager.Instance.Init(configuration.levelManagerConfiguration.configuration, levelManagerReference);
            // Custom game start logic
            vfxPlayParams = new VfxManager.VfxPlayParams();
            {
                vfxPlayParams.parent = VFXPosition;
            }
        }

        public override void GameOver()
        {
            // LevelManager.Instance.gameObject.SetActive(false);
            base.GameOver();
        }
        #endregion

        #region Score Management
        public void AddScore(string scoreType, Transform scoreTransform)
        {
            TextEffectType textType;
            switch (scoreType)
            {
                case CAUGHT:
                    HapticManager.Instance.PlayHaptic(HapticType.OnCorrect);
                    textType = Random.value < 0.5f ? TextEffectType.Nice : TextEffectType.Amazing;
                    break;
                case DROP:
                    HapticManager.Instance.PlayHaptic(HapticType.OnWrong);
                    textType = TextEffectType.Miss;
                    break;
                default:
                    HapticManager.Instance.PlayHaptic(HapticType.OnWrong);
                    textType = TextEffectType.Type2;
                    break;
            }
            VfxManager.Instance.ShowTextEffect(textType, scoreType, 0, vfxPlayParams);
            VfxManager.Instance.ShowScoreEffect(AddScore(scoreType), scoreTransform);
            AudioManager.Instance.Play(scoreType);
        }
        #endregion
    }
}
