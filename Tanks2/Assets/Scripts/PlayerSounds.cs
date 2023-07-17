using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] AudioClip mainCaliberShotSound;
    [SerializeField] AudioClip reloadingSound;
    [SerializeField] AudioClip[] penetratedSound;
    [SerializeField] AudioClip[] notPenetratedSound;
    
    GameObject player;
    AudioSource TankAudioSource;
    // Start is called before the first frame update
    private void Awake() {
        player=GameObject.FindGameObjectWithTag("Player");
        if(player)
        TankAudioSource=player.GetComponent<AudioSource>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMainCaliberSound()
    {
        if(player && mainCaliberShotSound&& TankAudioSource)
        {
            TankAudioSource.PlayOneShot(mainCaliberShotSound);
        }
    }
    public void PlayReloadSound()
    {
        if(player && reloadingSound&& TankAudioSource)
        {
            TankAudioSource.PlayOneShot(reloadingSound);
        }
    }
    public void PlayPenetratedSound()
    {
        if(player && penetratedSound.Length>0&& TankAudioSource)
        {
            int rand = Random.Range(0,penetratedSound.Length);
            TankAudioSource.PlayOneShot(penetratedSound[rand]);
        }
    }
    public void PlayNotPenetratedSound()
    {
        if(player && notPenetratedSound.Length>0&& TankAudioSource)
        {

            TankAudioSource.PlayOneShot(notPenetratedSound[Random.Range(0,notPenetratedSound.Length)]);
        }
    }

    void DestroyAudioSource(AudioSource source_)
    {
        Destroy(source_.gameObject);
    }
}
