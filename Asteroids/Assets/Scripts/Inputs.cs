using UnityEngine;

public class Inputs : MonoBehaviour
{
    /// <summary>
    /// Поле времени перезарядки выстрела
    /// </summary>
    [Tooltip("Время на перезарядку выстрела (0.1 - 1)")]
    [SerializeField, Range(0.1f, 1)] float shootTimerValue;
    /// <summary>
    /// поле экземпляра класса управления кораблем
    /// </summary>
    SpaceShipControl spaceShipControl;
    /// <summary>
    /// Поле хранения ввода по горизонтальной оси
    /// </summary>
    float horizontalAxis;
    /// <summary>
    /// Поле хранения ввода по вертикальной оси
    /// </summary>
    float verticalAxis;
    /// <summary>
    /// Поле таймера перезарядки выстрела
    /// </summary>
    float shootTimer;
    /// <summary>
    /// Поле возможности стрельбы
    /// </summary>
    public bool canShoot = true;

    private void Awake()
    {
        // Получение компонента управления кораблем
        spaceShipControl = GetComponent<SpaceShipControl>();
        shootTimer = shootTimerValue;
    }

    void Update()
    {
        // Получение ввода по горизонтальной оси
        horizontalAxis = Input.GetAxis("Horizontal");
        // Получение ввода по вертикальной оси
        verticalAxis = Input.GetAxis("Vertical");
        // Изменение таймера перезарядки
        shootTimer -= Time.deltaTime;

        // проверка возможности выстрела
        if (Input.GetKey(KeyCode.Space) && shootTimer <= 0 && canShoot)
        {
            // Присвоение значения таймера после выстрела
            shootTimer = shootTimerValue;
            // Выстрел
            spaceShipControl.Shoot();
            // Запуск звука выстрела
            GameScript.soundController.PlaySound("laserShoot");
        }
    }

    private void FixedUpdate()
    {
        // Осуществление пердвижения корабля
        spaceShipControl.MoveControl(horizontalAxis, verticalAxis);
    }
}
