using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    [SerializeField] private TMP_Text xScore;
    [SerializeField] private TMP_Text oScore;

    public void StartCountingScore()
    {
        TurnHandler.Instance.xScore.OnValueChanged += UpdateScoreXServerRpc;
        TurnHandler.Instance.oScore.OnValueChanged += UpdateScoreOServerRpc;
    }

    public void StopCountingScore()
    {
        // unsubaa noist eventeist, bro
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateScoreXServerRpc(int previous, int current)
    {
        xScore.text = current.ToString();
        UpdateScoreClientRpc(xScore.text, oScore.text);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateScoreOServerRpc(int previous, int current)
    {
        oScore.text = current.ToString();
        UpdateScoreClientRpc(xScore.text, oScore.text);
    }

    [ClientRpc]
    public void UpdateScoreClientRpc(string _x, string _o)
    {
        xScore.text = _x;
        oScore.text = _o;
    }
}
