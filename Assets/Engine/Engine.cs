using MVCEngine.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MVCEngine
{
    public class Engine
    {
        private static Engine _instance;

        public static Engine Instance
        {
            get
            {
                if (Engine._instance == null)
                {
                    Engine._instance = new Engine();
                }

                return Engine._instance;
            }
        }

        private List<ISingletonData> _singletonDataModels;
        private SaveGame _saveGame;
        
        public TimeController TimeController { get; private set; }
        public InputController InputController { get; private set; }
        public PostProcessingController PostProcessingController { get; private set; }
        public GraphicsQualityController GraphicsQualityController { get; private set; }
        public GameFlowController GameFlowController { get; private set; }

        private Engine()
        {
            // Initialize singleton data models
            _singletonDataModels = new List<ISingletonData>();

            CustomTime time = new CustomTime();
            _singletonDataModels.Add(time);

            Performance performance = new Performance();
            _singletonDataModels.Add(performance);

            _saveGame = new SaveGame();
            _saveGame.LoadFromJSON("save_0");

            // Initialize controllers
            var engineContainer = new GameObject { name = "[Core Engine]" };
            Transform engineParentTransform = engineContainer.transform;
            Object.DontDestroyOnLoad(engineContainer);
            TimeController = Object.Instantiate(Resources.Load<TimeController>("Controller Prefabs/Time Controller"), engineParentTransform);
            InputController = Object.Instantiate(Resources.Load<InputController>("Controller Prefabs/Input Controller"), engineParentTransform);
            PostProcessingController = Object.Instantiate(Resources.Load<PostProcessingController>("Controller Prefabs/Post Processing Controller"), engineParentTransform);
            GraphicsQualityController = Object.Instantiate(Resources.Load<GraphicsQualityController>("Controller Prefabs/Graphics Quality Controller"), engineParentTransform);
            GameFlowController = Object.Instantiate(Resources.Load<GameFlowController>("Controller Prefabs/Game Flow Controller"), engineParentTransform);

            // Inject runtime data model dependencies
            TimeController.Initialize(time);
            InputController.Initialize();
            PostProcessingController.Initialize();
            GraphicsQualityController.Initialize(performance);
            GameFlowController.Initialize();
        }

        public void Save()
        {
            _saveGame.SaveToJSON("save_0");
        }
    }
}
