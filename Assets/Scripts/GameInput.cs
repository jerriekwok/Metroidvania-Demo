using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set;}

    //ÉùÃ÷ÊÂ¼þ
    public event EventHandler OnJumpPressed;
    public event EventHandler OnAttackPressed;

    private CharacterInputAction characterInputAction;
    private void Awake()
    {
        Instance = this;

        characterInputAction = new CharacterInputAction();

        characterInputAction.Player.Jump.performed += ctx =>
        {
            OnJumpPressed?.Invoke(this,EventArgs.Empty);
        };

        characterInputAction.Player.Attack.performed += ctx =>
        {
            OnAttackPressed?.Invoke(this, EventArgs.Empty);
        };

        characterInputAction.Player.Enable();

    }

    private void OnDestroy()
    {
        characterInputAction.Player.Disable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 moveDir = characterInputAction.Player.Move.ReadValue<Vector2>();
        moveDir = moveDir.normalized;
        return moveDir;
    }


}
