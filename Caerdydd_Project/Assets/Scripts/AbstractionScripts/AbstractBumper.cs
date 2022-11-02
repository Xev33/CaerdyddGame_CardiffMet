using UnityEngine;
using System.Collections;

public abstract class AbstractBumper : MonoBehaviour
{
    protected Player player;
    [SerializeField] private float bumpProjection;

    private void Start()
    {
        player = Player._instance;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            if (player.currentState != player.standingState && player.hp > 0 && player.isInvicible == false)
            {
                player.SpinJump(bumpProjection);
                StartCoroutine(StretchMesh());
            }
        }
    }

    private IEnumerator StretchMesh()
    {
        float timer = 0f;
        float movementDuration = 0.2f;
        float normalizedValue = 0f;
        Vector3 initialScale = this.gameObject.transform.localScale;
        Vector3 stretchScale = new Vector3(initialScale.x, (initialScale.y / 4), initialScale.z);

        while (timer < movementDuration)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / movementDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            this.transform.localScale = Vector3.Lerp(initialScale, stretchScale, normalizedValue);

            yield return null;
        }

        timer = 0.0f;
        while (timer < movementDuration)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / movementDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            this.transform.localScale = Vector3.Lerp(stretchScale, initialScale, normalizedValue);

            yield return null;
        }

        this.gameObject.transform.localScale = initialScale;
        yield return null;
    }
}
