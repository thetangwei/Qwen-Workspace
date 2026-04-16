using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// POI (Point of Interest) 管理器 - 管理地球上的兴趣点标记
/// </summary>
[System.Serializable]
public class POIData
{
    public string name;
    public string description;
    public double latitude;
    public double longitude;
    public POICategory category;
}

public enum POICategory
{
    City,
    Mountain,
    Ocean,
    Landmark,
    Country
}

public class POIManager : MonoBehaviour
{
    [Header("POI Settings")]
    [SerializeField] private GameObject poiMarkerPrefab;
    [SerializeField] private Transform earthTransform;
    [SerializeField] private float markerHeight = 0.05f;
    
    [Header("Sample POI Data")]
    [SerializeField] private List<POIData> poiDataList = new List<POIData>();
    
    private Dictionary<string, GameObject> spawnedMarkers = new Dictionary<string, GameObject>();
    
    private void Awake()
    {
        if (earthTransform == null)
            earthTransform = transform;
            
        InitializeSampleData();
    }
    
    /// <summary>
    /// Initialize with sample POI data
    /// </summary>
    private void InitializeSampleData()
    {
        // Sample major cities
        poiDataList.Add(new POIData 
        { 
            name = "北京", 
            description = "中国首都，政治文化中心", 
            latitude = 39.9042, 
            longitude = 116.4074,
            category = POICategory.City
        });
        
        poiDataList.Add(new POIData 
        { 
            name = "纽约", 
            description = "美国最大城市，金融中心", 
            latitude = 40.7128, 
            longitude = -74.0060,
            category = POICategory.City
        });
        
        poiDataList.Add(new POIData 
        { 
            name = "伦敦", 
            description = "英国首都，历史文化名城", 
            latitude = 51.5074, 
            longitude = -0.1278,
            category = POICategory.City
        });
        
        poiDataList.Add(new POIData 
        { 
            name = "东京", 
            description = "日本首都，科技与传统文化交融", 
            latitude = 35.6762, 
            longitude = 139.6503,
            category = POICategory.City
        });
        
        poiDataList.Add(new POIData 
        { 
            name = "珠穆朗玛峰", 
            description = "世界最高峰，海拔 8848.86 米", 
            latitude = 27.9881, 
            longitude = 86.9250,
            category = POICategory.Mountain
        });
    }
    
    /// <summary>
    /// Spawn all POI markers
    /// </summary>
    public void SpawnAllMarkers()
    {
        foreach (var poi in poiDataList)
        {
            SpawnMarker(poi);
        }
    }
    
    /// <summary>
    /// Spawn a single POI marker
    /// </summary>
    public GameObject SpawnMarker(POIData poi)
    {
        if (spawnedMarkers.ContainsKey(poi.name))
        {
            Debug.LogWarning($"Marker for {poi.name} already exists!");
            return spawnedMarkers[poi.name];
        }
        
        Vector3 position = LatLonToCartesian(poi.latitude, poi.longitude);
        GameObject marker = Instantiate(poiMarkerPrefab, position, Quaternion.identity, earthTransform);
        marker.name = $"POI_{poi.name}";
        
        // Add POI info component
        POIMarker markerInfo = marker.AddComponent<POIMarker>();
        markerInfo.Initialize(poi.name, poi.description);
        
        spawnedMarkers[poi.name] = marker;
        
        Debug.Log($"Spawned marker for {poi.name}");
        return marker;
    }
    
    /// <summary>
    /// Remove a POI marker
    /// </summary>
    public void RemoveMarker(string poiName)
    {
        if (spawnedMarkers.ContainsKey(poiName))
        {
            GameObject marker = spawnedMarkers[poiName];
            spawnedMarkers.Remove(poiName);
            Destroy(marker);
            Debug.Log($"Removed marker for {poiName}");
        }
    }
    
    /// <summary>
    /// Remove all POI markers
    /// </summary>
    public void RemoveAllMarkers()
    {
        foreach (var kvp in spawnedMarkers)
        {
            Destroy(kvp.Value);
        }
        spawnedMarkers.Clear();
    }
    
    /// <summary>
    /// Convert latitude/longitude to Cartesian coordinates
    /// </summary>
    private Vector3 LatLonToCartesian(double latitude, double longitude)
    {
        float radius = 0.5f + markerHeight; // Earth radius + offset
        
        double latRad = latitude * Mathf.Deg2Rad;
        double lonRad = longitude * Mathf.Deg2Rad;
        
        float x = radius * Mathf.Cos((float)latRad) * Mathf.Sin((float)lonRad);
        float y = radius * Mathf.Sin((float)latRad);
        float z = radius * Mathf.Cos((float)latRad) * Mathf.Cos((float)lonRad);
        
        return new Vector3(x, y, z);
    }
    
    /// <summary>
    /// Get POI data by name
    /// </summary>
    public POIData GetPOIData(string name)
    {
        return poiDataList.Find(p => p.name == name);
    }
}

/// <summary>
/// POI Marker component - handles click interactions
/// </summary>
public class POIMarker : MonoBehaviour
{
    private string poiName;
    private string poiDescription;
    
    public void Initialize(string name, string description)
    {
        poiName = name;
        poiDescription = description;
    }
    
    private void OnMouseDown()
    {
        Debug.Log($"POI clicked: {poiName}");
        
        // Notify UI manager to show POI info
        ARUIManager uiManager = FindObjectOfType<ARUIManager>();
        if (uiManager != null)
        {
            uiManager.DisplayPOIInfo(poiName, poiDescription);
        }
    }
}
