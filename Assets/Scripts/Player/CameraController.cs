using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;


    private void Start()
    {
        offset = new Vector3(0, 2, -5);
    }
    private void LateUpdate() //is smoother if camera moves after the player
    {
        transform.position = target.position + target.rotation * offset;
        transform.rotation = target.rotation;
    }

}
