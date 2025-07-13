using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer :  IDisposable
{
    private NetworkManager networkManager;
    private Dictionary<ulong, string> clientAuthId = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> authIdtoUserData = new Dictionary<string, UserData>();
    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        networkManager.NetworkConfig.ConnectionApproval = true;
        networkManager.ConnectionApprovalCallback += ApprovalCheck;
        networkManager.OnServerStarted += OnNetworkdReady;
    }


    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);

        clientAuthId[request.ClientNetworkId] = userData.userAuthId;
        authIdtoUserData[userData.userAuthId] = userData;
        Debug.Log(userData.userName);

        response.Approved = true;
        response.Position = SpawnPoint.GetRandomSpawnPosition();
        response.Rotation = Quaternion.identity;
        response.CreatePlayerObject = true;
    }
    
    private void OnNetworkdReady()
    {
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;

    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (clientAuthId.TryGetValue(clientId, out string authId))
        {
            clientAuthId.Remove(clientId);
            authIdtoUserData.Remove(authId);
        }
    }

    public UserData GetUserDataByClientId(ulong clientId)
    {
        if (clientAuthId.TryGetValue(clientId, out string authId))
        {
            if (authIdtoUserData.TryGetValue(authId, out UserData userData))
            {
                return userData;
            }

            return null;
        }

        return null;
    }

    public void Dispose()
    {
        if (networkManager != null)
        {
            networkManager.ConnectionApprovalCallback -= ApprovalCheck;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            networkManager.OnServerStarted -= OnNetworkdReady;

            if (networkManager.IsListening)
            {
                networkManager.Shutdown();
            }
        }
    }
}
