using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;


    private void Start()
    {
        offset = transform.position - target.position;
    }
    private void LateUpdate() //is smoother if camera moves after the player
    {
        transform.position = target.position + offset;
    }

}
