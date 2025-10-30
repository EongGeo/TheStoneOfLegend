using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedDisplay : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }
    private void Start()
    {
        SoundManager.Instance.PlayMainMenuBGM();
    }
}
