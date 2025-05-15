using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPass : MonoBehaviour
{
    public GameObject Area;
    public RecordPlayer musicPlayer;
    public MakeSound musicInstrement;
    public Marc marc;
    public MarcHealth marcHealth;
    public PlayerSpawnManager playerSpawnManager;
    public string spawnerTag;

    public int yieldBeforeQuest; //time waiting before setting a certain object to true
    public GameObject quest; //quest gameobject that will be set to true after the yeild 

    public bool isPassed = false;
    public AudioClip chime;

    private IEnumerator setNextQuest()
    {
        yield return new WaitForSeconds(yieldBeforeQuest);
        quest.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {

        playerSpawnManager.currentSpawner = spawnerTag;

        Area.SetActive(false);
        Debug.Log($"Area Gone.");

        marc.HP = 250;

        if (marc != null)
        {
            marc.HP = 250; 
        }

        if (marcHealth != null)
        {
            marcHealth.RestoreFullHealth();
        }

        if (musicPlayer != null)
        {
            musicPlayer.Currupted();
        }
        else
        {
            Debug.LogWarning("musicPlayer GameObject is not assigned!");
        }

        if (musicInstrement != null)
        {
            musicInstrement.Currupted();
        }
        else
        {
            Debug.LogWarning("musicInstrement GameObject is not assigned!");
        }

        if (!isPassed)
        {
            SoundManager.Instance.MoonChannel.PlayOneShot(chime);
            isPassed = true;
            StartCoroutine(setNextQuest());
        }

        

    }
}
