using UnityEngine;

namespace Services.SoundServices
{
    public class SoundSource : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        public AudioSource GetAudioSource()
        {
            return _audioSource;
        }
    }
}