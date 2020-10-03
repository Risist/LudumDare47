using UnityEngine;
using System.Collections;

public class Tongue : MonoBehaviour
{
    public TongueBullet tongueBullet;
    public Transform tongueSpawnPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            FireTongueBullet();
        }
    }

    public void FireTongueBullet()
    {
        var bullet = Instantiate(tongueBullet, tongueSpawnPoint.position, tongueSpawnPoint.rotation);
        bullet.tongueEnd = transform;
    }
}