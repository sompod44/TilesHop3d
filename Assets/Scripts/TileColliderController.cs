using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColliderController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if(tag == "tile")
        {
            TileController _tileController = other.gameObject.GetComponent<TileController>();
            PLayerController.instance.BallFall(_tileController.GetTileSpeed(), other.transform);
        }
    }
}
