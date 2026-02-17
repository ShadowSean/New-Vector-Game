using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour
{
    [Header("Usage")]
    public float cooldown = 20f;    // cooldown for the flamethrower(will be changed to uses later)
    private bool canSlow = true; // boolean for knowing if slow can be applied

    [Header("Fire Zone")]
    public GameObject fireZoneObject;
    public float zoneDuration = 5f;

    [Header("Particles")]
    public ParticleSystem muzzleParticles;
    public ParticleSystem zoneParticles;

    private void OnEnable()
    {
        canSlow = true;

        if (fireZoneObject != null)
        {
            fireZoneObject.SetActive(false);
        }

        StopAndClear(muzzleParticles);
        StopAndClear(zoneParticles);
    }

    private void OnDisable()
    {
        if (fireZoneObject != null)
        {
            fireZoneObject.SetActive(false);
        }

        StopAndClear(muzzleParticles);
        StopAndClear(zoneParticles);
    }


    void Update()
    {
        // Only slow when pressing LMB AND cooldown ready
        if (Input.GetMouseButtonDown(0) && canSlow)
        {
            ActivateFlamethrower();
        }
    }

    void ActivateFlamethrower()
    {
        canSlow = false;

        //Enabling fire zone prefab
        if (fireZoneObject != null)
        {
            fireZoneObject.SetActive(true);
        }

        //PLay the muzzle particles
        if (muzzleParticles != null)
        {
            muzzleParticles.Play();
        }

        if (zoneParticles != null)
        {
            zoneParticles.Play();
        }

        StopAllCoroutines();
        StartCoroutine(FlameDurationRoutine());
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator FlameDurationRoutine()
    {
        yield return new WaitForSeconds(zoneDuration);
        if (fireZoneObject != null)
        {
            fireZoneObject.SetActive(false);
        }

        if (muzzleParticles != null)
        {
            muzzleParticles.Stop();
        }

        if (zoneParticles != null)
        {
            zoneParticles.Stop();
        }
    }


    IEnumerator CooldownRoutine()
    {
        canSlow = false;
        yield return new WaitForSeconds(cooldown);
        canSlow = true;
    }

    void StopAndClear(ParticleSystem particleSystem)
    {
        if (particleSystem == null)
        {
            return;
        }
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSystem.Clear(true);
    }
}
