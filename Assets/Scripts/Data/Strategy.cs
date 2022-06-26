using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Strategy : ScriptableObject
{
    public static readonly int LeaderBoardSize = 10;

    [SerializeField] private float initialProbability;
    [SerializeField] private float probabilityDownRate;
    
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

    private float _lastTime = 0f;

    protected SortedList<int, ShipGenome> GetLeaderBoard()
    {
        float deltaTime = Time.time - _lastTime;
        _lastTime = Time.time;
        var leaderBoard = new SortedList<int, ShipGenome>();
        foreach (var keyValuePair in _leaderBoard)
        {
            int newScore = Mathf.RoundToInt(keyValuePair.Key - deltaTime * .25f);
            leaderBoard[newScore] = keyValuePair.Value;
        }
        
        _leaderBoard.Clear();
        foreach (var keyValuePair in leaderBoard)
        {
            _leaderBoard[keyValuePair.Key] = keyValuePair.Value;
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

    protected ShipGenome GetLeaderBoardGenome(SortedList<int, ShipGenome> leaderBoard)
    {
        ShipGenome genome = null;
        bool genomeFound = false;
        int leaderBoardSize = leaderBoard.Count;
        float probability = initialProbability;
        int minId = GetLeaderBoardMinId(leaderBoardSize);
        int maxId = GetLeaderBoardMaxId(leaderBoardSize);
        int id = 0;
        while (!genomeFound)
        {
            if (Random.value < probability)
            {
                genomeFound = true;
                genome = Instantiate(leaderBoard.Values[leaderBoardSize - id - 1]);
            }
            else
            {
                id++;
                probability *= probabilityDownRate;
            }

            if (!genomeFound && (id == leaderBoard.Count || id == LeaderBoardSize))
            {
                genomeFound = true;
                genome = Instantiate(leaderBoard.Values[Random.Range(minId, maxId)]);
            }
        }

        return genome;
    }

    protected int GetLeaderBoardMinId(int leaderBoardSize)
    {
        int minId = leaderBoardSize - 1 - LeaderBoardSize;
        minId = minId < 0 ? 0 : minId;

        return minId;
    }
    
    protected int GetLeaderBoardMaxId(int leaderBoardSize)
    {
        return leaderBoardSize - 1;
    }
}