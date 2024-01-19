using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractiveTile : MonoBehaviour
{
    private Coordinate coords;
    private TILEVALUE tileValue;
    [SerializeField] private TextMeshPro symbol;

    public void InitTile(int _x, int _y)
    {
        coords.x = _x;
        coords.y = _y;
    }

    public (Coordinate, TILEVALUE) TileWasClicked(TILEVALUE _value)
    {
        if (tileValue != TILEVALUE.NONE)
        {
            Debug.Log("Tile already has a value.");
            return (coords, TILEVALUE.NONE);
        }

        tileValue = _value;

        switch (tileValue)
        {
            case TILEVALUE.O:
                symbol.text = "O";
                break;

            case TILEVALUE.X:
                symbol.text = "X";
                break;

            default:
                symbol.text = "";

                break;
        }
        return (coords, tileValue);
    }
}

