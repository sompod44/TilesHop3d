using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//This Enum check Tiles Type
public enum TileType
{
    Initial,
    Recreate
}

public class TilesManager : MonoBehaviour
{
    [Header("Tiles Color Changing Variable")]
    public Material[] matBlocks;
    public Material[] matFades;

    [Header("Tile Controlling variables")]
    public GameObject goLastTile;
    public Vector3 masterSpawnPos;
    public GameObject[] distinationSpawnPos;
    public GameObject goPrefabsTile;
    public float TileMovingSpeed;
    public int initialTileCount;
    public float masterSpawnSec;
    public float TilesSpawnSec;
    public bool isTileSpawnStart;
    public int whichTileCoroutineRunning;
    public int spawnTileCounter;
    public static TilesManager instance;
    // Start is called before the first frame update
    void Start()
    {
        isTileSpawnStart = true;
        whichTileCoroutineRunning = 0;
        spawnTileCounter = 0;
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            instance = null;
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This is for checking game start or not. And it just for first time..
        if(DDOL.instance.gameStart)
        {
            if(isTileSpawnStart)
            {
                isTileSpawnStart = false;
                StartCoroutine(RandomTileSpawn());

            }
        }
    }


    IEnumerator RandomTileSpawn() //This IEnumerator spawning tile into our game.
    {
        while(true)
        {
            if(spawnTileCounter <= 5) // New tile can't max from 5. (new tile can spawn when tiles are less than 5)
            {
                GameObject gotile = Instantiate(goPrefabsTile, masterSpawnPos, goPrefabsTile.transform.rotation);
                gotile.GetComponent<TileController>().ChangeTheBlockAndFadeColor(matBlocks[GameManager.environmentTypeIndex], matFades[GameManager.environmentTypeIndex]);
                Vector3 dis = distinationSpawnPos[Random.Range(0, distinationSpawnPos.Length)].transform.position;
                gotile.transform.DOMove(new Vector3(dis.x, dis.y, goLastTile.transform.position.z + 4.6f), 0.2f)
                    .OnComplete(() => DoSomethingAfterReachDis(gotile));
                spawnTileCounter++;
                ;
            }
            yield return new WaitForSeconds(TilesSpawnSec);
        }
    }   

    private void DoSomethingAfterReachDis(GameObject go) // This method call when new block place into Right position. 
    {
        go.GetComponent<TileController>().SetTileMovingState(TileType.Recreate, TileMovingSpeed, true);
        goLastTile = go;
    }
}
