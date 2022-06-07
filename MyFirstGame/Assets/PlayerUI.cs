using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    public EvolutionHandler evolutionHandler;

    bool _firstPerson = false;

    public bool firstPerson
    {
        get { return _firstPerson; }
        set
        {
            if (value == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            _firstPerson = value;
        }
    }

    public GameObject EvolvePanel;
    public bool EvolvePanelOpened = false;
    public Button confirmEvolve;

    // Start is called before the first frame update
    void Start()
    {
        firstPerson = true;
        confirmEvolve.onClick.AddListener(OnEvolveConfirmClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (EvolvePanelOpened == true)
            {
                toggleEvolveOpen(false);
            }
            else
            {
                toggleEvolveOpen(true);
            }

        }
    }

    void OnEvolveConfirmClicked() {
        evolutionHandler.EvolveNext();
    }

    void toggleEvolveOpen(bool toggle)
    {
        EvolvePanel.SetActive(toggle);
        EvolvePanelOpened = toggle;
        firstPerson = !toggle;

    }
}
