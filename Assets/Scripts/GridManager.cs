using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GridManager : NetworkBehaviour
{
    private Dictionary<Coordinate, TILEVALUE> GameState = new Dictionary<Coordinate, TILEVALUE>();
    [SerializeField] private int gridSize = 49;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private PlayerClick clickPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {

                GameObject newTile = Instantiate(tilePrefab, new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0), Quaternion.identity);
                newTile.GetComponent<NetworkObject>().Spawn();
                newTile.name = "[" + x + "|" + y + "]";
                newTile.GetComponentInChildren<InteractiveTile>().SetCoords(x, y);
                //newTile.transform.SetParent(this.transform);
                GameState.Add(new Coordinate(x, y), TILEVALUE.NONE);
            }
        }
    }

    [ServerRpc]
    private void UpdateTileServerRpc(Coordinate _coords, TILEVALUE _value)
    {
        GameState[_coords] = _value;
        if (VictoryCheck(_coords, _value))
        {
            Debug.Log(_value.ToString() + " WINS!");
            //end game client rpc;
        }
        Debug.Log("Tile updated in dictionary.");
    }


    public bool TryUpdateTile(Coordinate _coords, TILEVALUE _value)
    {
        if (GameState[_coords] != TILEVALUE.NONE)
            return false;

        UpdateTileServerRpc(_coords, _value);
        return true;
    }

    public bool VictoryCheck(Coordinate _coord, TILEVALUE _value)
    {
        int score = 0;
        //horizontal loop
        for (int x = -4; x <= 4; x++)
        {
            if (GameState[new Coordinate(_coord.x + x, _coord.y)] == _value)
                score++;
            else
                score = 0;

            if (score == 5)
                return true;
        }

        //vertical loop
        for (int y = -4; y <= 4; y++)
        {
            if (GameState[new Coordinate(_coord.x, _coord.y + y)] == _value)
                score++;
            else
                score = 0;

            if (score == 5)
                return true;
        }

        //first diagonal loop
        for (int i = -4; i <= 4; i++)
        {
            if (GameState[new Coordinate(_coord.x + i, _coord.y + i)] == _value)
                score++;
            else
                score = 0;

            if (score == 5)
                return true;
        }

        //second diagonal loop
        for (int i = -4; i <= 4; i++)
        {
            if (GameState[new Coordinate(_coord.x + i, _coord.y - i)] == _value)
                score++;
            else
                score = 0;

            if (score == 5)
                return true;
        }

        return false;
    }
}
