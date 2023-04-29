using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropLogic : MonoBehaviour
{
    private Stage currentStage;
    private float growTime = 5f; //Time for each grow stage in seconds
    private Wall wallScript;
    private Vector3 cropPos;
    private Vector3Int cropPosInt;

    [SerializeField] private GameObject cropItem;
    //[SerializeField] private GameObject cropSeed;
    [SerializeField] private RuleTile dryFarmTile;

    [SerializeField] private GameObject CropStage1;
    [SerializeField] private GameObject CropStage2;
    [SerializeField] private GameObject CropStage3;

    private Tilemap wallTilemap;
    private Tilemap groundTilemap;
    private enum Stage
    {
        Stage1,
        Stage2,
        Stage3,
    }

    private void Awake()
    {
        var tilemapObject = GameObject.FindWithTag("WallTilemap");
        wallTilemap = tilemapObject.GetComponent<Tilemap>();

        var groundTilemapObject = GameObject.FindWithTag("GroundTilemap");
        groundTilemap = groundTilemapObject.GetComponent<Tilemap>();

        currentStage = Stage.Stage1;
        wallScript = GetComponent<Wall>();
    }

    private void FixedUpdate()
    {
        cropPos = this.transform.position;
        cropPosInt = wallTilemap.WorldToCell(cropPos);

        switch (currentStage) //Switch statement on current stage
        {
            default:

            case Stage.Stage1:
                CropStage1.SetActive(true);
                CropStage2.SetActive(false);
                CropStage3.SetActive(false);
                StartCoroutine(GrowStage1());

                if (wallScript.currentHealth <= 0) //If this crop dies in stage 3
                {
                    Instantiate(cropItem, this.transform.position, Quaternion.identity);
                    wallTilemap.SetTile(cropPosInt, null);
                    Destroy(this.gameObject);
                }
                break;

            case Stage.Stage2:
                CropStage1.SetActive(false);
                CropStage2.SetActive(true);
                CropStage3.SetActive(false);
                StartCoroutine(GrowStage2());

                if (wallScript.currentHealth <= 0) //If this crop dies in stage 3
                {
                    Instantiate(cropItem, this.transform.position, Quaternion.identity);
                    wallTilemap.SetTile(cropPosInt, null);
                    Destroy(this.gameObject);
                }
                break;

            case Stage.Stage3:
                CropStage1.SetActive(false);
                CropStage2.SetActive(false);
                CropStage3.SetActive(true);

                if (wallScript.currentHealth <= 0) //If this crop dies in stage 3
                {                    
                    Instantiate(cropItem, this.transform.position, Quaternion.identity);
                    Instantiate(cropItem, this.transform.position, Quaternion.identity);
                    wallTilemap.SetTile(cropPosInt, null);
                    groundTilemap.SetTile(cropPosInt, dryFarmTile); //Turn tile back to try
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private bool IsCropWetDetector()
    {
        var tile = groundTilemap.GetTile(cropPosInt);
        if (tile.name == "WetFarmTile")
        {
            return true;
        }
        else return false;
    }

    private IEnumerator GrowStage1() //Stage 1 of the Plant
    {
        if (IsCropWetDetector() == true)
        {
            yield return new WaitForSeconds(growTime); //Wait for how long grow time is set for (in seconds)
            currentStage = Stage.Stage2; //Set stage to stage 2
            yield break; //End the couroutine
        }
    }

    private IEnumerator GrowStage2() //Stage 2 of the planet
    {
        yield return new WaitForSeconds(growTime); //Wait for grow time
        currentStage = Stage.Stage3; //Set stage to stage 3
        yield break; //End the couroutine
    }
}
