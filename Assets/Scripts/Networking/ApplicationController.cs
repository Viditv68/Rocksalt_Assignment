using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientManager clientPref;
    [SerializeField] private HostManager hostPref;
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            
        }
        else
        {
            HostManager hostManager = Instantiate(hostPref);
            hostManager.CreateHost();
            
            ClientManager clientManager = Instantiate(clientPref);
            bool authenticated = await clientManager.CreateClient();
           
           //Go to Main Menu

           if (authenticated)
           {
               clientManager.GameManager.GoToMenu();
           }
        }
    }

}
