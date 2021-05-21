using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioListener))]
public class AudioManager : MonoBehaviour
    {
        public bool IsSetByInspector = true;
        [Range(0, 1)] public float MusicVolum=1;
        [Range(0, 1)] public float SFXVolum=1;
        [Range(0, 1)] public float NaratorVolum=1;

        public static float VolumeMusic;
        public static float VolumeSFX;
        public static float VolumeNarator;

        private static List<AudioSource> _sfxPlayrs = new List<AudioSource>();
        private static List<AudioSource> _musicPlayers = new List<AudioSource>();
        private static AudioSource _narratorsPlayers;
        private static GameObject _audioHolder;
        private static Transform _cameraPos;


        private void Awake()
        {
            if (IsSetByInspector)
            {
                VolumeMusic = MusicVolum;
                VolumeSFX = SFXVolum;
                VolumeNarator = NaratorVolum;
            }
            _audioHolder = gameObject;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (_cameraPos != null) transform.position = _cameraPos.position;
        }

        /// <summary>
        ///  Permet de lancer un sound effect
        /// </summary>
        /// <param name="clip"> C'est la son à jouer</param>
        /// <param name="volume">Valeur normaliser pour le volume</param>
        public static void PlaySfx(AudioClip clip, float volume =1)
        {
            AudioSource audioSource = _audioHolder.AddComponent<AudioSource>();
            _sfxPlayrs.Add(audioSource);
            audioSource.volume = VolumeSFX * volume;
            audioSource.loop = false;
            audioSource.spatialBlend = 1;
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSource, clip.length+1);
        }
        
        /// <summary>
        /// Permet de lancer une music("pour l'instant ne coupe pas les autres")
        /// </summary>
        /// <param name="clip">La musique à jouer</param>
        /// <param name="volume">aleur normaliser pour le volume</param>
        public static void PlayMusic(AudioClip clip, float volume=1) {
            AudioSource audioSource = _audioHolder.AddComponent<AudioSource>();
            _musicPlayers.Add(audioSource);
            audioSource.volume = VolumeMusic * volume;
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSource, clip.length+1);
        } 
        
        /// <summary>
        /// Permet de Lancer un enregistrement pour le narateur
        /// </summary>
        /// <param name="clip">L'enregistrement du narrateur a lire</param>
        /// <param name="volume">aleur normaliser pour le volume</param>
        public static  void PlayNarator(AudioClip clip, float volume=1) {
            AudioSource audioSource = _audioHolder.AddComponent<AudioSource>();
            Destroy(_narratorsPlayers);
            _narratorsPlayers = audioSource;
            audioSource.volume = VolumeNarator * volume;
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSource, clip.length+1);
        }

        public static void SetCameraTransform(Transform pos)
        {
            _cameraPos = pos;
        }
    }
