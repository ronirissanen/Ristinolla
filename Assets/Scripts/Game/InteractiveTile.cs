using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class InteractiveTile : NetworkBehaviour
{
    private NetworkVariable<Coordinate> coords = new NetworkVariable<Coordinate>();
    [SerializeField] private TextMeshPro symbol;

    public void SetCoords(int _x, int _y)
    {
        coords.Value = new Coordinate(_x, _y);
    }

    public Coordinate GetCoords()
    {
        return coords.Value;
    }

    [ServerRpc]
    public void SetValueServerRpc(TILEVALUE _value)
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
        SetValueClientRpc(symbol.text);
    }

    [ClientRpc]
    private void SetValueClientRpc(string _text)
    {
        symbol.text = _text;
    }
}

