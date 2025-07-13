using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerTank : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [field: SerializeField] public Health Health { get; private set; }
    [SerializeField] private int ownerPriority = 20;

    public NetworkVariable<FixedString32Bytes> playerName =  new NetworkVariable<FixedString32Bytes>();

    public static event Action<PlayerTank> OnPlayerSpawned;
    public static event Action<PlayerTank> OnPlayerDespawned;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = HostManager.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            
            playerName.Value= userData.userName;
            
            OnPlayerSpawned?.Invoke(this);
        }
        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
}
    
