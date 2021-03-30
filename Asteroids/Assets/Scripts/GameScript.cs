using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    /// <summary>
    /// Поле для управления воспроизведением звукового оформления
    /// </summary>
    public static SoundController soundController;
    /// <summary>
    /// Поле List объектов GameObject для хранения астероидов созданных в текущей сессии. Так же служит для определения количества астероидов на экране.
    /// </summary>
    public static List<GameObject> asteroids;
    /// <summary>
    /// Поле объекта большого астероида для клонирования
    /// </summary>
    [Tooltip("Поле для перфаба большого астерода")]
    [SerializeField] GameObject bigAsteroidGO;
    /// <summary>
    /// Поле малого астероида для клонирования
    /// </summary>
    [Tooltip("Поле для перфаба малого астероида")]
    [SerializeField] GameObject smallAsteroidGO;
    /// <summary>
    /// Поле родительского объекта для астероидов
    /// </summary>
    [Tooltip("Поле для родительского объекта астероидов")]
    [SerializeField] Transform asteroidParent;
    /// <summary>
    /// Поле вывода информации на экран
    /// </summary>
    TextOutput output;
    /// <summary>
    /// Свойство выдачи слуйных координат появления астероида
    /// </summary>
    Vector2 RandomPosition
    {
        // Получение случайных координат
        get => new Vector2(Random.Range(-Screen.width, Screen.width), Random.Range(-Screen.height, Screen.height));
    }
    /// <summary>
    /// Полу обозначающее конец игры
    /// </summary>
    bool gameDone;
    /// <summary>
    /// Поле для хранения полученных игроком очков
    /// </summary>
    int points;      

    void Start()
    {
        // Инициализация коллекции asteroids
        asteroids = new List<GameObject>();
        // Инициализация поля вывода информации на экран
        output = GetComponent<TextOutput>();
        // Получение компонента управления звуком
        soundController = FindObjectOfType<SoundController>();

        AsteroidsScript.AsteriodSpawnEvent += AsteroidsScript_AsteriodSpawnEvent; //
        BulletScript.PointCountEvent += BulletScript_PointCountEvent;             // Подписка на события создания астероидов, начисления очков и уничтожения корабля
        SpaceShipControl.CrushEvent += SpaceShipControl_CrushEvent;               //

        // Создание первых астероидов
        while (asteroids.Count < 4)
            CreateAsteroid(true, RandomPosition);
           
    }

    /// <summary>
    /// Обработчик события уничтожения корабля
    /// </summary>
    private void SpaceShipControl_CrushEvent()
    {
        if (this != null)
        {
            gameDone = true;
            // Запуск звука взрыва коробля
            soundController.PlaySound("shipExplosion");
            // Вывод информации окончания игры
            output.loseText.text = $"Your ship is destroyed. You received {points} points. Press enter to restart.";
            output.loseText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Обработчик события начисления очков
    /// </summary>
    private void BulletScript_PointCountEvent(int pointCount)
    {
        if (this != null)
        {
            // Запуск звука взрыва астероида
            soundController.PlaySound("asteroudExplosion");
            // Начисление очков
            points += pointCount;
            // Вывод очков на экран
            output.pointsText.text = $"Points: {points}";
        }
    }

    /// <summary>
    /// Обработчик события создания астероида
    /// </summary>
    private void AsteroidsScript_AsteriodSpawnEvent(bool isBig, Vector2 position)
    {
        if (this != null)
            CreateAsteroid(isBig, position);
    }

    void Update()
    {
        // придание вращения скайбоксу
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 5f);
        CheckAsteroidCount();

        // Перезагрузка игры по нажатию на Enter
        if (gameDone && Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Создание астероидов
    /// </summary>
    /// <param name="isBig"></param>
    /// <param name="position"></param>
    void CreateAsteroid(bool isBig, Vector2 position)
    {
        // Определение какой астероид будет создан
        GameObject newAsteroid = isBig ? Instantiate(bigAsteroidGO, position, transform.rotation, asteroidParent) :
            Instantiate(smallAsteroidGO, position, transform.rotation, asteroidParent);

        // Добавление астероида в коллекцию
        asteroids.Add(newAsteroid);
    }

    /// <summary>
    /// Проверка количества астероидов на сцене
    /// </summary>
    void CheckAsteroidCount()
    {
        if (asteroids.Count < 4)
            CreateAsteroid(true, RandomPosition);
    }
}
