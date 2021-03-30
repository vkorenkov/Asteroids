using UnityEngine;
using UnityEngine.UI;

public class TextOutput : MonoBehaviour
{
    /// <summary>
    /// Поле вывода полученных очков на экран
    /// </summary>
    [Tooltip("Поле для UI Text для вывода очков на экран")]
    public Text pointsText;
    /// <summary>
    /// Поле сообщения поражения
    /// </summary>
    [Tooltip("Поле для UI Text для вывода сообщения поражения на экран")]
    public Text loseText;
}
