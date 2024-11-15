using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text messageBox;
    public TMP_Text matchText;
    public GameObject enterPanel;
    public GameObject askPanel;

    private CardManager cardManager;
    private int shufflecounter = 0;
    void Start()
    {
        cardManager = GetComponent<CardManager>();
    }

    void Update()
    {
    }

    public void EnterButton()
    {
        if (inputField.text.Count() > 0)
        {
            inputField.text = inputField.text.Replace(" ", "");
            cardManager.SetCards(inputField.text.Count());
            enterPanel.SetActive(false);
        }
        else
        {
            messageBox.text = "Lütfen bir isim giriniz!";
        }
    }

    public void ShuffleButton()
    {
        cardManager.ShuffleCards();
        shufflecounter++;
        messageBox.text = "KARTLAR KARIÞTIRILDI! " + shufflecounter;

        if (shufflecounter >= 5)
        {
            messageBox.text = "yeter aq";
        }
    }

    public void UpdateMatchText(string _matchText)
    {
        matchText.gameObject.SetActive(true);
        matchText.text = _matchText;
    }

    public void CloseMatchText()
    {
        matchText.gameObject.SetActive(false);
    }

    public void OpenAskPanel()
    {
        askPanel.SetActive(true);
    }

    public void CloseAskPanel()
    {
        askPanel.SetActive(false);
    }

    public void YesButton()
    {
        Camera.main.GetComponent<CameraScript>().Move(new Vector3(8f, 1, -10), 0.1f);
        CloseAskPanel();
    }

    public void NoButton()
    {
        CloseAskPanel();
    }
}
