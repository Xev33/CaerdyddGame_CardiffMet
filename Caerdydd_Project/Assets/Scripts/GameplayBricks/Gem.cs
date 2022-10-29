using UnityEngine;
using System.Collections;
using XDScript;

public class Gem : MonoBehaviour, ISubject
{
    public int gemID;
    [SerializeField] private Material notFoundedMat;
    [SerializeField] private Material alreadyFoundedMat;
    [SerializeField] private MeshRenderer currentMat;
    [SerializeField] private float vanishingDuration = 1.0f;
    private Subject subject = new Subject();

    void Awake()
    {
        currentMat = this.gameObject.GetComponent<MeshRenderer>();
        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
            NotifyObservers(this.gameObject, E_Event.GEM_FOUNDED);
            StartCoroutine(GemCollected());
        }
    }

    public void UpdateMaterial(int shouldBeTransparent)
    {
        if (shouldBeTransparent == 1)
            currentMat.material = alreadyFoundedMat;
        else
            currentMat.material = notFoundedMat;
    }

    private IEnumerator GemCollected()
    {
        float timer = 0.0f;
        this.gameObject.GetComponent<CollectibleRotation>().speed = this.gameObject.GetComponent<CollectibleRotation>().speed * 3;
        Vector3 initialScale = this.transform.localScale;
        float normalizedValue = 0f;

        currentMat.material = alreadyFoundedMat;
        while (timer < vanishingDuration)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / vanishingDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            this.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, normalizedValue);

            yield return null;
        }
        Destroy(this.gameObject.GetComponent<CollectibleRotation>());
        yield return null;
    }

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
