using Clubhouse.Tools.VisualEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clubhouse.Games.FoodCatch.Core
{
    public class GameManagerFC : Common.GameManager<GameManagerFC>
    {
        #region Data Structures
        [Serializable]
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
                vfxPlayParams.parent = VFXPosition.transform;
            }
        }
        #endregion
    }
}
