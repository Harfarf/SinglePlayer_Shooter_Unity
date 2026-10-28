using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;
    private float rotationX, rotationY;
    private float turnSpeedX = 15f, turnSpeedY = 3f;


    private void Start()
    {
        offset = new Vector3(0, 2, -5);
    }
    private void LateUpdate() //is smoother if camera moves after the player
    {
        rotationX += InputManager.Instance.GetAxis(InputCommand.LookX) * turnSpeedX * Time.deltaTime;
        rotationY -= InputManager.Instance.GetAxis(InputCommand.LookY) * turnSpeedY * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        transform.position = target.position + rotation * offset;
        transform.rotation = rotation;
    }

}
