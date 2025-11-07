using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScreenshotManager : MonoBehaviour
{
    // Create a class to hold device resolution and name
    [System.Serializable]
    public class DeviceResolution
    {
        public string deviceName;
        public Vector2Int resolution;
        public bool capture;  // Add a flag to determine if the device should be captured

        public DeviceResolution(string name, int width, int height, bool shouldCapture = true)
        {
            deviceName = name;
            resolution = new Vector2Int(width, height);
            capture = shouldCapture;
        }
    }

    // Use a list of devices with their resolutions and names
    [SerializeField] private List<DeviceResolution> devices = new List<DeviceResolution>
    {
        new DeviceResolution("Apple Store UPhone 6.5 Display", 1242, 2688),
        new DeviceResolution("Apple Store iPad 13inch", 2064, 2752),
    };

    // 👇 Add this variable to set the file name prefix
    [SerializeField] private string fileNamePrefix = "Screenshot_";

    // Canvas that contains UI elements you want to simulate safe area on
    [SerializeField] private Canvas canvas;

    [Button("Capture All Screenshots")]
    public void CaptureAllScreenshots()
    {
        StartCoroutine(CaptureScreenshotsCoroutine());
    }

    private IEnumerator CaptureScreenshotsCoroutine()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        Directory.CreateDirectory(folderPath);

        foreach (var device in devices)
        {
            // Skip horizontal devices (wider than tall)
            if (device.resolution.x > device.resolution.y)
            {
                continue; // Ignore horizontal devices for now
            }

            // Only capture the selected devices
            if (!device.capture)
            {
                continue;
            }

            yield return new WaitForEndOfFrame();

            // Set resolution for the current device
            Screen.SetResolution(device.resolution.x, device.resolution.y, false);

            // Wait for the screen to rerender after the resolution change
            yield return new WaitForEndOfFrame(); // Wait until screen rendering is done

            // Force a Canvas update to reflect the new resolution and safe area
            Canvas.ForceUpdateCanvases();  // Force an immediate canvas update

            // Apply safe area simulation to the UI canvas
            ApplySafeArea();

            // Capture screenshot with device name and resolution
            string filePath = Path.Combine(folderPath, $"{fileNamePrefix}{device.deviceName}_{device.resolution.x}x{device.resolution.y}.png");
            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log($"Saved: {filePath}");

            // Wait a frame to avoid issues
            yield return new WaitForSeconds(0.5f);  // Small delay to ensure screen is captured properly
        }

        Debug.Log("✅ All selected screenshots captured!");
    }

    // Applies safe area to the canvas before capturing the screenshot
    private void ApplySafeArea()
    {
        if (canvas != null)
        {
            // Get the safe area for the device
            Rect safeArea = Screen.safeArea;

            // Convert the safe area to local space of the canvas
            Vector2 anchorMin = new Vector2(safeArea.x / Screen.width, safeArea.y / Screen.height);
            Vector2 anchorMax = new Vector2((safeArea.x + safeArea.width) / Screen.width, (safeArea.y + safeArea.height) / Screen.height);

            // Apply the anchor values to the canvas RectTransform
            RectTransform rt = canvas.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero; // Let the canvas size be defined by the safe area
        }
    }
}
