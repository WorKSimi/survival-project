using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldSaveLoadManager : MonoBehaviour
{
    private string file1String = @"C:\JsonTest\Example/WorldData1.json";

    public Button SelectFile1;
    public Button SelectFile2;
    public Button SelectFile3;

    public Text file1Name;
    public Text file2Name;
    public Text file3Name;
}
