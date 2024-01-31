using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerClick : NetworkBehaviour
{
    public NetworkVariable<TILEVALUE> playerFaction = new NetworkVariable<TILEVALUE>(TILEVALUE.NONE);
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SetFaction();
    }
    private void SetFaction()
    {
        if (OwnerClientId == 0)
        {
            playerFaction.Value = TILEVALUE.O;
        }
        if (OwnerClientId == 1)
        {
            playerFaction.Value = TILEVALUE.X;
        }
    }

    void Update()
    {
        if (!IsOwner)
            return;
        if (TurnHandler.Instance.GetTurn() != OwnerClientId)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            ClickServerRpc(playerFaction.Value, Input.mousePosition);
            //Debug.Log("Faction on client: " + playerFaction.Value);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClickServerRpc(TILEVALUE _faction, Vector3 _mousePos)
    {
        //Debug.Log("Faction on server: " + _faction);
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(_mousePos));

        if (rayHit.collider == null)
            return;

        if (rayHit.collider.gameObject.TryGetComponent<InteractiveTile>(out InteractiveTile tile))
        {
            Coordinate coords = tile.GetCoords();
            if (GridManager.Instance.TryUpdateTile(coords, _faction))
            {
                tile.SetValueServerRpc(_faction);
                GridManager.Instance.VictoryCheckServerRpc(coords, _faction);
                TurnHandler.Instance.EndTurn();
            }
        }
    }
}
