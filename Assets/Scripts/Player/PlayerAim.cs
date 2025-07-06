using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAim : NetworkBehaviour
{
    private PlayerControls controls;
    [SerializeField] private Player Player;
    [SerializeField] private Transform turretTransform;
    
    private Vector2 mouseInput;


    public override void OnNetworkSpawn()
    {
        AssignInputEvents();
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        UpdateAimPosition();
    }

    private void UpdateAimPosition()
    {
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(mouseInput);
        turretTransform.up = new Vector2(aimWorldPosition.x - turretTransform.position.x,
            aimWorldPosition.y - turretTransform.position.y);
    }

    private void AssignInputEvents()
    {
        controls = Player.controls;
        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }
    


}
