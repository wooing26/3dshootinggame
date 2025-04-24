using UnityEngine;

public class MinimapZoom : MonoBehaviour
{
    public Camera MinimapCamera;
    public int MaxZoomOut = 15;
    public int MaxZoomIn = 5;

    public void MinimapZoomIn()
    {
        MinimapCamera.orthographicSize = Mathf.Max(MinimapCamera.orthographicSize - 1, MaxZoomIn);
    }

    public void MinimapZoomOut()
    {
        MinimapCamera.orthographicSize = Mathf.Min(MinimapCamera.orthographicSize + 1, MaxZoomOut);
    }
}
