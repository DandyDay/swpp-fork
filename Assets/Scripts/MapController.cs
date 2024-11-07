using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

public class MapController : MonoBehaviour
{
    public GameObject mapPrefab;
    private GameObject map;
    private int anomalyIndex = -1;
    private const int maxAnomalyCount = 10;
    private Anomaly[] anomalies;

    void Start()
    {
        // 델리게이트 배열 초기화
        anomalies = new Anomaly[]
        { gameObject.AddComponent<EasyPianoAnomaly>(),
        };
    }
    public GameObject GenerateMap(bool haveAnomaly)
    {
        if (!haveAnomaly)
        {
            map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity, transform);
            return map;
        }
        else
        {
            map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity, transform);
            SetAnomaly(anomalies[++anomalyIndex]);
            if (anomalyIndex >= maxAnomalyCount)
            {
                // TODO: There are two options
                // first option: just refill anomalies and keep playing game
                // second option: game over
            }
            return map;
        }
    }

    private void SetAnomaly(Anomaly anomaly)
    {
        if (anomalyIndex < anomalies.Length)
        {
            anomaly.Apply(map);
        }
    }
}
