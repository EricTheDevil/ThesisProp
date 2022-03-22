using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";
    public void Start()
    {
        SetupInputField();
    }
    void OnValidate()
    {
        if (!nameInputField)
            nameInputField=GetComponentInChildren<TMP_InputField>();
        if (!continueButton)
            continueButton = GetComponentInChildren<Button>();
    }
    private void SetupInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }
    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }
    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;

        //Player
    }
}
