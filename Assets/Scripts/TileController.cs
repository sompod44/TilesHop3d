using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileController : MonoBehaviour
{
    //public float second = 1;
    public TileType tiletype = TileType.Initial;
    public MeshRenderer meshRenderBlock;
    public MeshRenderer meshRenderFade;
    public GameObject goDimond;
    public GameObject goDimond3d;
    public GameObject goZoomEffect;
    public float tileSpeed = 0.05f;
    public float lastPos = -30.0f;
    public bool isDimond3dOpen;
    private DDOL _DDOL;
    private bool isTileMoving;
    private int tileTypeCheck = 0;
    // Start is called before the first frame update
    void Start()
    {
        isTileMoving = false;
        isDimond3dOpen = false;
        _DDOL = DDOL.instance;
        if(tiletype == TileType.Initial)
        {
            tileTypeCheck = 1;
        }
        else
        {
            tileTypeCheck = 2;
        }
    }

    //Update is called once per frame
    void Update()
    {
        if(tileTypeCheck == 1)
        {
            if (_DDOL.gameStart && !_DDOL.gamePause)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - tileSpeed*Time.deltaTime);

                if (transform.position.z <= lastPos)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if(tileTypeCheck == 2)
        {
            if (isTileMoving && !_DDOL.gamePause)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - tileSpeed*Time.deltaTime);

                if (transform.position.z <= lastPos)
                {
                    TilesManager.instance.spawnTileCounter--;
                    Destroy(gameObject);
                }
            }
        }


    }

    public void SetTileMovingState(TileType tileType,float speed, bool state)
    {
        tileSpeed = speed;
        tiletype = tileType;
        if (tiletype == TileType.Initial)
        {
            tileTypeCheck = 1;
        }
        else
        {
            tileTypeCheck = 2;
        }
        isTileMoving = true;
        int randDimond3D = Random.Range(1,100);
        if(randDimond3D >= 30 && randDimond3D <= 60)
        {
            isDimond3dOpen = true;
            goDimond3d.SetActive(true);
        }
        int rand = Random.Range(0,2);
        if(rand == 0)
        {
            goDimond.SetActive(false);
        }
        else
        {
            goDimond.SetActive(true);

        }
    }

    public void ChangeTheBlockAndFadeColor(Material block, Material fade)
    {
        meshRenderBlock.material = block;
        meshRenderFade.material = fade;
    }

    public float GetTileSpeed()
    {
        return tileSpeed;
    }
    
}
