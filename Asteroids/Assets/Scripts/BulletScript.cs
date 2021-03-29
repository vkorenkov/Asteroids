using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public delegate void PointCountDelegate(int pointCount);
    public static event PointCountDelegate PointCountEvent;

    [SerializeField, Range(1, 20)] float bulletSpeed;

    Vector2 BulletPosition
    {
        get => Camera.main.WorldToScreenPoint(transform.position);
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
    }

    private void Update()
    {
        bool isBulletThroughWidth = BulletPosition.x > Screen.width || BulletPosition.x < 0;
        bool isBulletThroughHeight = BulletPosition.y > Screen.height || BulletPosition.y < 0;

        if (isBulletThroughWidth || isBulletThroughHeight)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.ToLower().Contains("asteroid"))
        {
            int points = collision.transform.tag.ToLower() == "bigasteroid" ? 5 : 10;

            Destroy(gameObject);

            PointCountEvent?.Invoke(points);
        }
    }
}
