using UnityEngine;

public class Prop : MonoBehaviour
{
     public GameObject timerPrefab;  // Assign in prefab or during spawning
    public PropType propType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the task specific to this prop
            PerformTask(other.gameObject);

            // Show timer
            if (timerPrefab != null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    GameObject timer = Instantiate(timerPrefab, canvas.transform);

                    // Destroy after 5 seconds
                    Destroy(timer, 5f);
                }
            }

            // Destroy the prop
            Destroy(gameObject);
        }
    }

    void PerformTask(GameObject player)
    {
        switch (propType)
        {
            case PropType.WaterRipple:
                Debug.Log("crouch");
                // TODO: Call player health logic
                break;

            case PropType.Gulal:
                Debug.Log("Turns all tiles to one color for 5s.");
                // TODO: Call player speed logic
                break;

            case PropType.Shield:
                Debug.Log("Saves player from wrong match.");
                // TODO: Call shield logic
                break;

            case PropType.Star:
                Debug.Log("Double points");
                // TODO: Call score multiplier logic
                break;     

            case PropType.WaterGun:
                Debug.Log("Clears gulal clouds instantly");
                // TODO: Call score multiplier logic
                break;
        }
    }
}
