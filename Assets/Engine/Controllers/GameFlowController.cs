using MVCEngine.Data;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MVCEngine
{
    public class GameFlowController : MonoBehaviour, IController
    {
        public void Initialize()
        {
            
        }

        private void OnEnable()
        {
            InputController.OnQuit += HandleQuit;
            InputController.OnReload += HandleReload;
        }

        private void OnDisable()
        {
            InputController.OnQuit -= HandleQuit;
            InputController.OnReload -= HandleReload;
        }

        private void HandleQuit()
        {
            Application.Quit();
        }

        private void HandleReload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}