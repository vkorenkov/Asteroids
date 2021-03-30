using UnityEngine;

public class BulletScript : MonoBehaviour
{
    /// <summary>
    /// Делегат начисления очков
    /// </summary>
    /// <param name="pointCount"></param>
    public delegate void PointCountDelegate(int pointCount);
    /// <summary>
    /// Событие начисления очков
    /// </summary>
    public static event PointCountDelegate PointCountEvent;
    /// <summary>
    /// Скорость полета снаряда
    /// </summary>
    [Tooltip("Скорость снаряда (1 - 20)")]
    [SerializeField, Range(1, 20)] float bulletSpeed;
    /// <summary>
    /// Свойство позиции снаряда
    /// </summary>
    Vector2 BulletPosition
    {
        // Конвертирование координат из глобальных к координатам обзора камеры
        get => Camera.main.WorldToScreenPoint(transform.position);
    }

    void Start()
    {
        // Придание скорости снаряда при появлении
        GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
    }

    private void Update()
    {
        
        bool isBulletThroughWidth = BulletPosition.x > Screen.width || BulletPosition.x < 0;   // Проверка положения снаряда
        bool isBulletThroughHeight = BulletPosition.y > Screen.height || BulletPosition.y < 0; //

        // Проверка выхода снаряда за границы экрана по высоте и ширине экрана
        if (isBulletThroughWidth || isBulletThroughHeight)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка столкновения снаряда и астероида
        if (collision.transform.tag.ToLower().Contains("asteroid"))
        {
            // Начисление очков за попадание в астероид
            int points = collision.transform.tag.ToLower() == "bigasteroid" ? 5 : 10;
            // Уничтодение снаряда
            Destroy(gameObject);
            // Вызов события начисления очков за попадание в астероид
            PointCountEvent?.Invoke(points);
        }
    }
}
