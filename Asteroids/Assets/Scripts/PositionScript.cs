using UnityEngine;

public class PositionScript : MonoBehaviour
{
    new static Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    public static Vector2 CheckPosition(Vector2 position)
    {
        float sreenWidth = camera.orthographicSize * 2 * camera.aspect;
        float screenHeight = camera.orthographicSize * 2;

        float screenRightSide = sreenWidth / 2;
        float screenLeftSide = screenRightSide * -1;
        float screenTopSide = screenHeight / 2;
        float screenBottomSide = screenTopSide * -1;

        if (position.x > screenRightSide)
            position = new Vector2(screenLeftSide, position.y);

        if (position.x < screenLeftSide)
            position = new Vector2(screenRightSide, position.y);

        if (position.y > screenTopSide)
            position = new Vector2(position.x, screenBottomSide);

        if (position.y < screenBottomSide)
            position = new Vector2(position.x, screenTopSide);

        return position;
    }
}
