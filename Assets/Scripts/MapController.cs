using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.Generic;
using TMPro;
using Shuffle = System.Random;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public GameObject mapPrefab;
    private GameObject currentMap;
    private ObjectStorage storage;
    private float initialClockRotation = 240.0f;

    private AnomalyManager anomalyManager;

    //only for anomaly testing


    private void CleanupCurrentMap()
    {
        GameObject[] mapObjects = GameObject.FindGameObjectsWithTag("Map");
        foreach (GameObject mapObject in mapObjects)
        {
            Destroy(mapObject);
        }
        currentMap = null;
    }

    // returns current anomaly is hard or not
    public AnomalyCode GenerateMap(bool haveAnomaly, int stage)
    {
        CleanupCurrentMap();
        Anomaly anomaly;
        anomalyManager = FindObjectOfType<AnomalyManager>();
        bool test = anomalyManager.test;
        int testAnomaly = anomalyManager.testAnomaly;
        bool testHard = anomalyManager.testHard;

        if (!haveAnomaly)
        {
            currentMap = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity, transform);
            SetClock(stage);
            Debug.Log($"Stage {stage}: No Anomaly");
            if (stage == 0)
            {
                storage = currentMap.GetComponent<ObjectStorage>();
                storage.tutorialImage.SetActive(true);
            }
            return AnomalyCode.NoAnomaly; // TODO: Make seperate anomaly code for easy anomalies
        }
        else
        {
            currentMap = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity, transform);
            SetClock(stage);
            if (test)
            {
                if (testHard)
                {
                    Debug.Assert(anomalyManager.hardAnomalies != null, "NULL2");
                    anomaly = anomalyManager.hardAnomalies[testAnomaly];
                    Debug.Log($"Test: Anomaly {anomaly.GetType()}");
                }
                else
                {
                    anomaly = anomalyManager.easyAnomalies[testAnomaly];
                    Debug.Log($"Test: Anomaly {anomaly.GetType()}");
                }
            }
            else
            {
                if (stage < 5)
                {
                    anomaly = anomalyManager.easyAnomalies[anomalyManager.easyAnomalyIndex++];
                    Debug.Log($"Stage {stage}: Anomaly {anomaly.GetType()}");
                }
                else
                {
                    anomaly = anomalyManager.hardAnomalies[anomalyManager.hardAnomalyIndex++];
                    Debug.Log($"Stage {stage}: Anomaly {anomaly.GetType()}");
                }
            }

            SetAnomaly(anomaly);
            return anomaly.GetAnomalyCode();
        }
    }

    private void SetAnomaly(Anomaly anomaly)
    {
        anomaly.ApplyAnomaly(currentMap);
        //Do Additional Setting For Each Hard Anomaly
        if (anomaly is HardAnomaly)
        {
            HardAnomaly ha = anomaly as HardAnomaly;
            ha.SetAnomalyCode();
        }
    }

    private void SetClock(int stage)
    {
        ObjectStorage storage = currentMap.GetComponent<ObjectStorage>();
        GameObject clockHourHand = storage.clockHourHand;
        clockHourHand.transform.localRotation = Quaternion.Euler(-90, 0, initialClockRotation + 30 * stage);
        TextMeshPro digitalClockText = storage.digitalClockText.GetComponent<TextMeshPro>();
        digitalClockText.text = stage == 0 ? "00:00" : "0" + stage.ToString() + ":00";
    }
}
