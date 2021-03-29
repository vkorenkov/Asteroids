using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    public delegate void CrushDelegate();
    public static event CrushDelegate CrushEvent;

    [SerializeField, Range(100, 300)] float rotationSpeed = 100;
    [SerializeField] bool isGodMode;
    [SerializeField] Transform startBulletPosition;
    [SerializeField] GameObject bulletGO;
    [SerializeField] ParticleSystem crashEffect;
    Rigidbody2D spaceShipRb;
    float spaceShipSpeed = 4;
    Transform bulletsParent;

    Vector3 shipPosition;
    Vector3 ShipPosition
    {
        get => transform.position;
        set { shipPosition = value; transform.position = shipPosition; }
    }

    Vector2 spaceShipRbVelocity
    {
        get => spaceShipRb.velocity;
    }

    void Awake()
    {
        bulletsParent = GameObject.Find("Bullets").GetComponent<Transform>();
        spaceShipRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ShipPosition = PositionScript.CheckPosition(ShipPosition);
    }

    public void MoveControl(float horizontalAxis, float verticalAxis)
    {
        transform.Rotate(0, 0, -horizontalAxis * rotationSpeed * Time.deltaTime);
        spaceShipRb.AddForce(verticalAxis * spaceShipSpeed * transform.up);
        spaceShipRb.velocity = new Vector2(Mathf.Clamp(spaceShipRbVelocity.x, -spaceShipSpeed, spaceShipSpeed), Mathf.Clamp(spaceShipRbVelocity.y, -spaceShipSpeed, spaceShipSpeed));
    }

    public void Shoot()
    {
        Instantiate(bulletGO, startBulletPosition.position, transform.rotation, bulletsParent);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGodMode)
        {
            if (collision.gameObject.tag.ToLower().Contains("asteroid"))
            {
                crashEffect.Play();
                GetComponent<Inputs>().canShoot = false;
                GetComponent<PolygonCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject, 1f);

                CrushEvent?.Invoke();
            }
        }
    }
}
