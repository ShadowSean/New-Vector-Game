using UnityEngine;
using System.Collections;

public class TaserRodAttack : MonoBehaviour
{
    public float stunRange = 3f;
    public float cooldown = 1.5f;      // seconds between stuns
    public LayerMask enemyLayer;

    private bool canStun = true;
    private Camera playerCam;

    

    void Start()
    {
        playerCam = Camera.main;
    }

    void Update()
    {
        // Only stun when pressing LMB AND cooldown ready
        if (Input.GetMouseButtonDown(0) && canStun)
        {
            TryStunEnemy();
        }
    }

    void TryStunEnemy()
    {
        // Raycast from camera forward
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, stunRange, enemyLayer))
        {
            Vector9Movement enemy = hit.collider.GetComponent<Vector9Movement>();

            if (enemy != null && enemy.isStunned == false)
            {
                enemy.Stun();
                StartCoroutine(CooldownRoutine());
            }
        }
    }

    IEnumerator CooldownRoutine()
    {
        canStun = false;
        yield return new WaitForSeconds(cooldown);
        canStun = true;
    }
}
