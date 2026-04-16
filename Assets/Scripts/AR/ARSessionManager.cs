using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

/// <summary>
/// AR会话管理器 - 负责AR Foundation的初始化和配置
/// </summary>
public class ARSessionManager : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    
    [Header("Settings")]
    [SerializeField] private bool requireHorizontalPlane = true;
    [SerializeField] private float planeDetectionTimeout = 30f;
    
    private bool isSessionReady = false;
    private float sessionStartTime;
    
    public bool IsSessionReady => isSessionReady;
    public ARRaycastManager RaycastManager => raycastManager;
    
    private void Awake()
    {
        sessionStartTime = Time.time;
        
        if (arSession == null)
            arSession = FindObjectOfType<ARSession>();
            
        if (raycastManager == null)
            raycastManager = FindObjectOfType<ARRaycastManager>();
            
        if (planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();
    }
    
    private void OnEnable()
    {
        if (arSession != null)
        {
            arSession.stateChanged += OnARSessionStateChanged;
        }
    }
    
    private void OnDisable()
    {
        if (arSession != null)
        {
            arSession.stateChanged -= OnARSessionStateChanged;
        }
    }
    
    private void OnARSessionStateChanged(ARSessionStateChangedEventArgs args)
    {
        Debug.Log($"AR Session State Changed: {args.state}");
        
        switch (args.state)
        {
            case ARSessionState.SessionTracking:
                isSessionReady = true;
                Debug.Log("AR Session is ready!");
                break;
                
            case ARSessionState.SessionIdle:
            case ARSessionState.SessionStopped:
                isSessionReady = false;
                break;
                
            case ARSessionState.Error:
                Debug.LogError("AR Session encountered an error!");
                break;
        }
    }
    
    private void Update()
    {
        // Check for plane detection timeout
        if (!isSessionReady && Time.time - sessionStartTime > planeDetectionTimeout)
        {
            Debug.LogWarning("Plane detection timeout. Please try again in a better environment.");
        }
    }
    
    /// <summary>
    /// Reset AR session
    /// </summary>
    public void ResetSession()
    {
        if (arSession != null)
        {
            arSession.Reset();
        }
    }
}
