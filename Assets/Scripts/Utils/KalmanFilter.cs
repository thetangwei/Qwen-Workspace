using UnityEngine;

/// <summary>
/// 卡尔曼滤波器 - 用于平滑手部追踪数据，减少抖动
/// </summary>
public class KalmanFilter
{
    private float estimatedValue;
    private float errorEstimation;
    private float errorMeasurement;
    private float processNoise;
    
    public KalmanFilter(float initialValue, float processNoise = 0.001f, float measurementNoise = 0.1f)
    {
        estimatedValue = initialValue;
        errorEstimation = 1.0f;
        this.processNoise = processNoise;
        this.errorMeasurement = measurementNoise;
    }
    
    /// <summary>
    /// Update filter with new measurement
    /// </summary>
    public float Update(float measurement)
    {
        // Prediction step
        errorEstimation += processNoise;
        
        // Update step
        float kalmanGain = errorEstimation / (errorEstimation + errorMeasurement);
        estimatedValue = estimatedValue + kalmanGain * (measurement - estimatedValue);
        errorEstimation = (1 - kalmanGain) * errorEstimation;
        
        return estimatedValue;
    }
    
    /// <summary>
    /// Reset filter to initial state
    /// </summary>
    public void Reset(float newValue)
    {
        estimatedValue = newValue;
        errorEstimation = 1.0f;
    }
}

/// <summary>
/// 2D 向量卡尔曼滤波器
/// </summary>
public class KalmanFilter2D
{
    private KalmanFilter xFilter;
    private KalmanFilter yFilter;
    
    public KalmanFilter2D(Vector2 initialValue, float processNoise = 0.001f, float measurementNoise = 0.1f)
    {
        xFilter = new KalmanFilter(initialValue.x, processNoise, measurementNoise);
        yFilter = new KalmanFilter(initialValue.y, processNoise, measurementNoise);
    }
    
    /// <summary>
    /// Update filter with new measurement
    /// </summary>
    public Vector2 Update(Vector2 measurement)
    {
        float newX = xFilter.Update(measurement.x);
        float newY = yFilter.Update(measurement.y);
        
        return new Vector2(newX, newY);
    }
    
    /// <summary>
    /// Reset filter to initial state
    /// </summary>
    public void Reset(Vector2 newValue)
    {
        xFilter.Reset(newValue.x);
        yFilter.Reset(newValue.y);
    }
}

/// <summary>
/// 手势数据平滑工具类
/// </summary>
public static class GestureSmoothing
{
    private static KalmanFilter2D positionFilter;
    private static KalmanFilter pinchFilter;
    
    private const float PROCESS_NOISE = 0.001f;
    private const float MEASUREMENT_NOISE = 0.1f;
    
    /// <summary>
    /// Initialize smoothing filters
    /// </summary>
    public static void Initialize(Vector2 initialPosition, float initialPinch = 0f)
    {
        positionFilter = new KalmanFilter2D(initialPosition, PROCESS_NOISE, MEASUREMENT_NOISE);
        pinchFilter = new KalmanFilter(initialPinch, PROCESS_NOISE, MEASUREMENT_NOISE);
    }
    
    /// <summary>
    /// Smooth hand position data
    /// </summary>
    public static Vector2 SmoothPosition(Vector2 position)
    {
        if (positionFilter == null)
            Initialize(position);
            
        return positionFilter.Update(position);
    }
    
    /// <summary>
    /// Smooth pinch distance data
    /// </summary>
    public static float SmoothPinch(float pinchDistance)
    {
        if (pinchFilter == null)
            Initialize(Vector2.zero, pinchDistance);
            
        return pinchFilter.Update(pinchDistance);
    }
    
    /// <summary>
    /// Reset all filters
    /// </summary>
    public static void Reset()
    {
        positionFilter = null;
        pinchFilter = null;
    }
}
