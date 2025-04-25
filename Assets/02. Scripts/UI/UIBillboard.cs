using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    private Transform _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(_mainCamera);
        transform.Rotate(0, 180, 0);
    }

}
