using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private WorldDataCollector worldDataCollector;
    private int currentChunk;
    private GameObject[] worldChunks;

    //Get player object
    //Get played transform
    //Do a couroutine (every 5 seconds maybe?) to check what chunk you are in

    public void GetChunks()
    {
        worldChunks = worldDataCollector.worldChunks;
    }

    private void DisableAllChunks()
    {
        foreach (var chunk in worldChunks)
        {
            chunk.SetActive(false);
        }
    }

    //Make a function that takes in the current chunk, and enable or disables chunks based
    private void EnableChunks(int chunk)
    {
        if (currentChunk == 0)
        {
            DisableAllChunks();
            worldChunks[0].SetActive(true);
            worldChunks[1].SetActive(true);
            worldChunks[8].SetActive(true);
            worldChunks[9].SetActive(true);
        }
        else if (currentChunk == 1)
        {
            DisableAllChunks();
            worldChunks[1].SetActive(true);
            worldChunks[0].SetActive(true);
            worldChunks[2].SetActive(true);
            worldChunks[8].SetActive(true);
            worldChunks[9].SetActive(true);
            worldChunks[10].SetActive(true);
        }
        else if (currentChunk == 2)
        {
            DisableAllChunks();
            worldChunks[1].SetActive(true);
            worldChunks[2].SetActive(true);
            worldChunks[3].SetActive(true);
            worldChunks[9].SetActive(true);
            worldChunks[10].SetActive(true);
            worldChunks[11].SetActive(true);
        }
        else if (currentChunk == 3)
        {
            DisableAllChunks();
            worldChunks[2].SetActive(true);
            worldChunks[3].SetActive(true);
            worldChunks[4].SetActive(true);
            worldChunks[10].SetActive(true);
            worldChunks[11].SetActive(true);
            worldChunks[12].SetActive(true);
        }
        else if (currentChunk == 4)
        {
            DisableAllChunks();
            worldChunks[3].SetActive(true);
            worldChunks[4].SetActive(true);
            worldChunks[5].SetActive(true);
            worldChunks[11].SetActive(true);
            worldChunks[12].SetActive(true);
            worldChunks[13].SetActive(true);
        }
        else if (currentChunk == 5)
        {
            DisableAllChunks();
            worldChunks[4].SetActive(true);
            worldChunks[5].SetActive(true);
            worldChunks[6].SetActive(true);
            worldChunks[12].SetActive(true);
            worldChunks[13].SetActive(true);
            worldChunks[14].SetActive(true);
        }
        else if (currentChunk == 6)
        {
            DisableAllChunks();
            worldChunks[5].SetActive(true);
            worldChunks[6].SetActive(true);
            worldChunks[7].SetActive(true);
            worldChunks[13].SetActive(true);
            worldChunks[14].SetActive(true);
            worldChunks[15].SetActive(true);
        }
        else if (currentChunk == 4)
        {
            DisableAllChunks();
            worldChunks[6].SetActive(true);
            worldChunks[7].SetActive(true);
            worldChunks[14].SetActive(true);
            worldChunks[15].SetActive(true);
        }
    }

    private void CurrentChunkCheck(int x, int y)
    {
        if (x >= 0 && x <= 64 && y >= 0 && y <= 64)
        {
            currentChunk = 0;
        }
        else if (x >= 65 && x <= 128 && y >= 0 && y <= 64)
        {
            currentChunk = 1;
        }
        else if (x >= 129 && x <= 192 && y >= 0 && y <= 64)
        {
            currentChunk = 2;
        }
        else if (x >= 193 && x <= 256 && y >= 0 && y <= 64)
        {
            currentChunk = 3;
        }
        else if (x >= 257 && x <= 320 && y >= 0 && y <= 64)
        {
            currentChunk = 4;
        }
        else if (x >= 321 && x <= 384 && y >= 0 && y <= 64)
        {
            currentChunk = 5;
        }
        else if (x >= 385 && x <= 448 && y >= 0 && y <= 64)
        {
            currentChunk = 6;
        }
        else if (x >= 449 && x <= 512 && y >= 0 && y <= 64)
        {
            currentChunk = 7;
        }
        else if (x >= 0 && x <= 64 && y >= 65 && y <= 128)
        {
            currentChunk = 8;
        }
        else if (x >= 65 && x <= 128 && y >= 65 && y <= 128)
        {
            currentChunk = 9;
        }
        else if (x >= 129 && x <= 192 && y >= 65 && y <= 128)
        {
            currentChunk = 10;
        }
        else if (x >= 193 && x <= 256 && y >= 65 && y <= 128)
        {
            currentChunk = 11;
        }
        else if (x >= 257 && x <= 320 && y >= 65 && y <= 128)
        {
            currentChunk = 12;
        }
        else if (x >= 321 && x <= 384 && y >= 65 && y <= 128)
        {
            currentChunk = 13;
        }
        else if (x >= 385 && x <= 448 && y >= 65 && y <= 128)
        {
            currentChunk = 14;
        }
        else if (x >= 449 && x <= 512 && y >= 65 && y <= 128)
        {
            currentChunk = 15;
        }

        else if (x >= 0 && x <= 64 && y >= 129 && y <= 192)
        {
            currentChunk = 16;
        }
        else if (x >= 65 && x <= 128 && y >= 129 && y <= 192)
        {
            currentChunk = 17;
        }
        else if (x >= 129 && x <= 192 && y >= 129 && y <= 192)
        {
            currentChunk = 18;
        }
        else if (x >= 193 && x <= 256 && y >= 129 && y <= 192)
        {
            currentChunk = 19;
        }
        else if (x >= 257 && x <= 320 && y >= 129 && y <= 192)
        {
            currentChunk = 20;
        }
        else if (x >= 321 && x <= 384 && y >= 129 && y <= 192)
        {
            currentChunk = 21;
        }
        else if (x >= 385 && x <= 448 && y >= 129 && y <= 192)
        {
            currentChunk = 22;
        }
        else if (x >= 449 && x <= 512 && y >= 129 && y <= 192)
        {
            currentChunk = 23;
        }

        else if (x >= 0 && x <= 64 && y >= 193 && y <= 256)
        {
            currentChunk = 24;
        }
        else if (x >= 65 && x <= 128 && y >= 193 && y <= 256)
        {
            currentChunk = 25;
        }
        else if (x >= 129 && x <= 192 && y >= 193 && y <= 256)
        {
            currentChunk = 26;
        }
        else if (x >= 193 && x <= 256 && y >= 193 && y <= 256)
        {
            currentChunk = 27;
        }
        else if (x >= 257 && x <= 320 && y >= 193 && y <= 256)
        {
            currentChunk = 28;
        }
        else if (x >= 321 && x <= 384 && y >= 193 && y <= 256)
        {
            currentChunk = 29;
        }
        else if (x >= 385 && x <= 448 && y >= 193 && y <= 256)
        {
            currentChunk = 30;
        }
        else if (x >= 449 && x <= 512 && y >= 193 && y <= 256)
        {
            currentChunk = 31;
        }

        else if (x >= 0 && x <= 64 && y >= 257 && y <= 320)
        {
            currentChunk = 32;
        }
        else if (x >= 65 && x <= 128 && y >= 257 && y <= 320)
        {
            currentChunk = 33;
        }
        else if (x >= 129 && x <= 192 && y >= 257 && y <= 320)
        {
            currentChunk = 34;
        }
        else if (x >= 193 && x <= 256 && y >= 257 && y <= 320)
        {
            currentChunk = 35;
        }
        else if (x >= 257 && x <= 320 && y >= 257 && y <= 320)
        {
            currentChunk = 36;
        }
        else if (x >= 321 && x <= 384 && y >= 257 && y <= 320)
        {
            currentChunk = 37;
        }
        else if (x >= 385 && x <= 448 && y >= 257 && y <= 320)
        {
            currentChunk = 38;
        }
        else if (x >= 449 && x <= 512 && y >= 257 && y <= 320)
        {
            currentChunk = 39;
        }

        else if (x >= 0 && x <= 64 && y >= 321 && y <= 384)
        {
            currentChunk = 40;
        }
        else if (x >= 65 && x <= 128 && y >= 321 && y <= 384)
        {
            currentChunk = 41;
        }
        else if (x >= 129 && x <= 192 && y >= 321 && y <= 384)
        {
            currentChunk = 42;
        }
        else if (x >= 193 && x <= 256 && y >= 321 && y <= 384)
        {
            currentChunk = 43;
        }
        else if (x >= 257 && x <= 320 && y >= 321 && y <= 384)
        {
            currentChunk = 44;
        }
        else if (x >= 321 && x <= 384 && y >= 321 && y <= 384)
        {
            currentChunk = 45;
        }
        else if (x >= 385 && x <= 448 && y >= 321 && y <= 384)
        {
            currentChunk = 46;
        }
        else if (x >= 449 && x <= 512 && y >= 321 && y <= 384)
        {
            currentChunk = 47;
        }

        else if (x >= 0 && x <= 64 && y >= 385 && y <= 448)
        {
            currentChunk = 48;
        }
        else if (x >= 65 && x <= 128 && y >= 385 && y <= 448)
        {
            currentChunk = 49;
        }
        else if (x >= 129 && x <= 192 && y >= 385 && y <= 448)
        {
            currentChunk = 50;
        }
        else if (x >= 193 && x <= 256 && y >= 385 && y <= 448)
        {
            currentChunk = 51;
        }
        else if (x >= 257 && x <= 320 && y >= 385 && y <= 448)
        {
            currentChunk = 52;
        }
        else if (x >= 321 && x <= 384 && y >= 385 && y <= 448)
        {
            currentChunk = 53;
        }
        else if (x >= 385 && x <= 448 && y >= 385 && y <= 448)
        {
            currentChunk = 54;
        }
        else if (x >= 449 && x <= 512 && y >= 385 && y <= 448)
        {
            currentChunk = 55;
        }

        else if (x >= 0 && x <= 64 && y >= 449 && y <= 512)
        {
            currentChunk = 56;
        }
        else if (x >= 65 && x <= 128 && y >= 449 && y <= 512)
        {
            currentChunk = 57;
        }
        else if (x >= 129 && x <= 192 && y >= 449 && y <= 512)
        {
            currentChunk = 58;
        }
        else if (x >= 193 && x <= 256 && y >= 449 && y <= 512)
        {
            currentChunk = 59;
        }
        else if (x >= 257 && x <= 320 && y >= 449 && y <= 512)
        {
            currentChunk = 60;
        }
        else if (x >= 321 && x <= 384 && y >= 449 && y <= 512)
        {
            currentChunk = 61;
        }
        else if (x >= 385 && x <= 448 && y >= 449 && y <= 512)
        {
            currentChunk = 62;
        }
        else if (x >= 449 && x <= 512 && y >= 449 && y <= 512)
        {
            currentChunk = 63;
        }
    }
}
