using MVCEngine.Data;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MVCEngine
{
    public class InputController : MonoBehaviour, IController
    {
        private InputActions _inputActions;

        public delegate void Quit();
        public static event Quit OnQuit;

        public delegate void Reload();
        public static event Reload OnReload;

        public void Initialize()
        {
            
        }

        private void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();

            _inputActions.ActionMap.Quit.performed += ctx => HandleQuitPerformed(ctx);
            _inputActions.ActionMap.Reload.performed += ctx => HandleReloadPerformed(ctx);
        }

        private void OnDisable()
        {
            _inputActions.Disable();

            _inputActions.ActionMap.Quit.performed -= ctx => HandleQuitPerformed(ctx);
            _inputActions.ActionMap.Reload.performed -= ctx => HandleReloadPerformed(ctx);
        }

        private void HandleQuitPerformed(InputAction.CallbackContext ctx)
        {
            if (OnQuit != null)
            {
                OnQuit();
            }
        }

        private void HandleReloadPerformed(InputAction.CallbackContext ctx)
        {
            if (OnReload != null)
            {
                OnReload();
            }
        }
    }
}