using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameHandler : SingletonNetworkBehaviour<GameHandler>
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
            return;
        Debug.Log("client network spawn");
        StartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc()
    {
        FindObjectOfType<GridManager>().GenerateGridServerRpc();
        StartGameClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        FindObjectOfType<MenuUI>().SetMenuActive(false);
    }



}
