using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float Speed = 1f;

    private Rigidbody2D rb2D;

    private Vector2 movementInput;

    private void Awake()
    {
        this.rb2D = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (this.movementInput.magnitude > 0f)
        {
            this.rb2D.velocity = this.movementInput * Speed;
        }
    }

    public void OnDirectional(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
