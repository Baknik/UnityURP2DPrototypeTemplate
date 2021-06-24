using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class GameFlow : MonoBehaviour
{
    [Header("Settings")]
    public Sprite KeyboardQuitIcon;
    public Sprite KeyboardReloadIcon;
    public Sprite GamepadQuitIcon;
    public Sprite GamepadReloadIcon;

    [Header("References")]
    public Image QuitIcon;
    public Image ReloadIcon;

    private PlayerInput playerInput;

    private string currentControlScheme;

    private void Awake()
    {
        this.playerInput = this.GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerInput.currentControlScheme != currentControlScheme)
        {
            OnControlsChanged(this.playerInput);
            currentControlScheme = this.playerInput.currentControlScheme;
        }
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void OnControlsChanged(PlayerInput playerInput)
    {
        switch (playerInput.currentControlScheme)
        {
            case "KeyboardMouse":
                QuitIcon.sprite = KeyboardQuitIcon;
                ReloadIcon.sprite = KeyboardReloadIcon;
                break;
            case "Gamepad":
                QuitIcon.sprite = GamepadQuitIcon;
                ReloadIcon.sprite = GamepadReloadIcon;
                break;
        }
    }
}
