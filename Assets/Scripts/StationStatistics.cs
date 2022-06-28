using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StationStatistics : MonoBehaviour
{
    [SerializeField] private StationController station;

    [SerializeField] private TextMeshProUGUI totalGold;
    [SerializeField] private TextMeshProUGUI availableGold;
    [SerializeField] private TextMeshProUGUI shipProduced;
    [SerializeField] private TextMeshProUGUI shipAlive;
    [SerializeField] private TextMeshProUGUI enemyKilled;
    [SerializeField] private TextMeshProUGUI team;
    [SerializeField] private TextMeshProUGUI strategy;
    [SerializeField] private TextMeshProUGUI score;

    private int _totalGold
    {
        get { return int.Parse(totalGold.text); }
        set { totalGold.text = value.ToString(); }
    }

    private int _availableGold
    {
        get { return int.Parse(availableGold.text); }
        set { availableGold.text = value.ToString(); }
    }

    private int _shipProduced
    {
        get { return int.Parse(shipProduced.text); }
        set { shipProduced.text = value.ToString(); }
    }

    private int _shipAlive
    {
        get { return int.Parse(shipAlive.text); }
        set { shipAlive.text = value.ToString(); }
    }

    private int _enemyKilled
    {
        get { return int.Parse(enemyKilled.text); }
        set { enemyKilled.text = value.ToString(); }
    }

    Quaternion rotation;
    private Vector3 position;

    private void OnEnable()
    {
        station.OnGoldChange += i => { _availableGold = i; };

        station.OnGoldCollected += i => { _totalGold += i; };

        station.OnShipProduce += controller =>
        {
            _shipProduced += 1;
            _shipAlive += 1;
        };

        station.OnShipDie += controller => { _shipAlive--; };
        station.OnEnemyDie += controller => { _enemyKilled++; };
    }

    void Awake()
    {
        rotation = transform.rotation;
        position = transform.position;
    }

    private void Start()
    {
        StartCoroutine(OnFirstFrame());
    }

    IEnumerator OnFirstFrame()
    {
        yield return null;
        team.text = station.team.id.ToString();
        strategy.text = station._strategy.name;
        station._strategy.OnLeaderBoardRefresh += OnLeaderBoardRefreshHandler;
    }

    void OnLeaderBoardRefreshHandler(SortedList<int, ShipGenome> list)
    {
        var keys = list.Keys;
        int scoreI = 0;
        foreach (var key in keys)
        {
            scoreI += key;
        }

        score.text = scoreI.ToString();
    }


    void LateUpdate()
    {
        transform.rotation = rotation;
        transform.position = position;
    }
}