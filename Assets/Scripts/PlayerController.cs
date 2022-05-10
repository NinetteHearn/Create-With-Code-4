using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public float speed = 2.5f;
    private float superStrength = 15.0f;
    public bool poweredUp = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        playerRB.AddForce(focalPoint.transform.forward * speed * verticalInput);

        powerupIndicator.transform.position = transform.position + new Vector3(0, 1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            poweredUp = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }

    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        poweredUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && poweredUp)
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFP = collision.gameObject.transform.position - transform.position;

            enemyRB.AddForce(awayFP * superStrength, ForceMode.Impulse);

            Debug.Log($"Collided with {collision.gameObject.name} with powerup st to {poweredUp}");
        }

    }
}
