using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define each item in a level by mapping a single char (Ex. #) to a prefab
[System.Serializable]
public class LevelElement
{
    public string _character;
    public GameObject _prefab;
}

public class LevelBuilder : MonoBehaviour
{
    public int _currLevel;
    public List<LevelElement> _levelElements;
    private Level _levels;

    GameObject GetPrefab(char c)
    {
        LevelElement levelElement = _levelElements.Find(le => le._character == c.ToString());
        if(levelElement != null)
        {
            return levelElement._prefab;
        }
        else
        {
            return null;
        }
    }

    public void Nextlevel()
    {
        _currLevel++;
        if(_currLevel >= GetComponent<Levels>()._levels.Count)
        {
            _currLevel = 0;
        }
    }

    public void Build()
    {
        _levels = GetComponent<Levels>()._levels[_currLevel];
        //Offset coordinatess so that centre of level is roughly at 0,0
        int startx = -_levels.Width / 2;
        int x = startx;
        int y = -_levels.Height / 2;
        foreach(var row in _levels.row)
        {
            foreach(var ch in row)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
