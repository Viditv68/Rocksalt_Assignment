using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public PlayerControls controls { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();
    }

    public override void OnNetworkSpawn()
    {
        controls.Enable();
    }

    public override void OnNetworkDespawn()
    {
        controls.Disable();
    }
}