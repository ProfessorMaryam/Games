using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerTracker : MonoBehaviour
{

    public RecordPlayer recordPlayer;
    public MakeSound clubPiano;
    public MakeSound guitarAMP;
    public GameObject trophy;
    public bool hasBeenTracked = false;

    public float displayDuration = 5f;
    
    void Update()
    {
        if (!hasBeenTracked)
        {
            if (clubPiano.hasBeenStruck == true && guitarAMP.hasBeenStruck == true && recordPlayer.hasBeenStruck == true)
            {
                hasBeenTracked = true;
                StartCoroutine(ShowRewardCanvas());
                Debug.Log(gameObject.name + " - All music has been played.");
            }
        }
    }

    private IEnumerator ShowRewardCanvas()
    {
        if (trophy != null)
        {
            SoundManager.Instance.ObjectChannel.PlayOneShot(SoundManager.Instance.trophySound);
            trophy.SetActive(true);
            yield return new WaitForSeconds(displayDuration);
            trophy.SetActive(false);
        }
    }


}
