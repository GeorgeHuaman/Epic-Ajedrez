using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SpatialSys.UnitySDK;
public class PanelVictory : SpatialNetworkBehaviour, IVariablesChanged
{
    public static PanelVictory instance;
    public TMP_Text text;
    private NetworkVariable<string> winner = new(initialValue: "");

    private void Start()
    {
        instance = this;
    }
    public void ResetText()
    {
        GiveControl();
        winner.value = "";
        text.text = "";
    }
    public void WhiteWins()
    {
        GiveControl();
        winner.value = "Blancas";
    }
    public void BlackWins()
    {
        GiveControl();
        winner.value = "Negras";
    }
    public void ShowWinner()
    {
        GiveControl();
        if(winner.value != "")
        {
            text.text = $"Ganaron las {winner.value}";
        }
        else if(winner.value == "")
        {
            text.text = "";
        }

    }

    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {
        if (args.changedVariables.ContainsKey(winner.id) && hasControl)
        {
            ShowWinner();
        }
            
    }

    public void GiveControl()
    {
        SpatialNetworkObject obj = GetComponent<SpatialNetworkObject>();
        obj.RequestOwnership();
    }
}
