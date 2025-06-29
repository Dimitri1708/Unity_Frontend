using Dto_s;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 10f;

    float minX, maxX, minY, maxY;

    void Start()
    {
        float halfWidth = Environment.environmentXScale / 2f;
        float halfHeight = Environment.environmentYScale / 2f;

        minX = -halfWidth;
        maxX = halfWidth;
        minY = -halfHeight;
        maxY = halfHeight;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move.y += 1;
        if (Input.GetKey(KeyCode.S)) move.y -= 1;
        if (Input.GetKey(KeyCode.A)) move.x -= 1;
        if (Input.GetKey(KeyCode.D)) move.x += 1;

        move = move.normalized * moveSpeed * Time.deltaTime;
        transform.position += move;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
