using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 手势检测器 - 负责识别和处理手部手势
/// 使用 MediaPipe Hands 或平台原生 API
/// </summary>
public class HandGestureDetector : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float deadZoneThreshold = 0.01f;
    [SerializeField] private float rotationSensitivity = 2.0f;
    [SerializeField] private float zoomSensitivity = 5.0f;
    [SerializeField] private float minZoomDistance = 0.3f;
    [SerializeField] private float maxZoomDistance = 2.0f;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;
    
    // Gesture states
    private bool isDragging = false;
    private bool isPinching = false;
    private Vector2 lastHandPosition;
    private float lastPinchDistance;
    
    // Events
    public delegate void GestureEventHandler(Vector2 delta);
    public event GestureEventHandler OnRotateGesture;
    public event GestureEventHandler OnZoomGesture;
    public event System.Action OnGestureStart;
    public event System.Action OnGestureEnd;
    
    /// <summary>
    /// Process hand position update (called from MediaPipe or native API)
    /// </summary>
    public void UpdateHandPosition(Vector2 screenPosition, bool isHandDetected)
    {
        if (!isHandDetected)
        {
            if (isDragging)
            {
                EndDrag();
            }
            return;
        }
        
        if (!isDragging)
        {
            StartDrag(screenPosition);
        }
        else
        {
            ContinueDrag(screenPosition);
        }
        
        lastHandPosition = screenPosition;
    }
    
    /// <summary>
    /// Process pinch gesture (distance between thumb and index finger)
    /// </summary>
    public void UpdatePinchDistance(float distance, bool isPinchDetected)
    {
        if (!isPinchDetected)
        {
            if (isPinching)
            {
                EndPinch();
            }
            return;
        }
        
        if (!isPinching)
        {
            StartPinch(distance);
        }
        else
        {
            ContinuePinch(distance);
        }
        
        lastPinchDistance = distance;
    }
    
    private void StartDrag(Vector2 position)
    {
        isDragging = true;
        lastHandPosition = position;
        
        if (showDebugLogs)
            Debug.Log("Drag started");
            
        OnGestureStart?.Invoke();
    }
    
    private void ContinueDrag(Vector2 currentPosition)
    {
        Vector2 delta = currentPosition - lastHandPosition;
        
        // Apply dead zone filter
        if (delta.magnitude < deadZoneThreshold)
            return;
        
        // Fire rotate event
        OnRotateGesture?.Invoke(delta * rotationSensitivity);
        
        if (showDebugLogs)
            Debug.Log($"Drag delta: {delta}");
    }
    
    private void EndDrag()
    {
        isDragging = false;
        
        if (showDebugLogs)
            Debug.Log("Drag ended");
            
        OnGestureEnd?.Invoke();
    }
    
    private void StartPinch(float distance)
    {
        isPinching = true;
        lastPinchDistance = distance;
        
        if (showDebugLogs)
            Debug.Log("Pinch started");
    }
    
    private void ContinuePinch(float currentDistance)
    {
        float delta = currentDistance - lastPinchDistance;
        
        // Apply dead zone filter
        if (Mathf.Abs(delta) < deadZoneThreshold)
            return;
        
        // Fire zoom event (negative delta = zoom in, positive = zoom out)
        OnZoomGesture?.Invoke(new Vector2(0, delta * zoomSensitivity));
        
        if (showDebugLogs)
            Debug.Log($"Pinch delta: {delta}");
    }
    
    private void EndPinch()
    {
        isPinching = false;
        
        if (showDebugLogs)
            Debug.Log("Pinch ended");
    }
    
    /// <summary>
    /// Clamp zoom distance to min/max values
    /// </summary>
    public float ClampZoomDistance(float distance)
    {
        return Mathf.Clamp(distance, minZoomDistance, maxZoomDistance);
    }
    
    // Touch fallback for testing without hand tracking
    private Vector2? lastTouchPosition;
    
    private void Update()
    {
        // Fallback touch input for testing
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && lastTouchPosition.HasValue)
            {
                Vector2 delta = touch.position - lastTouchPosition.Value;
                OnRotateGesture?.Invoke(delta * rotationSensitivity);
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                lastTouchPosition = null;
            }
        }
        
        // Two-finger pinch for zoom
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            
            Vector2 prevDelta = touch0.position - touch1.position;
            Vector2 currDelta = touch0.position - touch1.position;
            
            float prevMagnitude = prevDelta.magnitude;
            float currMagnitude = currDelta.magnitude;
            
            float delta = currMagnitude - prevMagnitude;
            
            if (Mathf.Abs(delta) > deadZoneThreshold)
            {
                OnZoomGesture?.Invoke(new Vector2(0, delta * zoomSensitivity));
            }
        }
    }
}
