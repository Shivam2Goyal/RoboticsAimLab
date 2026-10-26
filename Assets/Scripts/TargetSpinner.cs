using UnityEngine;

public class TargetSpinner : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    
    void Update()
    {
        float angleStep = rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, angleStep, 0f);
    }
}
