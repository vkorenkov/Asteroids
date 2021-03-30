using UnityEngine;

public class AsteroidsScript : MonoBehaviour
{
    /// <summary>
    /// Делегат создания астероида
    /// </summary>
    /// <param name="isBig"></param>
    /// <param name="position"></param>
    public delegate void AsteriodSpawnDel(bool isBig, Vector2 position);
    /// <summary>
    /// Событие создания астероида
    /// </summary>
    public static event AsteriodSpawnDel AsteriodSpawnEvent;
    /// <summary>
    /// Эффект взрыва астероида
    /// </summary>
    [Tooltip("Поле для системы частитц имитации взрыва астероида")]
    [SerializeField] ParticleSystem destroyEffect;

    Vector3 asteroidPosition;
    /// <summary>
    /// Свойство текущей позиции астероида
    /// </summary>
    Vector3 AsteroidPosition
    {
        get => transform.position;
        set { asteroidPosition = value; transform.position = asteroidPosition; }
    }
    
    private void Start()
    {
        // Получение RigitBody2D с объекта астероида
        var rb = GetComponent<Rigidbody2D>();
        // Случайное направление движения астероида
        rb.AddForce(new Vector2(Random.Range(-200, 200), Random.Range(-200, 200)));
        // Случайное направление вращения астероида
        rb.AddTorque(Random.Range(-2, 2), ForceMode2D.Impulse); 

    }

    private void Update()
    {
        // Проверка позиции астероида
        AsteroidPosition = PositionScript.CheckPosition(AsteroidPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка что столкновение произошло со снарядом и типа астероида
        if (collision.gameObject.tag.ToLower() == "bullet" && transform.tag.ToLower() == "bigasteroid")
        {
            // Создание случайного количества малых астероидов
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                // Вызов события создания малого астероида
                AsteriodSpawnEvent?.Invoke(false, transform.position);
            }

            RemoveAsteroid();
        }
        else if(collision.gameObject.tag.ToLower() == "bullet" && transform.tag.ToLower() == "smallasteroid")
        {
            RemoveAsteroid();
        }
    }

    /// <summary>
    /// Уничтождение астероида
    /// </summary>
    void RemoveAsteroid()
    {
        // Уничтодение астероида через 1 секунду
        Destroy(gameObject, 1);
        // Запуск эффекта взрыва астероида
        destroyEffect.Play();
        // Отключение колайдера астероида, для предотвращения взаимодействий
        GetComponent<PolygonCollider2D>().enabled = false;
        // Отключение отображения астероида
        GetComponent<SpriteRenderer>().enabled = false;
        // Удаление астероида из коллекции астероидов
        GameScript.asteroids.Remove(gameObject);
    }
}
