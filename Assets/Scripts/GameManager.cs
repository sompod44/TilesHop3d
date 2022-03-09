using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI GameObjects")]
    public GameObject goStartPanel;

    [Header("Background Color changing variables")]
    public SpriteRenderer spriteRender_ImgBG;
    public MeshRenderer meshRenderRoad;
    public MeshRenderer meshRenderRoadHard;
    public Sprite[] imagesBG;
    public Material[] matRoads;
    public Material[] matRoadHards;
    public Material[] matBlocks;
    public Material[] matFades;

    [Header("Public Variables")]
    public float secondOfGameStart;
    public float secondOfChangingBG;

    //static variables
    public static int environmentTypeIndex;

    // Start is called before the first frame update
    void Start()
    {
        environmentTypeIndex = 0;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame() //This IEnumerator start the game.
    {
        yield return new WaitForSeconds(secondOfGameStart);
        goStartPanel.SetActive(false);
        DDOL.instance.gameStart = true;
        StartCoroutine(ChangeEnvironmentColors());
    }

    IEnumerator ChangeEnvironmentColors() // This IEnumerator change the background
    {
        while(true)
        {
            yield return new WaitForSeconds(secondOfChangingBG);
            environmentTypeIndex++;
            environmentTypeIndex = environmentTypeIndex > 2 ? 0 : environmentTypeIndex;
            spriteRender_ImgBG.sprite = imagesBG[environmentTypeIndex];
            meshRenderRoad.material = matRoads[environmentTypeIndex];
            meshRenderRoadHard.material = matRoadHards[environmentTypeIndex];
            GameObject[] goCurrentTiles = GameObject.FindGameObjectsWithTag("tile");
            foreach (GameObject go in goCurrentTiles)
            {
                //Debug.Log(go.name);
                go.GetComponent<TileController>().meshRenderBlock.material = matBlocks[environmentTypeIndex];
                go.GetComponent<TileController>().meshRenderFade.material = matFades[environmentTypeIndex];
            }
        }

    }


}
