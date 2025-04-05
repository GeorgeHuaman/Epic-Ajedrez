using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timers : SpatialNetworkBehaviour, IVariablesChanged
{
    public static Timers instance;
    public TMP_Text whiteTimerText;
    public TMP_Text blackTimerText;
    private NetworkVariable<float> countdownWhiteTime = new(initialValue: 1800f);
    private NetworkVariable<float> countdownBlackTime = new(initialValue: 1800f);
    private bool isRunningWhite = false;
    private bool isRunningBlack = false;

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateText();
        if (isRunningWhite)
        {
            CountDownWhite();
        }

        if (isRunningBlack) 
        {
            CountDownBlack();
        }
    }
    public void StartWhiteTimer()
    {
        GiveControl();
        isRunningWhite = true;
        isRunningBlack = false;
    }
    public void StartBlackTimer()
    {
        GiveControl();
        isRunningBlack = true;
        isRunningWhite = false;
    }
    public void CountDownWhite()
    {
        if (hasControl)
        {
            countdownWhiteTime.value -= Time.deltaTime;

            if (countdownWhiteTime <= 0)
            {
                countdownWhiteTime.value = 0;
                isRunningWhite = false;
            }
        }
        
    }
    public void CountDownBlack()
    {
        if (hasControl)
        {
            countdownBlackTime.value -= Time.deltaTime;

            if (countdownBlackTime <= 0)
            {
                countdownBlackTime.value = 0;
                isRunningBlack = false;
            }
        }
        
    }
    public void StopWhite()
    {
        isRunningWhite = false;
    }
    public void StopBlack()
    {
        isRunningBlack = false;
    }
    public void OnVariablesChanged(NetworkObjectVariablesChangedEventArgs args)
    {

        if (args.changedVariables.ContainsKey(countdownWhiteTime.id) || args.changedVariables.ContainsKey(countdownBlackTime.id))
        {
            UpdateText();
        }

    }
    private void UpdateText()
    {
        int totalSecondsWhite = Mathf.FloorToInt(countdownWhiteTime.value);
        int minutesWhite = totalSecondsWhite / 60;
        int secondsWhite = totalSecondsWhite % 60;

        int totalSecondsBlack = Mathf.FloorToInt(countdownBlackTime.value);
        int minutesBlack = totalSecondsBlack / 60;
        int secondsBlack = totalSecondsBlack % 60;

        whiteTimerText.text = $"{minutesWhite:00}:{secondsWhite:00}";
        blackTimerText.text = $"{minutesBlack:00}:{secondsBlack:00}";
    }
    public void GiveControl()
    {
        SpatialNetworkObject obj = GetComponent<SpatialNetworkObject>();
        obj.RequestOwnership();
    }
}
