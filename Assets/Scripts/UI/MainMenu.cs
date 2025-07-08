using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeField;
    public async void StartHost()
    {
        await HostManager.Instance.GameManager.StartHostSync();
    }

    public async void StartClient()
    {
        await ClientManager.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }
}
