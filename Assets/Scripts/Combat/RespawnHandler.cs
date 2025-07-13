using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        PlayerTank[] players = FindObjectsOfType<PlayerTank>();

        foreach (PlayerTank player in players)
        {
            HandlePlayerSpawned(player);
        }

        PlayerTank.OnPlayerSpawned += HandlePlayerSpawned;
        PlayerTank.OnPlayerDespawned += HandlePlayerDespawned;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            return;
        }
        
        PlayerTank.OnPlayerSpawned -= HandlePlayerSpawned;
        PlayerTank.OnPlayerDespawned -= HandlePlayerDespawned;
    }
    
    private void HandlePlayerSpawned(PlayerTank player)
    {
        player.Health.OnDie += (health) => HandlePlayerDie(player);
    }
    
    private void HandlePlayerDespawned(PlayerTank player)
    {
        player.Health.OnDie -= (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDie(PlayerTank player)
    {
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;

        NetworkObject player = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPosition(), Quaternion.identity);
        
        player.SpawnAsPlayerObject(ownerClientId);
    }

    
}
