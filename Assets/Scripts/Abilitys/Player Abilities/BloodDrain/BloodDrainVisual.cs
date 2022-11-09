using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDrainVisual : VisualEffect
{
    public float speed = 0.5f;
    public float rotationSpeed = 10f;
    private float fraction = 0f;

    [SerializeField]
    private ParticleSystem startParticle;
    [SerializeField]
    private ParticleSystem hitParticle;

    // Start is called before the first frame update
    void Start()
    {
        startParticle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        direction = (targetLocation.position - transform.position).normalized;

        if (fraction < 1f)
        {
            fraction += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(spawnLocation.transform.position, targetLocation.position, fraction);
        }
        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if ((Vector3.Distance(transform.position, targetLocation.position) <= 1f))
        {
            hitParticle.Play();
            Destroy(this.gameObject, 1);
        }
    }
}
