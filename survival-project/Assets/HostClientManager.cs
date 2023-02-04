using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostClientManager : MonoBehaviour
{
    public bool IsHost;

    public void Start()
    {
        DontDestroyOnLoad(this); //Dont Destroy this on load
        IsHost = false;
    }

    public void PlayerClickedHost()
    {
        IsHost = true;
    }

    public void PlayerClickedClient()
    {
        IsHost = false;
    }
}
