using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the box, which has its location
// arriveStorage checks whether the box is moved to storage or not
// When the player moves, it may trigger the move of a box
public class Box : MonoBehaviour
{
    // Fields
    protected string _boxType;
    protected int _boxWidth;
    protected int _locationCol, _locationRow;
    protected bool _arriveStorage;
    public bool _onCross;

    // Constructors
    public Box() { }

    public Box(int col, int row)
    {
        this._locationCol = col;
        this._locationRow = row;
    }

    public Box(string boxType, int boxWidth, int col, int row)
    {
        this._boxType = boxType;
        this._boxWidth = boxWidth;
        this._locationCol = col;
        this._locationRow = row;
    }

    // Methods

    // Is a valid place to push to
    public bool is_wrong_location(Vector3 position, Vector2 direction) 
    {
        Vector2 newPos = new Vector2(position.x, position.y) + direction;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            if (wall.transform.position.x == newPos.x && wall.transform.position.y == newPos.y)
            {
                return true;
            }
        }
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach (var box in boxes)
        {
            if (box.transform.position.x == newPos.x && box.transform.position.y == newPos.y)
            {
                return true;
            }
        }
        return false;
    }
    // Is the final correct place
    public bool is_correct_location() 
    {
        return false;
    }

    public bool move(Vector2 direction) 
    {
        return is_movable(direction);
    }

    public bool is_movable(Vector2 direction)
    {
        if (is_wrong_location(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            arriveStorage();
            return true;
        }
    }

    private void arriveStorage()
    {
        GameObject[] crosses = GameObject.FindGameObjectsWithTag("Cross");
        foreach(var cross in crosses)
        {
            if(transform.position.x == cross.transform.position.x && transform.position.y == cross.transform.position.y)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                _onCross = true;
                return;
            }
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        _onCross = false;
    }

    /*
    // Getter Setter of locationCol
    public int locationCol
    {
        get { return _locationCol; }
        set
        {
            if (is_valid_location())
            {
                _locationCol = value;
            }
        }
    }

    // Getter Setter of locationRow
    public int locationRow
    {
        get { return _locationRow; }
        set
        {
            if (is_valid_location())
            {
                _locationRow = value;
            }
        }
    }
    */
}
