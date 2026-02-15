using UnityEngine;

public class TrialPortal : MonoBehaviour
{
    [SerializeField] public int trialIndex;
    [SerializeField] private GameObject[] vfxLists;

    private void Start()
    {

    }

    private void Update()
    {
        switch (TrialManager.Instance.GetTrialState(trialIndex))
        {
            case TrialManager.TrialState.Idle:
                {
                    vfxLists[0].SetActive(true);
                    vfxLists[1].SetActive(false);
                    vfxLists[2].SetActive(false);
                    break;
                }
            case TrialManager.TrialState.InProgress:
                {
                    vfxLists[0].SetActive(false);
                    vfxLists[1].SetActive(true);
                    vfxLists[2].SetActive(false);
                    break;
                }
            case TrialManager.TrialState.Completed:
                {
                    vfxLists[0].SetActive(false);
                    vfxLists[1].SetActive(false);
                    vfxLists[2].SetActive(true);
                    break;
                }
        }
    }
}
