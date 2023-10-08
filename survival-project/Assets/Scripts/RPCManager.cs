using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RPCManager : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void DestroyObjectServerRpc(ServerRpcParams serverRpcParams = default)
    {
        NetworkObject networkObject = this.gameObject.GetComponent<NetworkObject>();
        networkObject.Despawn();
    }
}
