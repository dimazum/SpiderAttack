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
    private Transform _logPanel;
    [SerializeField]
    private ItemsData2 itemData;
    private EasyObjectPool _easyObjectPool;
    [SerializeField]


    void Start()
    {
        _logPanel = this.gameObject.transform;
        _easyObjectPool = GetComponent<EasyObjectPool>();
        EventManager.Instance.AddListener(EVENT_TYPE.GetResurs, this);
        EventManager.Instance.AddListener(EVENT_TYPE.Buy, this);
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

            case EVENT_TYPE.Buy:
            {
                if (Param == null) return;
                var purchaseModel = (PurchaseModel) Param;
                var resourceLog = new ResourceLog();
                resourceLog.Qty = 1;
                resourceLog.Image = purchaseModel.sprite;


                var imageLogs = new List<ResourceLog>();
                imageLogs.Add(resourceLog);

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
        var obj = _easyObjectPool.GetObjectFromPool("Logs", _logPanel.position, _logPanel.rotation);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.GetChild(2).GetComponent<Image>().sprite = resourceLog.Image;
        obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = resourceLog.Qty.ToString();
        yield return new WaitForSeconds(2f);
        obj.transform.GetChild(2).GetComponent<Image>().sprite = null;
        obj.transform.SetParent(null);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.SetParent(_logPanel);
        _easyObjectPool.ReturnObjectToPool(obj);
    }
}

[System.Serializable]
public struct ResourceLog
{
    public Sprite Image;
    public int Qty;
}