using UnityEngine;

public class EasyDrawerMissingContentAnomaly : Anomaly
{
    public override void Apply(GameObject map)
    {
        GameObject missingContent = storage.drawerMissingContent;
        missingContent.SetActive(false);
    }
}