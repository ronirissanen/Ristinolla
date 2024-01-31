using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.Netcode;
using UnityEngine;

public class GridManager : SingletonNetworkBehaviour<GridManager>
{
    private NetworkVariable<bool> isGenerated = new NetworkVariable<bool>(false);
    private Dictionary<Coordinate, TILEVALUE> GameState = new Dictionary<Coordinate, TILEVALUE>();
    [SerializeField] private int gridSize = 49;
    [SerializeField] private GameObject tilePrefab;

    [ServerRpc(RequireOwnership = false)]
    public void GenerateGridServerRpc()
    {
        if (isGenerated.Value)
        {
            Debug.Log("Tried to generate the grid more than once.");
            Debug.Log("Resetting grid...");
            ResetGame();
            return;
        }
        for (int x = 0; x < gridSize; x++)
        {
            Debug.Log("Value of x in loop: " + x);
            for (int y = 0; y < gridSize; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0), Quaternion.identity);
                newTile.name = "[" + x + "|" + y + "]";
                newTile.GetComponent<NetworkObject>().Spawn();
                GameState.Add(new Coordinate(x, y), TILEVALUE.NONE);
                newTile.GetComponentInChildren<InteractiveTile>().SetCoords(x, y);
                Debug.Log("Spawned tile.");
            }
        }
        isGenerated.Value = true;
    }

    public bool TryUpdateTile(Coordinate _coords, TILEVALUE _value)
    {
        if (GameState[_coords] == TILEVALUE.NONE)
        {
            UpdateTileServerRpc(_coords, _value);
            return true;
        }
        else
        {
            return false;
        }
    }

    [ClientRpc]
    private void UpdateTileClientRpc(Coordinate _coords, TILEVALUE _value)
    {
        GameState[_coords] = _value;
    }

    [ServerRpc]
    public void UpdateTileServerRpc(Coordinate _coords, TILEVALUE _value)
    {
        Debug.Log(_value + " placed at " + "(" + _coords.x + "|" + _coords.y + ")");
        GameState[_coords] = _value;
        UpdateTileClientRpc(_coords, _value);
    }

    [ServerRpc]
    public void VictoryCheckServerRpc(Coordinate _coords, TILEVALUE _value)
    {
        if (VictoryCheck(_coords, _value))
        {
            Debug.Log(_value.ToString() + " WINS!");

            ResetGame();
        }
    }

    private void ResetGame()
    {
        foreach (var tile in FindObjectsOfType<InteractiveTile>())
        {
            Coordinate coords = tile.GetCoords();

            GameState[coords] = TILEVALUE.NONE;
            UpdateTileClientRpc(coords, TILEVALUE.NONE);
            tile.SetValueServerRpc(TILEVALUE.NONE);
        }
        Debug.Log("Grid reset.");
    }

    private bool VictoryCheck(Coordinate _coord, TILEVALUE _value)
    {
        if (_value == TILEVALUE.NONE)
            return false;

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
