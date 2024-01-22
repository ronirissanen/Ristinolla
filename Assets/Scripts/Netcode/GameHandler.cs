using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameHandler : SingletonNetworkBehaviour<GameHandler>
{



    [ClientRpc]
    public void EndGameClientRpc()
    {

    }

    public void StartGame()
    {
        FindObjectOfType<GridManager>().GenerateGridServerRpc();
    }


}
