using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerTank : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private int ownerPriority = 20;

    public NetworkVariable<FixedString32Bytes> playerName =  new NetworkVariable<FixedString32Bytes>();
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = HostManager.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            
            playerName.Value= userData.userName;
        }
        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;
        }
    }
}
    
