using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    private GameObject _miniMapPanel;

    public void MiniMapToggle()
    {
        PanelsController.Instance.PanelsDeactivation(_miniMapPanel);
        _miniMapPanel.SetActive(!_miniMapPanel.activeSelf);
    }
}
