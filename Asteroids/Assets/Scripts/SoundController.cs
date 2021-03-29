using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] List<AudioSource> audios;

    public void PlaySound(string audioName)
    {
        audios.Where(x => x.name.ToLower() == audioName.ToLower()).FirstOrDefault().Play();
    }
}
