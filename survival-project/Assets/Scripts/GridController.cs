using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridController : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tilemap pathMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile pathTile = null;

    private Vector2Int range = new Vector2Int(5, 4);

    private Vector3Int playerPos;

    private Vector3Int previousMousePos = new Vector3Int();

    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    void Update()
    {
        // Mouse over, highlight the tile
        playerPos = pathMap.WorldToCell(transform.position);

        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos))
        {
            //if (InRange(playerPos, mousePos, (Vector3Int)range))
            //{
                interactiveMap.SetTile(previousMousePos, null);
                interactiveMap.SetTile(mousePos, hoverTile);
                previousMousePos = mousePos;
            //}
        }

        // Left mouse click > add path tile
        //if (Input.GetMouseButton(0))
        //{
        //    pathMap.SetTile(mousePos, pathTile);
        //}

        // Right mouse click > remove path tile
        if (Input.GetMouseButton(1))
        {
            //pathMap.SetTile(mousePos, pathTile);
            
        }
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    private bool InRange(Vector3Int positionA, Vector3Int positionB, Vector3Int range)
    {
        Vector3Int distance = positionA - positionB;

        if (Math.Abs(distance.x) >= range.x || Math.Abs(distance.y) >= range.y)
        {
            return false;
        }
        return true;
    }
}
