using System;
using MarchingBytes;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils.enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogResourcesPanel : MonoBehaviour, IListener
{
    [SerializeField]
    private Transform logPanel;
    [SerializeField]
    private ItemsData2 itemData;
    private EasyObjectPool _easyObjectPool;

    void Start()
    {
        _easyObjectPool = GetComponent<EasyObjectPool>();
        EventManager.Instance.AddListener(EVENT_TYPE.GetResurs, this);
    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GetResurs:
            {
                if (Param == null) return;

                var resourceBlockInfo = (ResourceBlockInfo) Param;

                if (resourceBlockInfo.ResourceInfos[0].ItemGroup == (int)ItemGroup.Grounds)
                {
                    return;
                }

                var imageLogs = new List<ResourceLog>();
                foreach (var resourceInfo in resourceBlockInfo.ResourceInfos)
                {
                    var item = itemData.collections2[resourceInfo.ItemGroup].itemTypes[resourceInfo.Category];
                    imageLogs.Add(new ResourceLog{Image = item.image, Qty = resourceInfo.Qty});
                }

                StartCoroutine(LogsOutput(imageLogs));
                break;
            }
        }
    }

    public IEnumerator LogsOutput(List<ResourceLog> imageLogs)
    {
        for (int i = 0; i < imageLogs.Count; i++)
        {
            StartCoroutine(ShowLog(imageLogs[i]));
            yield return new WaitForSeconds(.2f);
        }

    }

    public IEnumerator ShowLog(ResourceLog resourceLog)
    {
        var obj = _easyObjectPool.GetObjectFromPool("Logs", logPanel.position, logPanel.rotation);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<Image>().sprite = resourceLog.Image;
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = resourceLog.Qty.ToString();
        yield return new WaitForSeconds(2f);
        obj.GetComponent<Image>().sprite = null;
        obj.transform.SetParent(null);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.SetParent(logPanel);
        _easyObjectPool.ReturnObjectToPool(obj);
    }
}

[System.Serializable]
public struct ResourceLog
{
    public Sprite Image;
    public int Qty;
}