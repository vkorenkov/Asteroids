using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    /// <summary>
    /// Делегат уничтожения корабля
    /// </summary>
    public delegate void CrushDelegate();
    /// <summary>
    /// Событие уничтодения корабля
    /// </summary>
    public static event CrushDelegate CrushEvent;
    /// <summary>
    /// Скорость поворота корабля
    /// </summary>
    [Tooltip("Установка скорости поворота корабля (100 - 300)")]
    [SerializeField, Range(100, 300)] float rotationSpeed = 100;
    /// <summary>
    /// Режим бессмертия корабля
    /// </summary>
    [Tooltip("Установка режима бессмертия")]
    [SerializeField] bool isGodMode;
    /// <summary>
    /// Стартовая позиция снаряда
    /// </summary>
    [Tooltip("Поле для стартовой позиции снаряда")]
    [SerializeField] Transform startBulletPosition;
    /// <summary>
    /// Объект снаряда для клонирования
    /// </summary>
    [Tooltip("Поле для префаба снаряда")]
    [SerializeField] GameObject bulletGO;
    /// <summary>
    /// Эффект уничтожения корабля
    /// </summary>
    [Tooltip("Поле для системы частиц имитации взрыва корабля")]
    [SerializeField] ParticleSystem crashEffect;
    /// <summary>
    /// Твердое тело корабля
    /// </summary>
    Rigidbody2D spaceShipRb;
    /// <summary>
    /// Скорость корабля
    /// </summary>
    float spaceShipSpeed = 4;
    /// <summary>
    /// Родительский объект для хранения объектов снаряда
    /// </summary>
    [Tooltip("Поле для родительского объекта снарядов")]
    [SerializeField] Transform bulletsParent;

    Vector3 shipPosition;
    /// <summary>
    /// Свойство текущей позиции корабля
    /// </summary>
    Vector3 ShipPosition
    {
        get => transform.position;
        set { shipPosition = value; transform.position = shipPosition; }
    }
    /// <summary>
    /// Свойство текущей скорости корабля
    /// </summary>
    Vector2 spaceShipRbVelocity
    {
        get => spaceShipRb.velocity;
    }

    void Awake()
    {
        // Инициализация твердого тела корабля
        spaceShipRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Проверка позиции корабля
        ShipPosition = PositionScript.CheckPosition(ShipPosition);
    }

    public void MoveControl(float horizontalAxis, float verticalAxis)
    {
        // Поворот корабля
        transform.Rotate(0, 0, -horizontalAxis * rotationSpeed * Time.deltaTime);
        // Движение корабля
        spaceShipRb.AddForce(verticalAxis * spaceShipSpeed * transform.up);
        // Ограничение максимальной скорости корабля
        spaceShipRb.velocity = new Vector2(Mathf.Clamp(spaceShipRbVelocity.x, -spaceShipSpeed, spaceShipSpeed), Mathf.Clamp(spaceShipRbVelocity.y, -spaceShipSpeed, spaceShipSpeed));
    }

    public void Shoot()
    {
        // Создание снаряда
        Instantiate(bulletGO, startBulletPosition.position, transform.rotation, bulletsParent);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGodMode)
        {
            // проверка на столкновение с астероидом
            if (collision.gameObject.tag.ToLower().Contains("asteroid"))
            {
                // Воспроизведение эффекта уничтожения корабля
                crashEffect.Play();
                // Отключение возможности стрельбы
                GetComponent<Inputs>().canShoot = false;
                // Отключение взаимодействия коробля с окружением
                GetComponent<PolygonCollider2D>().enabled = false;
                // Отключение отображения коробля
                GetComponent<SpriteRenderer>().enabled = false;
                // Уничтодение объекта коробля через 1 секунду
                Destroy(gameObject, 1f);
                // Вызов события уничтоджения корабля
                CrushEvent?.Invoke();
            }
        }
    }
}
