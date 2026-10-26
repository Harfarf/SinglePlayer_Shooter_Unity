using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{

    private Rigidbody rb;
    private float speed = 7f;
    private float rotationSpeed = 100f;
    private int health = 100;
    private bool isAlive = true;

    public event Action OnDead;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        MovePlayer();
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
    }


    #region IDamageable
    public void SetDamage(int damageAmount)
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
