using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{

    private Rigidbody rb;
    private float speed = 7f;
    private float jumpForce = 7f;
    private int health = 100;
    private bool isAlive = true;

    float groundRayLength = 0.3f;
    private LayerMask floorLayer;

    public event Action OnDead;
    public event Action<int> OnKillsUpdated;
    public event Action<int> OnDeathsUpdated;

    public int playerKills { get; private set; } = 0;
    public int playerDeaths { get; private set; } = 0;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        floorLayer = LayerMask.GetMask("Floor");

        //Load Data
        PlayerData data = DataManager.Instance.LoadData();
        playerKills = data.playerScore;
        playerDeaths = data.playerDeaths;
        OnKillsUpdated.Invoke(playerKills);
        OnDeathsUpdated.Invoke(playerDeaths);
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

        //rotation (following the camera)
        transform.rotation = Quaternion.Euler(0f, Camera.main.GetComponent<CameraController>().rotationX, 0f);

        //jump
        if (InputManager.Instance.GetInput(InputCommand.Jump) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    #endregion


    public void RegisterKill()
    {
        playerKills++;
        PlayerData data = DataManager.Instance.LoadData();
        data.playerScore = playerKills;
        DataManager.Instance.SaveData(data);
        OnKillsUpdated.Invoke(playerKills);
    }

    public void RegisterDeath()
    {
        playerDeaths++;
        PlayerData data = DataManager.Instance.LoadData();
        data.playerDeaths = playerDeaths;
        DataManager.Instance.SaveData(data);
        OnDeathsUpdated.Invoke(playerDeaths);
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
