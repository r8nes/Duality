using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector2 difference = Vector2.zero;
    Vector2 current;

    private float _offsetX = 2f;

    private void Start()
    {
        current = transform.position;
    }
    private void OnMouseDown()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;   
        if (transform.position.x < current.x - _offsetX)
        {
            Destroy(gameObject);
        }
    }
}