using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDeleteWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Tile"))
        {
            Destroy(other.transform.parent.gameObject);
        }   
    }
}
