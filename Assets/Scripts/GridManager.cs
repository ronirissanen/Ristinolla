using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Dictionary<Coordinate, TILEVALUE> GameState = new Dictionary<Coordinate, TILEVALUE>();
    [SerializeField] private int gridSize = 49;
    [SerializeField] private GameObject tilePrefab;

    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {

                GameObject newTile = Instantiate(tilePrefab, new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0), Quaternion.identity);
                newTile.name = "[" + x + "|" + y + "]";
                newTile.GetComponentInChildren<InteractiveTile>().InitTile(x, y);
                GameState.Add(new Coordinate(x, y), TILEVALUE.NONE);
            }
        }
    }

    public void UpdateTile(Coordinate _coord, TILEVALUE _value)
    {
        GameState[_coord] = _value;
        if (VictoryCheck(_coord, _value))
        {
            Debug.Log(_value.ToString() + " WINS!");
            //gamemanager.endgame();
        }
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
