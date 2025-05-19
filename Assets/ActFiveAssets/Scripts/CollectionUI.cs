using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

public class CollectionUI : MonoBehaviour
{
    public GameObject eventosCanvas;
    private TextMeshProUGUI eventoSaurusText;
    private int currentCount;

    // Start is called before the first frame update
    void Start()
    {
        eventosCanvas.SetActive(false);
        eventoSaurusText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateEventoSaurus(PlayerCollection playerCollection)
    {
        currentCount = playerCollection.NumberOfEventoSaurus;
        eventoSaurusText.text = currentCount + " of 7 EventoSaurus Collected";
    }

}
