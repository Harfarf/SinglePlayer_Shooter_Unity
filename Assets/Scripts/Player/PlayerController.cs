using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{

    private Rigidbody rb;
    private float speed = 7f;
    private float rotationSpeed = 100f;
    private float jumpForce = 7f;
    private int health = 100;
    private bool isAlive = true;

    float groundRayLength = 0.3f;
    private LayerMask floorLayer;

    public event Action OnDead;

    public int PlayerKills { get; private set; } = 0;
    public int PlayerDeaths { get; private set; } = 0;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        floorLayer = LayerMask.GetMask("Floor");
    }
    private void Update()
    {
        MovePlayer();
    }



    #region Movement
    bool IsGrounded()
    {
        Ray groundCheckRay = new Ray(this.transform.position, Vector3.down);
        return Physics.Raycast(groundCheckRay, groundRayLength, floorLayer);
    }
    private void MovePlayer()
    {
        //position
        Vector3 movementDirection = InputManager.Instance.GetAxis(InputCommand.MovementX) * Camera.main.transform.right 
            + InputManager.Instance.GetAxis(InputCommand.MovementY) * Camera.main.transform.forward;
        rb.linearVelocity = movementDirection * speed; //units per second - consistent between devices

        //rotation
        if (rb.linearVelocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rb.linearVelocity, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime); //rotation speed consistent across frame rates
            rb.angularVelocity = Vector3.zero;
        }
        else rb.angularVelocity = Vector3.zero;

        //jump
        if (InputManager.Instance.GetInput(InputCommand.Jump) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    #endregion


    public void RegisterKill()
    {
        PlayerKills++;
        PlayerData data = DataManager.Instance.LoadData();
        data.playerScore = PlayerKills;
        DataManager.Instance.SaveData(data);
    }

    public void RegisterDeath()
    {
        PlayerDeaths++;
        PlayerData data = DataManager.Instance.LoadData();
        data.playerDeaths = PlayerDeaths;
        DataManager.Instance.SaveData(data);
    }


    #region IDamageable
    public void SetDamage(int damageAmount, GameObject attacker)
    {
        health -= damageAmount;
        if (health <= 0) OnDie();
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public int GetHealth()
    {
        return health;
    }
    public void OnDie()
    {
        if (isAlive)
        {
            RegisterDeath();
            isAlive = false;
            OnDead.Invoke();
            Debug.Log("---END GAME---");
        }
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion
}
