using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TurnHandler : SingletonNetworkBehaviour<TurnHandler>
{
    public NetworkVariable<ulong> turnId = new NetworkVariable<ulong>(1);
    public NetworkVariable<int> oScore = new NetworkVariable<int>(0);
    public NetworkVariable<int> xScore = new NetworkVariable<int>(0);

    public ulong GetTurn()
    {
        return turnId.Value;
    }

    public void EndTurn()
    {
        //turnId = turnId == 0 ? 1 : (ulong)0;
        if (turnId.Value == 0)
        {
            turnId.Value = 1;
        }
        else if (turnId.Value == 1)
        {
            turnId.Value = 0;
        }
    }

    public void Score(TILEVALUE _value)
    {
        if (_value == TILEVALUE.X)
            xScore.Value++;
        if (_value == TILEVALUE.O)
            oScore.Value++;
    }

    public void ResetScore()
    {
        xScore.Value = 0;
        oScore.Value = 0;
    }

}
