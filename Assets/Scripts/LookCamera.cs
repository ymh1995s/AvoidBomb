using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(transform.position + Camera.main.transform.forward);
        transform.LookAt(transform.position );
    }
}
