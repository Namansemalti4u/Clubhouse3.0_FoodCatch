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
            //public GameConfiguration gameConfiguration;
            public LevelManagerConfiguration levelManagerConfiguration;
        }

        public const string EDGE = "Edge";
        public const string LAND = "Land";
        public const string PERFECT = "Perfect";
        public const string FAST_TAP_BONUS = "FastTapBonus";
        public const string MISS = "Miss";
        private static readonly List<string> ScoreTypes = new() { EDGE, LAND, PERFECT, FAST_TAP_BONUS, MISS };
        #endregion

        #region Fields
        [Header("Game Specific Data")]
        [SerializeField] private Configuration configuration;
        [SerializeField] private LevelManager.Reference levelManagerReference;
        private LevelManager levelManager;
        private VfxManager.VfxPlayParams vfxPlayParams;

        [SerializeField] private Transform vfxPosition;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initializes game components
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            levelManager = new LevelManager(configuration.levelManagerConfiguration.configuration, levelManagerReference);
        }

        public override void StartGame()
        {
            base.StartGame();
            // Custom game start logic
            vfxPlayParams = new VfxManager.VfxPlayParams();
            {
                vfxPlayParams.parent = vfxPosition.transform;
            }
        }
        #endregion

        #region GamePlay Management
        
        #endregion    
    }
}
