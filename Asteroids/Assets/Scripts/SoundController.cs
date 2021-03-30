using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    /// <summary>
    /// Коллекция всех аудио объектов
    /// </summary>
    [Tooltip("Поле List для объектов звукового оформления")]
    [SerializeField] List<AudioSource> audios;

    /// <summary>
    /// Метод воспроизведения необходимого звука по имени объекта
    /// </summary>
    /// <param name="audioName"></param>
    public void PlaySound(string audioName)
    {
        audios.Where(x => x.name.ToLower() == audioName.ToLower()).FirstOrDefault().Play();
    }
}
