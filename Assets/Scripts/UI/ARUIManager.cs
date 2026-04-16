using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AR UI 管理器 - 管理 AR 场景中的 UI 元素
/// </summary>
public class ARUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject poiInfoPanel;
    
    [Header("Control Panel Elements")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button viewModeButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Text viewModeText;
    
    [Header("POI Info Panel Elements")]
    [SerializeField] private Text poiNameText;
    [SerializeField] private Text poiDescriptionText;
    [SerializeField] private Button closePoiButton;
    
    [Header("References")]
    [SerializeField] private EarthController earthController;
    
    private EarthController.ViewMode currentViewMode = EarthController.ViewMode.Day;
    private string[] viewModeNames = { "日间", "夜景", "地形" };
    private int currentViewModeIndex = 0;
    
    private void Awake()
    {
        if (earthController == null)
            earthController = FindObjectOfType<EarthController>();
    }
    
    private void Start()
    {
        SetupButtons();
        ShowLoading(true);
        
        // Auto-hide loading after delay
        Invoke(nameof(HideLoading), 2f);
        
        // Show instructions initially
        ShowInstructions(true);
    }
    
    private void SetupButtons()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
            
        if (viewModeButton != null)
            viewModeButton.onClick.AddListener(OnViewModeButtonClicked);
            
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonClicked);
            
        if (closePoiButton != null)
            closePoiButton.onClick.AddListener(OnClosePoiButtonClicked);
    }
    
    #region Panel Visibility
    
    public void ShowLoading(bool show)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(show);
    }
    
    public void HideLoading()
    {
        ShowLoading(false);
    }
    
    public void ShowInstructions(bool show)
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(show);
    }
    
    public void ToggleInstructions()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(!instructionPanel.activeSelf);
    }
    
    public void ShowControls(bool show)
    {
        if (controlPanel != null)
            controlPanel.SetActive(show);
    }
    
    public void ShowPOIInfo(bool show)
    {
        if (poiInfoPanel != null)
            poiInfoPanel.SetActive(show);
    }
    
    #endregion
    
    #region Button Handlers
    
    private void OnPauseButtonClicked()
    {
        if (earthController != null)
        {
            earthController.TogglePause();
            UpdatePauseButtonText();
        }
    }
    
    private void OnViewModeButtonClicked()
    {
        currentViewModeIndex = (currentViewModeIndex + 1) % viewModeNames.Length;
        currentViewMode = (EarthController.ViewMode)currentViewModeIndex;
        
        if (earthController != null)
        {
            earthController.SetViewMode(currentViewMode);
        }
        
        if (viewModeText != null)
        {
            viewModeText.text = viewModeNames[currentViewModeIndex];
        }
    }
    
    private void OnResetButtonClicked()
    {
        if (earthController != null)
        {
            earthController.ResetPosition();
        }
    }
    
    private void OnClosePoiButtonClicked()
    {
        ShowPOIInfo(false);
    }
    
    private void UpdatePauseButtonText()
    {
        if (pauseButton != null && earthController != null)
        {
            Text buttonText = pauseButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = earthController.IsPaused ? "继续" : "暂停";
            }
        }
    }
    
    #endregion
    
    #region POI Display
    
    public void DisplayPOIInfo(string name, string description)
    {
        if (poiNameText != null)
            poiNameText.text = name;
            
        if (poiDescriptionText != null)
            poiDescriptionText.text = description;
            
        ShowPOIInfo(true);
    }
    
    #endregion
    
    private void OnDestroy()
    {
        // Cleanup button listeners
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(OnPauseButtonClicked);
            
        if (viewModeButton != null)
            viewModeButton.onClick.RemoveListener(OnViewModeButtonClicked);
            
        if (resetButton != null)
            resetButton.onClick.RemoveListener(OnResetButtonClicked);
            
        if (closePoiButton != null)
            closePoiButton.onClick.RemoveListener(OnClosePoiButtonClicked);
    }
}
