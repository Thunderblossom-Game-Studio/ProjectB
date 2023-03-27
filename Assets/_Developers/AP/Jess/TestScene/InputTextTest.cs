using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputTextTest : MonoBehaviour
{

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = InputManager.Instance.GetBindingName(InputManager.Binding.Interact);
    }
}
