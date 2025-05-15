using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollection : MonoBehaviour
{
    [SerializeField] private GameObject eventosCanvas;
    public int NumberOfEventoSaurus { get; private set; }

    public UnityEvent<PlayerCollection> OnEventoSaurusCollected;
    public void EventoSaurusCollected()
    {
        NumberOfEventoSaurus++;
        OnEventoSaurusCollected.Invoke(this);

            StartCoroutine(ShowCanvasForSeconds(5f));
        
    }

    private IEnumerator ShowCanvasForSeconds(float seconds)
    {
        eventosCanvas.SetActive(true);
        yield return new WaitForSeconds(seconds);
        eventosCanvas.SetActive(false);
    }
}
