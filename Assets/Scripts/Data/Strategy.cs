using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Strategy : ScriptableObject
{
    public static readonly int LeaderBoardSize = 10;

    [SerializeField] protected ShipGenome defaultGenome;
    [SerializeField] public string name;

    protected SortedList<int, ShipGenome> _leaderBoard;
    protected List<ShipController> _ships;

    public Action<SortedList<int, ShipGenome>> OnLeaderBoardRefresh;

    protected void OnEnable()
    {
        _leaderBoard = new SortedList<int, ShipGenome>();
        _ships = new List<ShipController>();
    }

    public virtual ShipGenome GetGenome()
    {
        GetLeaderBoard();
        return Instantiate(defaultGenome);
    }

    public void RegisterShip(ShipController shipController)
    {
        shipController.OnDie += OnShipDie;
        _ships.Add(shipController);
    }

    protected void OnShipDie(ShipController shipController)
    {
        if (_ships.Contains(shipController))
        {
            _ships.Remove(shipController);
        }

        shipController.OnDie -= OnShipDie;
        int score = shipController.GetScore();

        PutOnLeaderBoard(score, shipController.Genome);
    }

    protected void PutOnLeaderBoard(int score, ShipGenome genome)
    {
        while (_leaderBoard.ContainsKey(score))
        {
            score++;
        }

        _leaderBoard[score] = genome;
    }

    protected SortedList<int, ShipGenome> GetLeaderBoard()
    {
        var leaderBoard = new SortedList<int, ShipGenome>();
        foreach (var keyValuePair in _leaderBoard)
        {
            leaderBoard[keyValuePair.Key] = keyValuePair.Value;
        }

        foreach (var shipController in _ships)
        {
            int score = shipController.GetScore();
            while (leaderBoard.ContainsKey(score))
            {
                score++;
            }

            leaderBoard[score] = shipController.Genome;
        }
        
        OnLeaderBoardRefresh?.Invoke(leaderBoard);

        return leaderBoard;
    }
}