using UnityEngine;

/// <summary>
/// 地球着色器控制器 - 管理地球的材质和着色器效果
/// 支持日间/夜间模式切换、云层动画、大气散射等效果
/// </summary>
[RequireComponent(typeof(Renderer))]
public class EarthShaderController : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;
    [SerializeField] private Material cloudMaterial;
    [SerializeField] private Material atmosphereMaterial;
    
    [Header("Shader Properties")]
    [SerializeField] private string blendPropertyName = "_BlendFactor";
    [SerializeField] private string cloudSpeedPropertyName = "_CloudSpeed";
    [SerializeField] private string atmosphereIntensityPropertyName = "_AtmosphereIntensity";
    
    [Header("Animation Settings")]
    [SerializeField] private float dayNightTransitionSpeed = 1.0f;
    [SerializeField] private float cloudSpeed = 0.01f;
    [SerializeField] private float atmosphereIntensity = 1.0f;
    
    [Header("Lighting")]
    [SerializeField] private Light sunLight;
    [SerializeField] private Gradient daySkyColor;
    [SerializeField] private Gradient nightSkyColor;
    
    private Renderer earthRenderer;
    private Renderer cloudRenderer;
    private Renderer atmosphereRenderer;
    private float currentBlendFactor = 0f; // 0 = day, 1 = night
    private bool isTransitioning = false;
    private TargetMode currentTargetMode = TargetMode.Day;
    
    private enum TargetMode { Day, Night }
    
    private void Awake()
    {
        earthRenderer = GetComponent<Renderer>();
        
        // Find child renderers
        Transform clouds = transform.Find("Clouds");
        if (clouds != null)
            cloudRenderer = clouds.GetComponent<Renderer>();
            
        Transform atmosphere = transform.Find("Atmosphere");
        if (atmosphere != null)
            atmosphereRenderer = atmosphere.GetComponent<Renderer>();
    }
    
    private void Update()
    {
        AnimateClouds();
        
        if (isTransitioning)
        {
            UpdateDayNightTransition();
        }
    }
    
    /// <summary>
    /// Transition to day mode
    /// </summary>
    public void SetDayMode()
    {
        if (currentTargetMode != TargetMode.Day)
        {
            currentTargetMode = TargetMode.Day;
            isTransitioning = true;
        }
    }
    
    /// <summary>
    /// Transition to night mode
    /// </summary>
    public void SetNightMode()
    {
        if (currentTargetMode != TargetMode.Night)
        {
            currentTargetMode = TargetMode.Night;
            isTransitioning = true;
        }
    }
    
    /// <summary>
    /// Update day/night transition
    /// </summary>
    private void UpdateDayNightTransition()
    {
        float targetValue = currentTargetMode == TargetMode.Day ? 0f : 1f;
        
        currentBlendFactor = Mathf.MoveTowards(
            currentBlendFactor, 
            targetValue, 
            dayNightTransitionSpeed * Time.deltaTime
        );
        
        // Update shader property
        if (earthRenderer != null && earthRenderer.material.HasProperty(blendPropertyName))
        {
            earthRenderer.material.SetFloat(blendPropertyName, currentBlendFactor);
        }
        
        // Update sky color
        UpdateSkyColor();
        
        // Check if transition complete
        if (Mathf.Approximately(currentBlendFactor, targetValue))
        {
            isTransitioning = false;
        }
    }
    
    /// <summary>
    /// Animate cloud movement
    /// </summary>
    private void AnimateClouds()
    {
        if (cloudRenderer != null && cloudRenderer.material.HasProperty("_MainTex_ST"))
        {
            Vector2 offset = cloudRenderer.material.GetTextureOffset("_MainTex");
            offset.x += cloudSpeed * Time.deltaTime;
            cloudRenderer.material.SetTextureOffset("_MainTex", offset);
        }
    }
    
    /// <summary>
    /// Update sky color based on day/night blend
    /// </summary>
    private void UpdateSkyColor()
    {
        if (sunLight == null)
            return;
        
        Color dayColor = daySkyColor.Evaluate(0.5f);
        Color nightColor = nightSkyColor.Evaluate(0.5f);
        
        Color blendedColor = Color.Lerp(dayColor, nightColor, currentBlendFactor);
        
        // Update ambient light
        RenderSettings.ambientLight = blendedColor;
        
        // Update sun intensity
        if (sunLight != null)
        {
            sunLight.intensity = Mathf.Lerp(1.0f, 0.0f, currentBlendFactor);
        }
    }
    
    /// <summary>
    /// Set atmosphere glow intensity
    /// </summary>
    public void SetAtmosphereIntensity(float intensity)
    {
        atmosphereIntensity = Mathf.Clamp01(intensity);
        
        if (atmosphereRenderer != null && 
            atmosphereRenderer.material.HasProperty(atmosphereIntensityPropertyName))
        {
            atmosphereRenderer.material.SetFloat(
                atmosphereIntensityPropertyName, 
                atmosphereIntensity
            );
        }
    }
    
    /// <summary>
    /// Set cloud animation speed
    /// </summary>
    public void SetCloudSpeed(float speed)
    {
        cloudSpeed = speed;
        
        if (cloudRenderer != null && cloudRenderer.material.HasProperty(cloudSpeedPropertyName))
        {
            cloudRenderer.material.SetFloat(cloudSpeedPropertyName, cloudSpeed);
        }
    }
    
    /// <summary>
    /// Get current day/night blend factor
    /// </summary>
    public float GetBlendFactor()
    {
        return currentBlendFactor;
    }
    
    /// <summary>
    /// Force immediate mode switch (no transition)
    /// </summary>
    public void ForceMode(bool isDay)
    {
        currentBlendFactor = isDay ? 0f : 1f;
        currentTargetMode = isDay ? TargetMode.Day : TargetMode.Night;
        isTransitioning = false;
        
        if (earthRenderer != null && earthRenderer.material.HasProperty(blendPropertyName))
        {
            earthRenderer.material.SetFloat(blendPropertyName, currentBlendFactor);
        }
        
        UpdateSkyColor();
    }
}
