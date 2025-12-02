using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public PlayerInputSet playerInput;

    //ÉùÃ÷ÊÂ¼þ
    public event EventHandler onPlayerMove;
    public event EventHandler onPlayerIdle;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInputSet();
    }
    private void Start()
    {
        
        playerInput.Enable();

        playerInput.Player.Movement.performed += ctx =>
        {
            
            onPlayerMove?.Invoke(this, EventArgs.Empty);
        };
        playerInput.Player.Movement.canceled += ctx =>
        {
            onPlayerIdle?.Invoke(this, EventArgs.Empty);
        };
      

    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    public Vector2 GetMoveValueNormailize()
    {
        return playerInput.Player.Movement.ReadValue<Vector2>();
    }


}
