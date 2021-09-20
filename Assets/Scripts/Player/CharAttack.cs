using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CharAttack : NetworkBehaviour
{
    [Header("Main")]
    [SerializeField] GameObject WeaponPrefab;
    public GameObject Hand;
    public GameObject Weapon;

    public void Attack(){

        Quaternion rot = Quaternion.Euler(0f,0f,Hand.transform.rotation.eulerAngles.z - 90f);
        SpawnProjectile(rot);
    }

    [Command]
    private void SpawnProjectile(Quaternion rot){
        
        GameObject projectile = NetworkManager.Instantiate(WeaponPrefab,Weapon.transform.position, rot);
        NetworkServer.Spawn(projectile);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
