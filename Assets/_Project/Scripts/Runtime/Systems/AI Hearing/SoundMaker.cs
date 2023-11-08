using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay
{
    public class SoundMaker : MonoBehaviour
    {
        [SerializeField] private AudioSource source = null;

        [SerializeField] private float soundRange = 25f;
        
        [SerializeField] private Sound.SoundType soundType = Sound.SoundType.Dangerous;

        public void MakeASound()
        {
            if (source.isPlaying) //If already playing a sound, don't allow overlapping sounds 
                return;

            var sound = new Sound(transform.position, soundRange, soundType);

            Sounds.MakeSound(sound);
        }
    }
}
