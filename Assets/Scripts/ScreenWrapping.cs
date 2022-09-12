using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour
{
    [HideInInspector]
    public Vector2 _new_position = new Vector2(0,0);

    public bool CheckWrapping(Vector2 position, float top, float bottom, float left, float right)
    {
        bool wrap = false;
        Vector2 new_position = new Vector2(0, 0);
        Vector2 current_position = position;

        if (current_position.y > top)
        {
            new_position = new Vector2(current_position.x, bottom);
            wrap = true;
        }
        if (current_position.y < bottom)
        {
            new_position = new Vector2(current_position.x, top);
            wrap = true;
        }
        if (current_position.x > right)
        {
            new_position = new Vector2(left, current_position.y);
            wrap = true;
        }
        if (current_position.x < left)
        {
            new_position = new Vector2(right, current_position.y);
            wrap = true;
        }

        if (wrap)
        {
            _new_position = new_position;
            return true;
        }
        else
            return false;
    }
}
