using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameControl : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Dropdown redStrategy;
    [SerializeField] private Dropdown blueStrategy;
    
    private void Start()
    {
        restartButton.onClick.AddListener(OnRestartClickHandler);
    }

    private void OnRestartClickHandler()
    {
        // resourcesManager.ClearGold();
        // resourcesManager.ClearShips();
        // resourcesManager.ClearStations();
        //remove station resources to stop generation ships
        //redStation.Restart();

        //remove ships

        //remove gold

        //reset genome stat

        //reset station stat

        //reset graph

        //change station strategy

        //put gold on map

        //put initial resources

    }
}
