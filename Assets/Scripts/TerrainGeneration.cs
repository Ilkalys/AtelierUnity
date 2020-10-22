using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TerrainGeneration : MonoBehaviour
{
    public NavMeshSurface nav;

    [Header("Map Size")]
    public int cols = 100;
    public int rows = 100;
    [Header("World Generation Parameters")]
    public GameObject[] BlockTypes = new GameObject[5];
    public float[] rateBlockSpawnZtoO = new float[5];
    public GameObject Sand;
    public Vector2 sandRateFrame;

    public GameObject[] Tree;
    public float TreeSpawningRate;
    public float ForestFreq = 20f;
    public int TreeFreq;
    public float freq = 2f;


    [Header("First Building")]

    public GameObject TownsToSpawn;
    
    private float seed;
    private const float distanceToCam = 10;

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(0,10) * 100;
        int[,] blocks = GenerateTerrain();
        nav.BuildNavMesh();
        GenerateBeginning(blocks);
    }

    public int[,] GenerateTerrain()
    {
        int[,] blocks = new int[cols, rows];

        for(int x = 0; x < cols; x++)
        {
            for(int z = 0; z < rows; z++)
            {
                float y = Mathf.PerlinNoise((x + seed) /freq, (z + seed) /freq);
                GameObject currentBlockType = null;
                for (int i = 0; i < rateBlockSpawnZtoO.Length; i++)
                {
                    if(currentBlockType == null && y < rateBlockSpawnZtoO[i])
                    {
                        if(y < sandRateFrame.y && y > sandRateFrame.x)
                        {
                            currentBlockType = Sand;
                        }
                        else
                        {
                            currentBlockType = BlockTypes[i];
                        }
                        
                        y = i;
                    }
                }
                if (currentBlockType == null)
                {
                    currentBlockType = BlockTypes[BlockTypes.Length - 1];
                    y = BlockTypes.Length - 1;
                }
                GameObject newBlock = GameObject.Instantiate(currentBlockType, this.transform);
                newBlock.transform.position += new Vector3(x, y / 2, z);
                blocks[x, z] = (int)y;
                if (currentBlockType.name == "Ground")
                {
                    float treeSpawn = Mathf.PerlinNoise((x + seed*2) / ForestFreq, (z + seed) / ForestFreq);
                    if (treeSpawn < TreeSpawningRate || Random.Range(0, 100) < TreeFreq)
                    {
                        GameObject tree = GameObject.Instantiate(Tree[Random.Range(0,Tree.Length)], this.transform);
                        tree.transform.position += new Vector3(x, y / 2, z);
                        blocks[x, z] = 0;
                    }



                }

            }
        }
        return blocks;
    }


    public void GenerateBeginning(int[,] blocks)
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> TownList = new List<GameObject>();
        for(int i = 0; i < player.Length; i++)
        {
            bool correctPos = false;
            Vector3 position = Vector3.zero;
            while (!correctPos)
            {
                correctPos = true;
                int x = Random.Range(2, cols - 2);
                int z = Random.Range(2, rows - 2);

                position = new Vector3(x, blocks[x, z]/2 + this.transform.position.y, z);
                for(int v = -1; v <2; v++)
                {
                    for (int w = -1; w < 2; w++)
                    {
                        if (BlockTypes[blocks[x + v , z + w]].name != "Ground")
                        {
                            correctPos = false;
                        }
                    }
                }


            }
            GameObject Town = GameObject.Instantiate(TownsToSpawn);
            Town.GetComponent<Building>().player = player[i].GetComponent<Player>();
            Town.transform.position = position;
            Camera.main.transform.position = new Vector3(position.x, Camera.main.transform.position.y, position.z - distanceToCam); 
            TownList.Add(Town);

        }

    }


    }
