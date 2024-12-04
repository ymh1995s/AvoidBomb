using UnityEngine;

public class LookCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(transform.position );
    }
}
