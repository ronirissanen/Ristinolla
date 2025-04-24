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

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        FindObjectOfType<MenuUI>().SetMenuActive(true);
        RelayHandler.Instance.ShutdownRelay();
        GridManager.Instance.IsNotGenerated();
        if (!IsServer)
            return;
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc()
    {
        GridManager.Instance.GenerateGridServerRpc();
        StartGameClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        FindObjectOfType<MenuUI>().SetMenuActive(false);
        FindObjectOfType<ScoreBoard>().StartCountingScore();
    }

}