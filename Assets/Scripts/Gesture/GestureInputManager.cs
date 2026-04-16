using UnityEngine;

/// <summary>
/// 手势输入管理器 - 连接手势检测和地球控制
/// </summary>
public class GestureInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandGestureDetector gestureDetector;
    [SerializeField] private EarthController earthController;
    
    [Header("Settings")]
    [SerializeField] private bool enableGestureFeedback = true;
    [SerializeField] private float hapticFeedbackIntensity = 0.5f;
    
    private bool isInitialized = false;
    
    private void Awake()
    {
        Initialize();
    }
    
    private void OnEnable()
    {
        if (isInitialized)
        {
            SubscribeToEvents();
        }
    }
    
    private void OnDisable()
    {
        if (isInitialized)
        {
            UnsubscribeFromEvents();
        }
    }
    
    private void Initialize()
    {
        // Auto-find components if not assigned
        if (gestureDetector == null)
            gestureDetector = FindObjectOfType<HandGestureDetector>();
            
        if (earthController == null)
            earthController = FindObjectOfType<EarthController>();
        
        if (gestureDetector != null && earthController != null)
        {
            SubscribeToEvents();
            isInitialized = true;
            Debug.Log("GestureInputManager initialized successfully");
        }
        else
        {
            Debug.LogError("GestureInputManager missing required components!");
        }
    }
    
    private void SubscribeToEvents()
    {
        if (gestureDetector != null)
        {
            gestureDetector.OnRotateGesture += HandleRotateGesture;
            gestureDetector.OnZoomGesture += HandleZoomGesture;
            gestureDetector.OnGestureStart += HandleGestureStart;
            gestureDetector.OnGestureEnd += HandleGestureEnd;
        }
    }
    
    private void UnsubscribeFromEvents()
    {
        if (gestureDetector != null)
        {
            gestureDetector.OnRotateGesture -= HandleRotateGesture;
            gestureDetector.OnZoomGesture -= HandleZoomGesture;
            gestureDetector.OnGestureStart -= HandleGestureStart;
            gestureDetector.OnGestureEnd -= HandleGestureEnd;
        }
    }
    
    private void HandleRotateGesture(Vector2 delta)
    {
        if (earthController != null && !earthController.IsPaused)
        {
            earthController.ApplyRotation(delta);
        }
    }
    
    private void HandleZoomGesture(Vector2 delta)
    {
        if (earthController != null)
        {
            earthController.ApplyZoom(delta.y);
            
            // Haptic feedback at zoom limits
            if (enableGestureFeedback)
            {
                // Check if at zoom limit (simplified check)
                // In production, get actual zoom state from earthController
                ProvideHapticFeedback(0.3f);
            }
        }
    }
    
    private void HandleGestureStart()
    {
        if (enableGestureFeedback)
        {
            // Visual feedback could be added here
            Debug.Log("Gesture started");
        }
    }
    
    private void HandleGestureEnd()
    {
        if (enableGestureFeedback)
        {
            Debug.Log("Gesture ended");
        }
    }
    
    /// <summary>
    /// Provide haptic feedback
    /// </summary>
    private void ProvideHapticFeedback(float intensity)
    {
        #if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
        #endif
        
        // For more advanced haptics, use platform-specific APIs
    }
}
