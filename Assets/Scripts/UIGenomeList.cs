using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGenomeList : MonoBehaviour
{
    [SerializeField] private List<UIGenomeRow> rows;

    public void Refresh(SortedList<int, ShipGenome> leaderBoard)
    {
        int rowsCount = rows.Count;
        if(leaderBoard.Count == 0 || rowsCount == 0)
            return;

        int id = leaderBoard.Count - 1;
        int rowId = 0;

        while (id >= 0 && rowId < rowsCount)
        {
            rows[rowId].ApplyGenome(leaderBoard.Keys[id], leaderBoard.Values[id]);
            id--;
            rowId++;
        }
    }
}
