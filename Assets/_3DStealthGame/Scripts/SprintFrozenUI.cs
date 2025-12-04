using UnityEngine;
using UnityEngine.UI;

public class SprintFrozenUI : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public GameObject SprintOn;         
    public GameObject SprintOff;
    public GameObject FrozenOn;
    public GameObject FrozenOff;


    void Update()
    {
        if (playerMovement == null) return;

        if (playerMovement.CanSprint)
        {
            SprintOn.SetActive(true);
            SprintOff.SetActive(false);
        }
        else
        {
            SprintOn.SetActive(false);
            SprintOff.SetActive(true);
        }
        if (playerMovement.IsFrozen)
        {
            FrozenOn.SetActive(true);
            FrozenOff.SetActive(false);
        }
        else
        {
            FrozenOn.SetActive(false);
            FrozenOff.SetActive(true);
        }
    }
}
