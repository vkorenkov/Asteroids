using UnityEngine;

public class PositionScript : MonoBehaviour
{
    /// <summary>
    /// Поле главной камеры
    /// </summary>
    new static Camera camera;

    private void Awake()
    {
        // Инициализация поля главной камеры
        camera = Camera.main;
    }

    public static Vector2 CheckPosition(Vector2 position)
    {
        // Получение ширины и высоты экрана 
        // camera.orthographicSize возвращает половину размера экрана по этому требуется умножение на 2
        // Требуется умножение на camera.aspect, для корректного перемещения по ширине от одной стороны экрана к другой
        // через границы экрана
        float sreenWidth = camera.orthographicSize * 2 * camera.aspect;
        float screenHeight = camera.orthographicSize * 2;
       
        float screenRightSide = sreenWidth / 2;      // Получение правой и левой границ экрана
        float screenLeftSide = screenRightSide * -1; //
        
        float screenTopSide = screenHeight / 2;      // Получение верхней и нижней границ экрана
        float screenBottomSide = screenTopSide * -1; //

        // Перещение объекта при пересечении правой границы экрана
        if (position.x > screenRightSide)
            position = new Vector2(screenLeftSide, position.y);
        // Перещение объекта при пересечении левой границы экрана
        if (position.x < screenLeftSide)
            position = new Vector2(screenRightSide, position.y);
        // Перещение объекта при пересечении верхней границы экрана
        if (position.y > screenTopSide)
            position = new Vector2(position.x, screenBottomSide);
        // Перещение объекта при пересечении нижней границы экрана
        if (position.y < screenBottomSide)
            position = new Vector2(position.x, screenTopSide);

        return position;
    }
}
