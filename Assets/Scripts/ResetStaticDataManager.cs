using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        CuttingCounter.resetStaticData();
        BaseCounter.resetStaticData();
        TrashCounter.resetStaticData();
    }
}
