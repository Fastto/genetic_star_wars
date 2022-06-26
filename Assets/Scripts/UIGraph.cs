using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGraph : MonoBehaviour
{
    [SerializeField] private Image _graph;
    private Texture2D _texture2D;

    private int _graphX = 0;

    void Start()
    {
        RectTransform rt = _graph.GetComponent<RectTransform>();
        _texture2D = new Texture2D((int) rt.rect.width, (int) rt.rect.height);
        _graph.material.mainTexture = _texture2D;

        CleanGraph();

        StartCoroutine(CollectData());
    }

    private void CleanGraph()
    {
        RectTransform rt = _graph.GetComponent<RectTransform>();
        Color c = new Color(0, 0, 0, .2f);
        for (int x = 0; x < (int) rt.rect.width; x++)
        {
            for (int y = 0; y < (int) rt.rect.height; y++)
            {
                _texture2D.SetPixel(x, y, c);
            }
        }

        _texture2D.Apply();
        _graphX = 0;
    }

    IEnumerator CollectData()
    {
        while (true)
        {
            ShipController[] _list = FindObjectsOfType<ShipController>();
            if (_list.Length > 0)
            {
                int redCount = 0;
                int blueCount = 0;
                
                foreach (var item in _list)
                {
                    if (item.Team.id == 1) redCount++;
                    if (item.Team.id == 2) blueCount++;
                }
                
                _texture2D.SetPixel(_graphX, redCount * 4, new Color(1, 0, 0, 1));
                _texture2D.SetPixel(_graphX, redCount * 4 + 1, new Color(1, 0, 0, 1));
                _texture2D.SetPixel(_graphX, redCount * 4 + 2, new Color(1, 0, 0, 1));
                _texture2D.SetPixel(_graphX, blueCount * 4 + 1, new Color(0, 0, 1, 1));
                _texture2D.SetPixel(_graphX, blueCount * 4 + 2, new Color(0, 0, 1, 1));
                _texture2D.SetPixel(_graphX, blueCount * 4 + 3, new Color(0, 0, 1, 1));
                _graphX++;
                _texture2D.Apply();
            }
            
            yield return new WaitForSeconds(.5f);
        }
    }
}
