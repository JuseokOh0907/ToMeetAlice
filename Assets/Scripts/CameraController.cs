using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public GameObject Player; // Drag the Player object here
    public PlayerState PlayerState;

    [Header("Follow")]
    public float cameraSpeed = 3.0f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Smooth Zoom & Vignette")]
    public float minSize = 2.0f;
    public float maxSize = 3.5f;
    public float normalVignette = 0.7f;
    public float glassesVignette = 0.4f;
    public float lerpSpeed = 2.0f;

    private Camera cam;
    private Vignette vignette;
    public Volume globalVolume;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (globalVolume != null) globalVolume.profile.TryGet(out vignette);
        vignette.intensity.value = normalVignette;
    }

    private void LateUpdate()
    {
        if (Player == null) return;

        // 1. Follow Player
        transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, cameraSpeed * Time.deltaTime);

        // 2. Read state from PlayerState.cs and Lerp
        float targetSize = PlayerState.isWearingGlasses ? maxSize : minSize;
        float targetVignette = PlayerState.isWearingGlasses ? glassesVignette : normalVignette;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, lerpSpeed * Time.deltaTime);

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, lerpSpeed * Time.deltaTime);
        }
    }
}