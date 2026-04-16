using UnityEngine;

/// <summary>
/// AR 主控制器 - 协调整个 AR 体验的流程
/// </summary>
public class ARMainController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ARSessionManager sessionManager;
    [SerializeField] private AREarthPlacer earthPlacer;
    [SerializeField] private HandGestureDetector gestureDetector;
    [SerializeField] private EarthController earthController;
    [SerializeField] private POIManager poiManager;
    [SerializeField] private ARUIManager uiManager;
    
    [Header("Game Objects")]
    [SerializeField] private GameObject arContentRoot;
    
    [Header("Settings")]
    [SerializeField] private bool autoSpawnPOIs = true;
    
    private enum AppState
    {
        Initializing,
        WaitingForPlane,
        PlacingEarth,
        Running,
        Error
    }
    
    private AppState currentState = AppState.Initializing;
    
    private void Awake()
    {
        // Auto-find components if not assigned
        FindComponents();
    }
    
    private void Start()
    {
        InitializeApp();
    }
    
    /// <summary>
    /// Find all required components
    /// </summary>
    private void FindComponents()
    {
        if (sessionManager == null)
            sessionManager = FindObjectOfType<ARSessionManager>();
            
        if (earthPlacer == null)
            earthPlacer = FindObjectOfType<AREarthPlacer>();
            
        if (gestureDetector == null)
            gestureDetector = FindObjectOfType<HandGestureDetector>();
            
        if (earthController == null)
            earthController = FindObjectOfType<EarthController>();
            
        if (poiManager == null)
            poiManager = FindObjectOfType<POIManager>();
            
        if (uiManager == null)
            uiManager = FindObjectOfType<ARUIManager>();
    }
    
    /// <summary>
    /// Initialize the application
    /// </summary>
    private void InitializeApp()
    {
        Debug.Log("Initializing GeoSphere AR...");
        
        if (arContentRoot != null)
            arContentRoot.SetActive(false);
        
        currentState = AppState.Initializing;
        
        // Check AR support
        if (!Application.isMobilePlatform && !UnityEditor.EditorApplication.isPlaying)
        {
            Debug.LogWarning("AR features require mobile platform or Unity Editor with XR simulation");
        }
        
        Invoke(nameof(CheckARReady), 1f);
    }
    
    /// <summary>
    /// Check if AR is ready
    /// </summary>
    private void CheckARReady()
    {
        if (sessionManager != null && sessionManager.IsSessionReady)
        {
            currentState = AppState.WaitingForPlane;
            Debug.Log("AR Session ready, waiting for plane detection...");
            
            if (uiManager != null)
            {
                uiManager.ShowLoading(false);
                uiManager.ShowInstructions(true);
            }
        }
        else
        {
            Debug.LogWarning("AR Session not ready, retrying...");
            Invoke(nameof(CheckARReady), 1f);
        }
    }
    
    private void Update()
    {
        switch (currentState)
        {
            case AppState.WaitingForPlane:
                HandleWaitingForPlane();
                break;
                
            case AppState.Running:
                HandleRunning();
                break;
        }
    }
    
    /// <summary>
    /// Handle waiting for plane state
    /// </summary>
    private void HandleWaitingForPlane()
    {
        // Show placement hint
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = GetTouchPosition();
            
            if (earthPlacer != null && earthPlacer.TryPlaceEarth(touchPosition))
            {
                OnEarthPlaced();
            }
        }
    }
    
    /// <summary>
    /// Handle running state
    /// </summary>
    private void HandleRunning()
    {
        // Main interaction loop - gestures are handled by GestureInputManager
    }
    
    /// <summary>
    /// Called when earth is successfully placed
    /// </summary>
    private void OnEarthPlaced()
    {
        Debug.Log("Earth placed! Starting experience...");
        
        currentState = AppState.Running;
        
        if (arContentRoot != null)
            arContentRoot.SetActive(true);
        
        if (uiManager != null)
        {
            uiManager.ShowControls(true);
            uiManager.ShowInstructions(false);
        }
        
        // Spawn POI markers
        if (autoSpawnPOIs && poiManager != null)
        {
            poiManager.SpawnAllMarkers();
        }
    }
    
    /// <summary>
    /// Get touch or mouse position
    /// </summary>
    private Vector2 GetTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }
        return Vector2.zero;
    }
    
    /// <summary>
    /// Reset the AR experience
    /// </summary>
    public void ResetExperience()
    {
        Debug.Log("Resetting AR experience...");
        
        if (earthPlacer != null)
            earthPlacer.RemoveEarth();
            
        if (poiManager != null)
            poiManager.RemoveAllMarkers();
            
        if (arContentRoot != null)
            arContentRoot.SetActive(false);
        
        currentState = AppState.WaitingForPlane;
        
        if (uiManager != null)
        {
            uiManager.ShowControls(false);
            uiManager.ShowInstructions(true);
        }
    }
    
    /// <summary>
    /// Restart AR session
    /// </summary>
    public void RestartARSession()
    {
        if (sessionManager != null)
        {
            sessionManager.ResetSession();
        }
        
        ResetExperience();
        InitializeApp();
    }
}
