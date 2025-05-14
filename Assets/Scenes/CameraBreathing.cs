using UnityEngine;

public class CameraBreathing : MonoBehaviour
{
    public float radius = 0.02f;     // Radius of circular movement
    public float speed = 0.5f;       // Speed of breathing movement

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float xOffset = Mathf.Cos(Time.time * speed * 2 * Mathf.PI) * radius;
        float yOffset = Mathf.Sin(Time.time * speed * 2 * Mathf.PI) * radius;

        transform.localPosition = initialPosition + new Vector3(xOffset, yOffset, 0f);
    }
}
