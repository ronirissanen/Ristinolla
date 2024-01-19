using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerClick : NetworkBehaviour
{
    private NetworkVariable<TILEVALUE> playerSymbol = new NetworkVariable<TILEVALUE>(TILEVALUE.NONE);
    private NetworkVariable<TILEVALUE> whoseTurn = new NetworkVariable<TILEVALUE>(TILEVALUE.X);
    public bool isMyTurn = false;
    private GridManager grid;

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            playerSymbol = new NetworkVariable<TILEVALUE>(TILEVALUE.O);
            isMyTurn = false;
        }
        if (IsHost)
        {
            playerSymbol = new NetworkVariable<TILEVALUE>(TILEVALUE.X);
            isMyTurn = true;
        }
        SetGridManager();
    }

    public void SetGridManager()
    {
        grid = FindObjectOfType<GridManager>();
    }

    public void TakeTurns()
    {
        isMyTurn = !isMyTurn;
    }

    [ClientRpc]
    private void TakeTurnsClientRpc()
    {
        isMyTurn = !isMyTurn;
    }

    void Update()
    {
        if (!IsOwner)
            return;
        if (!isMyTurn)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (rayHit.collider == null)
                return;
            if (rayHit.collider.gameObject.TryGetComponent<InteractiveTile>(out InteractiveTile tile))
            {
                if (grid.TryUpdateTile(tile.GetCoords(), playerSymbol.Value))
                {
                    tile.DrawSymbol(playerSymbol.Value);
                    TakeTurnsClientRpc();
                }
            }
        }
    }
}
