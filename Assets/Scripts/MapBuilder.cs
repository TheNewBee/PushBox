using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define each item in a level by mapping a single char (Ex. #) to a prefab
[System.Serializable]
public class MapElement
{
    public string _character;
    public GameObject _prefab;
}

public class MapBuilder : MonoBehaviour
{
    public int selected;
    public List<MapElement> _mapElements;
    private Maps _maps;


    GameObject GetPrefab(char c)
    {
        MapElement mapElement = _mapElements.Find(le => le._character == c.ToString());
        if (mapElement != null)
        {
            return mapElement._prefab;
        }
        else
        {
            return null;
        }
    }

    public void selectMap(int selection)
    {
        selected = selection;
        if (selected >= GetComponent<Map>()._map.Count)
        {
            selected = 0;
        }
    }

    public void Build(int selection)
    {
        selectMap(selection);
        _maps = GetComponent<Map>()._map[selected];
        //Offset coordinatess so that centre of level is roughly at 0,0
        int startx = -_maps.Width / 2;
        int x = startx;
        int y = -_maps.Height / 2;
        foreach (var row in _maps.row)
        {
            foreach (var ch in row)
            {
                Debug.Log(ch);
                GameObject prefab = GetPrefab(ch);
                if (prefab)
                {
                    Debug.Log(prefab.name);
                    Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                }
                x++;
            }
            y++;
            x = startx;
        }
    }
}
