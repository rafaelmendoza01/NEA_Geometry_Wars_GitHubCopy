using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EscapeEnemy : EnemyMovement
{
    GameObject[] PlayerBullets;
    List<BulletAndDistance> Distance2 = new List<BulletAndDistance>(0);

    //source for the merge algorithm
    //https://www.w3resource.com/csharp-exercises/searching-and-sorting-algorithm/searching-and-sorting-algorithm-exercise-7.php
    static List<BulletAndDistance> MergeSort(List<BulletAndDistance> Unsorted)
    {
        if(Unsorted.Count <= 1)
        {
            return Unsorted;
        }
        List<BulletAndDistance> left = new List<BulletAndDistance>();
        List<BulletAndDistance> right = new List<BulletAndDistance>();
        int middle = Unsorted.Count / 2;

        for(int i = 0; i < middle; i++)
        {
            left.Add(Unsorted[i]);
        }
        for(int i =  middle; i < Unsorted.Count; i++)
        {
            right.Add(Unsorted[i]);
        }

        left = MergeSort(left);
        right = MergeSort(right);
        return Merge(left, right);
    }

    private static List<BulletAndDistance> Merge(List<BulletAndDistance> left, List<BulletAndDistance> right)
    {
        List<BulletAndDistance> result = new List<BulletAndDistance>();
        while(left.Count > 0 || right.Count > 0)
        {
            if(left.Count > 0 && right.Count > 0)
            {
                if(left[0].Distance <= right[0].Distance)
                {
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    result.Add(right[0]);
                    right.RemoveAt(0);
                }
            } 
            else if(left.Count > 0)
            {
                result.Add(left[0]);
                left.RemoveAt(0);
            } 
            else if(right.Count > 0)
            {
                result.Add(right[0]);
                right.RemoveAt(0);
            }
        }
        return result;
    }

    public struct BulletAndDistance
    {
        public GameObject CurrentBullet;
        public float Distance;

        public BulletAndDistance(GameObject TheBullet, float TheDistance)
        {
            CurrentBullet = TheBullet;
            Distance = TheDistance;
        }
    }
    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        List<BulletAndDistance> Distances = new List<BulletAndDistance>();
        for (int i = 0; i < Distance2.Count; i++)
        {
            if(Distance2[i].CurrentBullet == null)
            {
                Distance2.RemoveAt(i);
            }
        }

        if (player != null)
        {
            Vector2 Diff = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
            distance = Diff.magnitude;
        }

        PlayerBullets = GameObject.FindGameObjectsWithTag("Bullet");
        if(PlayerBullets.Length > 0)
        {
            for (int i = 0; i < PlayerBullets.Length; i++)
            {
                GameObject NextBullet = PlayerBullets[i];
                Vector2 Difference = NextBullet.GetComponent<Transform>().position - GetComponent<Transform>().position;
                if (Difference.magnitude <= 4f)
                {
                    Distances.Add(new BulletAndDistance(NextBullet, Difference.magnitude));
                }
            }
            Distances = MergeSort(Distances);
            Distance2 = Distances;
        }
    }

    private void FixedUpdate()
    {
        if(Distance2.Count == 0)
        {
            moveSpeed = 2.5f;
            if (player != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                if (player.GetComponent<CircleCollider2D>().radius + radius > distance)
                {
                    NeedToGetStats.PlayDeathSFX();
                    NeedToGetStats.Life--;
                    NeedToGetStats.PlayerSpawnState = RandomSpawner.PlayerJustSpawned.SpawnPlayerAgain;
                    Destroy(player);
                }
            }
        }
        else
        {
            moveSpeed = 10f;
            GameObject FocusBullet = Distance2[0].CurrentBullet;
            if (FocusBullet != null)
            {
                Bullet ToGetVectorOfThisBullet = FocusBullet.GetComponent<Bullet>();
                Vector2 MoveVector = Vector2.Perpendicular(ToGetVectorOfThisBullet.tempVector);
                transform.Translate(MoveVector * moveSpeed * Time.deltaTime);
            }
            
        }
    }
}
