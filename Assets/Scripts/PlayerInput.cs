using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Button _leftButton; 
    [SerializeField] private Button _rightButton;

    public static event Action<float> OnMove;
    public static event Action<bool> OnJump;
 
    private void Update()
    {
#if  UNITY_EDITOR

        OnMove?.Invoke(Input.GetAxisRaw("Horizontal"));
        OnJump?.Invoke(Input.GetKeyDown(KeyCode.Space));

#endif
#if  UNITY_ANDROID
        //_leftButton.gameObject.SetActive(true);
        //_rightButton.gameObject.SetActive(true);
#endif
    }
}
