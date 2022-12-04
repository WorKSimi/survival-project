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
    [SerializeField] private Tile hoverTile = null;

    private Vector3 playerPos2;

    private Vector3 mouseWorldPos;

    private Vector2Int range = new Vector2Int(5, 4);

    private Vector3Int playerPos;

    private Vector3Int cellPosition;

    private Vector3Int previousMousePos = new Vector3Int();

    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    void Update()
    {
        
        Vector3Int cellPosition = grid.WorldToCell(playerPos);

        Vector3Int mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos))
        {
            if (IsInRange()) 
            {
                interactiveMap.SetTile(previousMousePos, null);
                interactiveMap.SetTile(mousePos, hoverTile);
                previousMousePos = mousePos;
            }
        }
    }

    private Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    public bool IsInRange()
    {
        float maxRange = 10.5f;
        float dist = Vector3.Distance(mouseWorldPos, playerPos2);
        if (dist <= maxRange)
        {
            return true;
        }
        else return false; 
    }
}