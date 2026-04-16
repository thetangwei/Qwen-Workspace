using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// AR地球仪放置管理器 - 负责在检测到的平面上放置虚拟地球仪
/// </summary>
public class AREarthPlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject earthPrefab;
    [SerializeField] private Transform parentTransform;
    
    [Header("Settings")]
    [SerializeField] private float placementHeight = 0.1f;
    [SerializeField] private float earthScale = 1.0f;
    
    private GameObject placedEarth;
    private bool isEarthPlaced = false;
    
    public GameObject PlacedEarth => placedEarth;
    public bool IsEarthPlaced => isEarthPlaced;
    
    private void Awake()
    {
        if (raycastManager == null)
            raycastManager = FindObjectOfType<ARRaycastManager>();
            
        if (parentTransform == null)
            parentTransform = transform;
    }
    
    /// <summary>
    /// Try to place earth at screen position
    /// </summary>
    public bool TryPlaceEarth(Vector2 screenPosition)
    {
        if (raycastManager == null || !isEarthPlaced)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            
            if (raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                Vector3 placementPosition = hitPose.position + Vector3.up * placementHeight;
                
                PlaceEarth(placementPosition, hitPose.rotation);
                return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// Place earth at specified position
    /// </summary>
    private void PlaceEarth(Vector3 position, Quaternion rotation)
    {
        if (placedEarth != null)
        {
            Destroy(placedEarth);
        }
        
        if (earthPrefab != null)
        {
            placedEarth = Instantiate(earthPrefab, position, rotation, parentTransform);
            placedEarth.localScale = Vector3.one * earthScale;
            isEarthPlaced = true;
            
            Debug.Log("Earth placed successfully!");
        }
        else
        {
            Debug.LogError("Earth prefab is not assigned!");
        }
    }
    
    /// <summary>
    /// Remove placed earth
    /// </summary>
    public void RemoveEarth()
    {
        if (placedEarth != null)
        {
            Destroy(placedEarth);
            placedEarth = null;
            isEarthPlaced = false;
        }
    }
    
    /// <summary>
    /// Reposition earth to new location
    /// </summary>
    public void RepositionEarth(Vector3 newPosition)
    {
        if (placedEarth != null)
        {
            placedEarth.transform.position = newPosition;
        }
    }
}
