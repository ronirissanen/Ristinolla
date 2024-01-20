using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InteractiveTile : MonoBehaviour
{
    private Coordinate coords;
    [SerializeField] private TextMeshPro symbol;

    public void SetCoords(int _x, int _y)
    {
        coords.x = _x;
        coords.y = _y;
    }

    public Coordinate GetCoords()
    {
        return coords;
    }

    [ClientRpc]
    public void SetValueClientRpc(TILEVALUE _value)
    {
        switch (_value)
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
    }
}

