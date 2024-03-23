using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    GameObject[] _tiles;
    [SerializeField] List<GameObject> _enabledTiles;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _tiles = GameObject.FindGameObjectsWithTag("Tiles");
        //Subscribing to the OnStateChanged from The tiles (just once)
        foreach(GameObject tileObject in _tiles) { 
            tileObject.GetComponent<TileScript>().OnStateChanged += HandleTileStateChanged;
            if (tileObject.GetComponent<TileScript>() != null &&
                tileObject.GetComponent<TileScript>().State == TileState.Enabled)
            {
                _enabledTiles.Add(tileObject);
            }
        }
        
    }

    private void EnableAllTiles()
    {
        foreach (GameObject tileObject in _tiles)
        {
            if (tileObject.GetComponent<TileScript>() != null)
            {
                tileObject.GetComponent<TileScript>().SetEnabled();
                _enabledTiles.Add(tileObject);
            }
        }
    }

    private bool CheckEnabled()
    {
        bool areEnabled = false;
        foreach (GameObject tileObject in _tiles)
        {
            if (tileObject.GetComponent<TileScript>() != null &&
                tileObject.GetComponent<TileScript>().State == TileState.Enabled)
            {
                areEnabled = true;
            }
        }
        return areEnabled;
    }

    void HandleTileStateChanged(TileScript tile)
    {
        if (tile.State == TileState.SteppedOn && _enabledTiles.Contains(tile.gameObject))
        {
            _enabledTiles.Clear();

            UpdateEnabledTiles(tile);
        } else if (!_enabledTiles.Contains(tile.gameObject)) { EnableAllTiles(); }
    }

    void UpdateEnabledTiles(TileScript changedTile)
    {

        // Get neighbors of the changed tile and add enabled ones to the list
        foreach (string neighborName in changedTile.GetNeighbors())
        {
            GameObject neighborObject = GameObject.Find(neighborName);
            if (neighborObject != null)
            {
                TileScript neighborTile = neighborObject.GetComponent<TileScript>();
                if (neighborTile != null && neighborTile.State == TileState.Enabled)
                {
                    _enabledTiles.Add(neighborObject);
                }
            }
        }

        if(!CheckEnabled()) { _renderer.material.color = Color.blue; }
    }




}
