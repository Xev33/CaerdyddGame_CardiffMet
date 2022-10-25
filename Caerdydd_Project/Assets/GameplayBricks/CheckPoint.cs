using UnityEngine;
using XDScript;

public class CheckPoint : MonoBehaviour, ISubject
{
    private Subject subject = new Subject();
    public bool isOn = false;

    #region Unity functions
    void Start()
    {
        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && isOn == false)
        {
            NotifyObservers(this.gameObject, E_Event.CHECKPOINT_REACHED);
            isOn = true;
        }
    }
    #endregion

    #region Subject initialization
    // How to use it =======> NotifyObservers(this.gameObject, E_Event.YOUR_EVENT);
    public void NotifyObservers(GameObject entity, E_Event eventToTrigger)
    {
        subject.NotifyObservers(entity, eventToTrigger);
    }

    public void AddObserver(ref AbstractObserver observer)
    {
        subject.AddObserver(ref observer);
    }

    public void RemoveObserver(ref AbstractObserver observer)
    {
        subject.RemoveObserver(ref observer);
    }
    #endregion
}
