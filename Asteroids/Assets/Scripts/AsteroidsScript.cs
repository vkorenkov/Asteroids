using UnityEngine;

public class AsteroidsScript : MonoBehaviour
{
    public delegate void AsteriodSpawnDel(bool isBig, Vector2 position);
    public static event AsteriodSpawnDel AsteriodSpawnEvent;

    [SerializeField] ParticleSystem destroyEffect;

    Vector3 asteroidPosition;
    Vector3 AsteroidPosition
    {
        get => transform.position;
        set { asteroidPosition = value; transform.position = asteroidPosition; }
    }
    
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
    }

    private void Update()
    {
        AsteroidPosition = PositionScript.CheckPosition(AsteroidPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.ToLower() == "bullet" && transform.tag.ToLower() == "bigasteroid")
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                AsteriodSpawnEvent?.Invoke(false, transform.position);
            }

            RemoveAsteroid();
        }
        else if(collision.gameObject.tag.ToLower() == "bullet" && transform.tag.ToLower() == "smallasteroid")
        {
            RemoveAsteroid();
        }
    }

    void RemoveAsteroid()
    {
        Destroy(gameObject, 1);
        destroyEffect.Play();
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        GameScript.asteroids.Remove(gameObject);
    }
}
