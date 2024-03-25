using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class TileScript : MonoBehaviour
{
    [Header("Neighbors")]
    [SerializeField] private List<string> _neighbors = new List<string>();
    [SerializeField] private float _side = 1.1f;
    private Renderer _renderer;

    public TileState State;

    // Event for notifying state change
    public event Action<TileScript> OnStateChanged;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        SetEnabled();
        Vector3 boxSizeX = new Vector3(0.1f, 0.1f, _side);
        Vector3 boxSizeY = new Vector3(_side, 0.1f, 0.1f);
        FindNeighbors(boxSizeX);
        FindNeighbors(boxSizeY);

    }

    void FindNeighbors(Vector3 boxSize)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize);
        
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Tiles")) _neighbors.Add(collider.gameObject.name);
            //Debug.Log($"{collider.gameObject.name}");

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && State != TileState.Disabled)
        {
            SteppedOn();
        }
    }

    public List<string> GetNeighbors() {  return _neighbors; }



    public void SetEnabled()
    {
        if (State != TileState.Disabled)
        {
            State = TileState.Enabled;
            _renderer.material.color = Color.white;
        } else { SetDisabled(); }
        
    }

    public void SetDisabled()
    {
        State = TileState.Disabled;
        _renderer.material.color = Color.grey;
    }

    public void SteppedOn()
    {
        //Debug.Log("Selecting");
        if (State == TileState.Enabled)
        {
            State = TileState.SteppedOn;
            _renderer.material.color = Color.blue;

            // Invoke the event to notify state change
            OnStateChanged?.Invoke(this);

        }

    }
}

//void OnDrawGizmosSelected()
//{
//    Gizmos.color = Color.red;
//    Gizmos.DrawWireCube(transform.position, new Vector3(0.1f, 0.1f, _side));
//    Gizmos.DrawWireCube(transform.position, new Vector3(_side, 0.1f, 0.1f));
//}
