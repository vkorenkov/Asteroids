using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1)] float shootTimerValue;
    SpaceShipControl spaceShipControl;
    float horizontalAxis;
    float verticalAxis;
    float shootTimer;
    public bool canShoot = true;

    private void Awake()
    {
        spaceShipControl = GetComponent<SpaceShipControl>();
        shootTimer = shootTimerValue;
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        shootTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && shootTimer <= 0 && canShoot)
        {
            shootTimer = shootTimerValue;
            spaceShipControl.Shoot();
            GameScript.soundController.PlaySound("laserShoot");
        }
    }

    private void FixedUpdate()
    {
        spaceShipControl.MoveControl(horizontalAxis, verticalAxis);
    }
}
