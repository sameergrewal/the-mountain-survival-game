using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;
    private int alphamapWidth;
    private int alphamapHeight;
    private float[,,] alphamaps;
    
    void Start()
    {
        terrain = Terrain.activeTerrain;
        if (terrain != null)
        {
            terrainData = terrain.terrainData;
            alphamapWidth = terrainData.alphamapWidth;
            alphamapHeight = terrainData.alphamapHeight;
        }
    }
    
    public string GetCurrentTerrain(Vector3 playerPosition)
    {
        if (terrain == null || terrainData == null)
        {
            return "grass";
        }
        
        Vector3 terrainPosition = playerPosition - terrain.transform.position;
        Vector3 mapPosition = new Vector3(
            terrainPosition.x / terrainData.size.x,
            0,
            terrainPosition.z / terrainData.size.z
        );
        
        int x = (int)(mapPosition.x * alphamapWidth);
        int z = (int)(mapPosition.z * alphamapHeight);
        
        x = Mathf.Clamp(x, 0, alphamapWidth - 1);
        z = Mathf.Clamp(z, 0, alphamapHeight - 1);
        
        alphamaps = terrainData.GetAlphamaps(x, z, 1, 1);
        
        int dominantLayer = 0;
        float maxWeight = 0f;
        
        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            if (alphamaps[0, 0, i] > maxWeight)
            {
                maxWeight = alphamaps[0, 0, i];
                dominantLayer = i;
            }
        }
        
        return GetTerrainTypeName(dominantLayer);
    }
    
    string GetTerrainTypeName(int layerIndex)
    {
        if (terrainData.terrainLayers.Length <= layerIndex)
        {
            return "grass";
        }
        
        string layerName = terrainData.terrainLayers[layerIndex].name.ToLower();
        
        if (layerName.Contains("grass"))
        {
            return "grass";
        }
        else if (layerName.Contains("rock") || layerName.Contains("stone"))
        {
            return "rock";
        }
        else if (layerName.Contains("snow"))
        {
            return "snow";
        }
        else if (layerName.Contains("path") || layerName.Contains("dirt"))
        {
            return "rock";
        }
        
        return "grass";
    }
}
