using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the player, which has his location
// When the player moves, it checks whether the move is valid or not
public class Player : MonoBehaviour
{
    protected int _locationCol, _locationRow;

    // Constructor
    public Player() { }

    public Player(int col, int row)
    {
        this._locationCol = col;
        this._locationRow = row;
    }

    // Methods
    public void move(Vector2 direction)
    {
        is_valid_location(direction);
    }

    // Is a valid place to push to
    public bool is_valid_location(Vector2 direction) 
    {
        if(Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        // Either x or y = 1
        direction.Normalize();

        if(Blocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            return true;
        }
    }

    bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPos = new Vector2(position.x, position.y) + direction;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach(var wall in walls)
        {
            if(wall.transform.position.x == newPos.x && wall.transform.position.y == newPos.y)
            {
                return true;
            }
        }
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach(var box in boxes)
        {
            if(box.transform.position.x == newPos.x && box.transform.position.y == newPos.y)
            {
                Box bx = box.GetComponent<Box>();
                if(bx && bx.move(direction))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }


    // Getter Setter of locationCol
    public int locationCol
    {
        get { return _locationCol; }
    }

    // Getter Setter of locationRow
    public int locationRow
    {
        get { return _locationRow; }
    }
}
