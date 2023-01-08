using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private GameObject Enemy;
    private GameObject[] AllEnemies;
    private float distance;
    private float FireForce = 20f;
    private PlayerMovement player;
    private RandomSpawner ToGetlevel;
    private GameObject[] AllEnemyBullets;
    private GameObject AnEnemyBullet;

    [SerializeField]
    private GameObject ExplodeEffect;
    

    private void CreateExplosionFX()
    {
        int SpawnHere = Random.Range(0, 360);
        for(int i = 0; i < 4; i++)
        {
            Instantiate(ExplodeEffect, transform.position, Random.rotation);
        }
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        ToGetlevel = GameObject.FindObjectOfType<RandomSpawner>();
    }
    private void Update()
    {


        AllEnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < AllEnemies.Length; i++)
        {
            Enemy = AllEnemies[i];
            if (Enemy != null)
            {
                Vector2 Diff = Enemy.transform.position - transform.position;
                distance = Diff.magnitude;
                if (Enemy.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
                {
                    CreateExplosionFX();
                    player.KillHistory++;
                    ToGetlevel.PlayExplodeSFX();
                    Destroy(Enemy);
                    if (player.KillHistory == ToGetlevel.level)
                    {
                        ToGetlevel.LevelCleared = true;
                        player.KillHistory = 0;
                    }
                    Destroy(gameObject);
                }
            }
           
        }

        for(int i = 0; i < AllEnemyBullets.Length; i++)
        {
            AnEnemyBullet = AllEnemyBullets[i];
            Vector2 Diff = AnEnemyBullet.transform.position - transform.position;
            distance = Diff.magnitude;
            if(AnEnemyBullet.GetComponent<CircleCollider2D>().radius + GetComponent<CircleCollider2D>().radius > distance)
            {
                ToGetlevel.PlayExplodeSFX();
                CreateExplosionFX();
                Destroy(AnEnemyBullet);
                Destroy(gameObject);
            }
        }

        
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * FireForce * Time.deltaTime);

    }
}
