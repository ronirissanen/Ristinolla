using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerClick : NetworkBehaviour
{
    private GridManager grid;
    private TurnManager turn;

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += Setup;
    }

    public void Setup(ulong id)
    {
        grid = FindObjectOfType<GridManager>();
        turn = FindObjectOfType<TurnManager>();
    }

    void Update()
    {
        if (!IsOwner)
            return;

        //check turn
        if (!turn.GetTurn(OwnerClientId))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsHost)
                Debug.Log("MB1 down.");


            Debug.Log("Server gets ray.");
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (rayHit.collider == null)
            {
                Debug.Log("Rayhit collider is null");
                return;
            }
            if (rayHit.collider.gameObject.TryGetComponent<InteractiveTile>(out InteractiveTile tile))
            {
                if (grid.TryUpdateTile(tile.GetCoords(), turn.GetSymbol(OwnerClientId)))
                {
                    tile.DrawSymbolClientRpc(turn.GetSymbol(OwnerClientId));
                    Debug.Log("Take turns.");
                    turn.UpdateTurnServerRpc();
                }
                else
                {
                    Debug.Log("Tile was not valid.");
                }
            }
            else
            {
                Debug.Log("No tile component.");
            }
        }
    }
}
