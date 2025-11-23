using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform character;
    public float speed;

    void Start()
    {
        //
    }

    void Update()
    {
        float lerpX = Mathf.Lerp(transform.position.x, character.position.x, speed + Time.deltaTime);
        Vector3 v = transform.position;
        v.x = lerpX;
        transform.position = v;
    }
}