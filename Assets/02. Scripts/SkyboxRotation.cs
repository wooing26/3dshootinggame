using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float RotationSpeed = 10f;

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotationSpeed);
    }
}
