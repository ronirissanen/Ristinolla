using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerClick : NetworkBehaviour
{
    TILEVALUE playerFaction;
    private ulong turnId = 1;
    private GridManager grid;

    private void Start()
    {
        grid = GetComponent<GridManager>();
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
                    tile.SetValue(playerFaction);
                    EndTurn();
                }
            }
        }
    }

    private void EndTurn()
    {
        turnId = turnId == 0 ? 1 : (ulong)0;
    }
}
