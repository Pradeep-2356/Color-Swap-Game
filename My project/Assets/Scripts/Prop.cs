using UnityEngine;

public class Prop : MonoBehaviour
{
    public GameObject timerPrefab;  
    public PropType propType = PropType.None; // default

private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    // Timer and task only for real props
    if (propType != PropType.None)
    {
        PerformTask(other.gameObject);

        if (timerPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject timer = Instantiate(timerPrefab, canvas.transform);
                Destroy(timer, 5f);
            }
        }

        // Destroy real prop immediately
        Destroy(gameObject);
    }
    else
    {
        // For distraction props (propType.None)
        // Optionally make them disappear after some time instead of destroying immediately
        float distractionDuration = 5f; // for example
        Destroy(gameObject, distractionDuration);
    }
}

    void PerformTask(GameObject player)
    {
        var pc = player.GetComponent<PlayerController>();

        switch (propType)
        {
            case PropType.WaterRipple:
                if (pc != null) pc.ForceDuck(1f); // force crouch 1 sec
                break;

            case PropType.Gulal:
                Debug.Log("Turns all tiles to one color for 5s.");
                TileSpawner spawner = FindObjectOfType<TileSpawner>();
                if (spawner != null && pc != null)
                {
                    spawner.ActivateGulal(pc.currentColor, 10f); // use player's current color
                }
                break;

            case PropType.Shield:
                Debug.Log("Saves player from wrong match.");
                if (pc != null)
                {
                    pc.ActivateShield(5f); // shield active for 5 seconds
                }
                break;

            case PropType.Star:
                Debug.Log("Double points");
                break;

            case PropType.WaterGun:
                Debug.Log("Clears gulal clouds instantly");
                    if (pc != null)
                    {
                        pc.hasWaterGun = true;
                        if (pc.waterGunIcon != null)
                        pc.waterGunIcon.SetActive(true); // show icon on canvas
                    }
                break;
        }
    }
}
