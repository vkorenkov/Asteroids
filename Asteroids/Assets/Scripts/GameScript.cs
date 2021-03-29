using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    /// <summary>
    /// поле для управления воспроизведением звукового оформления
    /// </summary>
    public static SoundController soundController;
    /// <summary>
    /// поле List объектов GameObject для хранения астероидов созданных в текущей сессии. Так же служит для определения количества астероидов на экране.
    /// </summary>
    public static List<GameObject> asteroids;

    [SerializeField] GameObject bigAsteroidGO;
    [SerializeField] GameObject smallAsteroidGO;
    [SerializeField] Transform asteroidParent;
    TextOutput output;
    bool gameDone;
    int points;      

    void Start()
    {
        asteroids = new List<GameObject>();

        output = GetComponent<TextOutput>();

        soundController = FindObjectOfType<SoundController>();

        AsteroidsScript.AsteriodSpawnEvent += AsteroidsScript_AsteriodSpawnEvent;
        BulletScript.PointCountEvent += BulletScript_PointCountEvent;
        SpaceShipControl.CrushEvent += SpaceShipControl_CrushEvent;

        while (asteroids.Count < 4)
            CreateAsteroid(true, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }

    private void SpaceShipControl_CrushEvent()
    {
        if (this != null)
        {
            gameDone = true;
            soundController.PlaySound("shipExplosion");
            output.loseText.text = $"Your ship is destroyed. You received {points} points. Press enter to restart.";
            output.loseText.gameObject.SetActive(true);
        }
    }

    private void BulletScript_PointCountEvent(int pointCount)
    {
        if (this != null)
        {
            soundController.PlaySound("asteroudExplosion");
            points += pointCount;
            output.pointsText.text = $"Points: {points}";
        }
    }

    private void AsteroidsScript_AsteriodSpawnEvent(bool isBig, Vector2 position)
    {
        if (this != null)
            CreateAsteroid(isBig, position);
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 5f);
        CheckAsteroidCount();

        if (gameDone && Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void CreateAsteroid(bool isBig, Vector2 position)
    {
        GameObject newAsteroid = isBig ? Instantiate(bigAsteroidGO, position, transform.rotation, asteroidParent) :
            Instantiate(smallAsteroidGO, position, transform.rotation, asteroidParent);

        asteroids.Add(newAsteroid);
    }

    void CheckAsteroidCount()
    {
        if (asteroids.Count < 4)
            CreateAsteroid(true, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }
}
