using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public GameObject treePrefab;
    public int treeCount;
    public float terrainWidth;
    public LayerMask obstacleLayer;
    

    void Start()
    {
        GenerateTrees(treeCount);
    }



    void GenerateTrees(int treeCount)
    {
        Terrain terrain = GetComponent<Terrain>();
        float terrainWidth = terrain.terrainData.size.x;
        float treeAreaWidth = terrainWidth * 0.85f;
        float treeAreaOffsetX = (terrainWidth - treeAreaWidth) / 2f;

        for (int i = 0; i < treeCount; i++)
        {
            float randomX = Random.Range(treeAreaOffsetX, terrainWidth - treeAreaOffsetX);
            float randomZ = Random.Range(treeAreaOffsetX, terrainWidth - treeAreaOffsetX);

            Vector3 spawnPosition = new Vector3(randomX, terrain.SampleHeight(new Vector3(randomX, 0, randomZ)), randomZ);

            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }


}
