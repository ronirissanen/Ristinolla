using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button codeButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject hostMenu;
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            RelayHandler.Instance.CreateRelay();
            SetVisible(true, false, true);
        });
        joinButton.onClick.AddListener(() =>
        {
            SetVisible(false, true, true);
        });
        backButton.onClick.AddListener(() =>
        {
            SetVisible(false, false, false);
        });
        inputField.onSubmit.AddListener((value) =>
        {
            Debug.Log("Entered code: " + value);
            RelayHandler.Instance.JoinRelay(value);
        });
        codeButton.onClick.AddListener(() =>
        {
            CopyCode();
        });
        RelayHandler.Instance.hostReady.AddListener(SetCodeText);
    }

    private void SetVisible(bool _isHostMenuVisible, bool _isJoinMenuVisible, bool _isBackButtonVisible)
    {
        hostMenu.SetActive(_isHostMenuVisible);
        joinMenu.SetActive(_isJoinMenuVisible);
        backButton.gameObject.SetActive(_isBackButtonVisible);
        if (_isHostMenuVisible)
            CopyCode();
    }

    private void CopyCode()
    {
        GUIUtility.systemCopyBuffer = RelayHandler.Instance.GetCode();
    }

    private void SetCodeText()
    {
        codeButton.GetComponent<TMP_Text>().text = RelayHandler.Instance.GetCode();
    }
}
