using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class TurnManager : NetworkBehaviour
{
    public Dictionary<ulong, ConnectedPlayer> players;

    private void Start()
    {
        players = new Dictionary<ulong, ConnectedPlayer>
        {
            { 0, new ConnectedPlayer(TILEVALUE.X, true) },
            { 1, new ConnectedPlayer(TILEVALUE.O, false) }
        };
    }

    public TILEVALUE GetSymbol(ulong _id)
    {
        return players[_id].symbol;
    }

    public bool GetTurn(ulong _id)
    {
        return players[_id].isTurn;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateTurnServerRpc()
    {
        players = new Dictionary<ulong, ConnectedPlayer>
        {
            { 0, new ConnectedPlayer(TILEVALUE.X, !players[0].isTurn) },
            { 1, new ConnectedPlayer(TILEVALUE.O, !players[1].isTurn) }
        };
        UpdateTurnClientRpc(players[0].isTurn, players[1].isTurn);
        Debug.Log("Server 0: " + players[0].isTurn);
        Debug.Log("Server 1: " + players[1].isTurn);
    }

    [ClientRpc]
    private void UpdateTurnClientRpc(bool _zero, bool _one)
    {
        players[0] = new ConnectedPlayer(TILEVALUE.X, _zero);
        players[1] = new ConnectedPlayer(TILEVALUE.O, _one);
        Debug.Log("Client 0: " + players[0].isTurn);
        Debug.Log("Client 1: " + players[1].isTurn);

    }
}
public struct ConnectedPlayer : INetworkSerializable
{
    public TILEVALUE symbol;
    public bool isTurn;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref symbol);
        serializer.SerializeValue(ref isTurn);
    }
    public void FlipTurn()
    {
        isTurn = !isTurn;
    }
    public ConnectedPlayer(TILEVALUE _symbol, bool _turn)
    {
        this.symbol = _symbol;
        this.isTurn = _turn;
    }
}