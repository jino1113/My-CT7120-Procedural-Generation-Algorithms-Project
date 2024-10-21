using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class ButtonTextController : MonoBehaviour
{
    public TextMeshProUGUI message;
    public UnityEngine.UI.Button myButton;
    public UnityEngine.UI.Button myButton2;

    public void showMessage()
    {
        message.text = "ok";
        message.gameObject.SetActive(true);
    }
    public void showMessage2()
    {
        Debug.Log("123");
        message.text = "ok2";
        message.gameObject.SetActive(true);
    }


}
