using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerClick : NetworkBehaviour
{
    TILEVALUE playerFaction;
    public ulong turnId = 1;
    private GridManager grid;

    public void SetGrid(GridManager _grid)
    {
        grid = _grid;
    }

    void Update()
    {
        if (!IsOwner)
            return;
        if (turnId != OwnerClientId)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (rayHit.collider == null)
                return;

            if (rayHit.collider.gameObject.TryGetComponent<InteractiveTile>(out InteractiveTile tile))
            {
                Coordinate coords = tile.GetCoords();
                if (grid.TryUpdateTile(coords, playerFaction))
                {
                    tile.SetValueClientRpc(playerFaction);
                    EndTurnClientRpc();
                }
            }
        }
    }

    [ClientRpc]
    private void EndTurnClientRpc()
    {
        turnId = turnId == 0 ? 1 : (ulong)0;
    }
}
