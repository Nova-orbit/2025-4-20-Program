using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyUi : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject BuidingMode, BridgeToggle, SupportToggle,RopeToggle, ContinuousModeToggle, BuidingModeToggle;//建造模式全家桶
    public GameObject DestoryModeToggle;
    public GameObject AdjustModeToggle;

    //方法
    private bool IsOn(GameObject tg)
    {
        return tg.GetComponent<Toggle>().isOn;
    }
    public void OnStartButtonClick()
    {
        Global.StartButtonOn = true;
        StartButton.SetActive(false);
        Global.NumToBuild = 0;
        Global.AdjustMode = false;
        Global.BuildingMode = false;
        Global.DestoryMode = false;
    }
    public void OnBridgeToggleClick()
    {
        Global.NumToBuild = IsOn(BridgeToggle) ? 1 : 0;
    }
    public void OnSupportToggleClick()
    {
        Global.NumToBuild = IsOn(SupportToggle) ? 2 : 0;
    }

    public void OnRopeToggleClick()
    {
        Global.NumToBuild = IsOn(RopeToggle) ? 3 : 0;
    }
    public void OnBuidingModeToggleClick()
    {
        Global.BuildingMode = IsOn(BuidingModeToggle);
        BuidingMode.SetActive(IsOn(BuidingModeToggle));
    }

    public void OnContinuousModeToggleClick()
    {
        Global.ContinuousMode = IsOn(ContinuousModeToggle);
    }

    public void OnDestoryModeToggleClick()
    {
        Global.DestoryMode =IsOn(DestoryModeToggle);
    }
    public void OnAdjustModeToggleClick()
    {
        Global.AdjustMode = IsOn(AdjustModeToggle);
    }
}
