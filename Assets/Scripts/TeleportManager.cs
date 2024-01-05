using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public GameObject centralPack;
    public GameObject player;
    public CinemachineVirtualCamera mainCamera;
    public Transform targetPosition;
    public Transform bufferPosition;
    public SpawnEffectTrigger spawnEffect;
    public float effectTime = 2f;
    public float waitTime = 0f;
    public Transform originPosition;
    private void Start()
    {
        spawnEffect.setEffectTime(effectTime);
    }
    public void teleport(Transform target)
    {
        targetPosition = target;
        StartCoroutine(startTeleport());
    }

    public void OnButtonTeleport()
    {
        StartCoroutine(startTeleport());
    }

    IEnumerator startTeleport()
    {
        spawnEffect.Construct();
        yield return new WaitForSeconds(spawnEffect.spawnEffectTime);
        simpleTeleport(bufferPosition.position);
        yield return new WaitForSeconds(waitTime);
        simpleTeleport(originPosition.position);
        centralPack.SetActive(false);
        mainCamera.OnTargetObjectWarped(centralPack.transform, targetPosition.position - centralPack.transform.position);
        //yield return new WaitForSeconds(0.001f);
        centralPack.transform.position = targetPosition.position;
        // yield return new WaitForSeconds(0.001f);
        centralPack.SetActive(true);
        spawnEffect.Dissvolve();
    }

    public void simpleTeleport(Vector3 target)
    {
        StartCoroutine(Teleport(target));
    }

    IEnumerator Teleport(Vector3 target)
    {
        //spawnEffect.Construct();
        //yield return new WaitForSeconds(spawnEffect.spawnEffectTime + waitTime);
        player.SetActive(false);
        mainCamera.OnTargetObjectWarped(player.transform, target - player.transform.position);
        //yield return new WaitForSeconds(0.001f);
        player.transform.position = target;
        // yield return new WaitForSeconds(0.001f);
        player.SetActive(true);
        //spawnEffect.Dissvolve();
        yield return null;
    }

    public void setDissolve(GameObject gameObject)
    {
        spawnEffect = gameObject.GetComponent<SpawnEffectTrigger>();
    }
}
