using UnityEngine;

/// <summary>
/// 地球控制器 - 负责地球的旋转、缩放和视图模式切换
/// </summary>
[RequireComponent(typeof(Renderer))]
public class EarthController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationDamping = 0.95f;
    [SerializeField] private float autoRotationSpeed = 0.5f;
    [SerializeField] private bool enableAutoRotation = false;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSmoothTime = 0.3f;
    [SerializeField] private float defaultDistance = 1.0f;
    
    [Header("View Modes")]
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;
    [SerializeField] private Material terrainMaterial;
    [SerializeField] private GameObject cloudLayer;
    [SerializeField] private GameObject atmosphereGlow;
    
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform pivotTransform;
    
    // Private state
    private Renderer earthRenderer;
    private Vector3 currentRotationVelocity;
    private float currentZoomVelocity;
    private ViewMode currentViewMode = ViewMode.Day;
    private bool isPaused = false;
    
    public enum ViewMode
    {
        Day,
        Night,
        Terrain
    }
    
    public bool IsPaused => isPaused;
    public ViewMode CurrentViewMode => currentViewMode;
    
    private void Awake()
    {
        earthRenderer = GetComponent<Renderer>();
        
        if (pivotTransform == null)
            pivotTransform = transform.parent;
            
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }
    
    private void Update()
    {
        if (!isPaused)
        {
            HandleAutoRotation();
        }
    }
    
    /// <summary>
    /// Apply rotation from gesture input
    /// </summary>
    public void ApplyRotation(Vector2 rotationDelta)
    {
        if (pivotTransform != null)
        {
            // Rotate around Y axis for horizontal drag
            pivotTransform.Rotate(Vector3.up, rotationDelta.x * Time.deltaTime, Space.World);
            
            // Rotate around X axis for vertical drag (with limits)
            float newRotationX = Mathf.Clamp(
                pivotTransform.eulerAngles.x + rotationDelta.y * Time.deltaTime,
                -80f, 80f
            );
            pivotTransform.rotation = Quaternion.Euler(newRotationX, pivotTransform.eulerAngles.y, 0);
        }
        else
        {
            // Fallback: rotate the earth itself
            transform.Rotate(Vector3.up, rotationDelta.x * Time.deltaTime, Space.Self);
            transform.Rotate(Vector3.right, rotationDelta.y * Time.deltaTime, Space.Self);
        }
    }
    
    /// <summary>
    /// Apply zoom from gesture input
    /// </summary>
    public void ApplyZoom(float zoomDelta)
    {
        if (cameraTransform == null || pivotTransform == null)
            return;
        
        float currentDistance = Vector3.Distance(cameraTransform.position, pivotTransform.position);
        float targetDistance = currentDistance - zoomDelta * Time.deltaTime;
        
        // Clamp zoom distance
        targetDistance = Mathf.Clamp(targetDistance, 0.3f, 3.0f);
        
        // Smooth zoom
        Vector3 direction = (cameraTransform.position - pivotTransform.position).normalized;
        Vector3 newPosition = pivotTransform.position + direction * targetDistance;
        cameraTransform.position = Vector3.SmoothDamp(
            cameraTransform.position, 
            newPosition, 
            ref currentZoomVelocity, 
            zoomSmoothTime
        );
    }
    
    /// <summary>
    /// Set zoom distance directly
    /// </summary>
    public void SetZoomDistance(float distance)
    {
        if (cameraTransform == null || pivotTransform == null)
            return;
        
        distance = Mathf.Clamp(distance, 0.3f, 3.0f);
        
        Vector3 direction = (cameraTransform.position - pivotTransform.position).normalized;
        cameraTransform.position = pivotTransform.position + direction * distance;
    }
    
    /// <summary>
    /// Toggle auto rotation
    /// </summary>
    private void HandleAutoRotation()
    {
        if (enableAutoRotation && !isPaused)
        {
            transform.Rotate(Vector3.up, autoRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
    
    /// <summary>
    /// Change earth view mode
    /// </summary>
    public void SetViewMode(ViewMode mode)
    {
        currentViewMode = mode;
        
        switch (mode)
        {
            case ViewMode.Day:
                if (dayMaterial != null)
                    earthRenderer.material = dayMaterial;
                ShowClouds(true);
                ShowAtmosphere(true);
                break;
                
            case ViewMode.Night:
                if (nightMaterial != null)
                    earthRenderer.material = nightMaterial;
                ShowClouds(false);
                ShowAtmosphere(false);
                break;
                
            case ViewMode.Terrain:
                if (terrainMaterial != null)
                    earthRenderer.material = terrainMaterial;
                ShowClouds(false);
                ShowAtmosphere(true);
                break;
        }
        
        Debug.Log($"View mode changed to: {mode}");
    }
    
    /// <summary>
    /// Toggle pause state
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log($"Earth rotation paused: {isPaused}");
    }
    
    /// <summary>
    /// Set pause state
    /// </summary>
    public void SetPause(bool paused)
    {
        isPaused = paused;
    }
    
    /// <summary>
    /// Show/hide cloud layer
    /// </summary>
    private void ShowClouds(bool show)
    {
        if (cloudLayer != null)
            cloudLayer.SetActive(show);
    }
    
    /// <summary>
    /// Show/hide atmosphere glow
    /// </summary>
    private void ShowAtmosphere(bool show)
    {
        if (atmosphereGlow != null)
            atmosphereGlow.SetActive(show);
    }
    
    /// <summary>
    /// Reset earth to default position and rotation
    /// </summary>
    public void ResetPosition()
    {
        transform.localRotation = Quaternion.identity;
        
        if (pivotTransform != null)
        {
            pivotTransform.localRotation = Quaternion.identity;
        }
        
        SetZoomDistance(defaultDistance);
    }
}
