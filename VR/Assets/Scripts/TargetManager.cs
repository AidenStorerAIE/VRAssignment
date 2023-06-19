using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetManager : MonoBehaviour
{
    public Target[] targets;
    public List<int> numberToSpawn;
    public List<GameObject> gameObjects;
    public List<GameObject> gameObjectsToAdd;
    public List<Transform> moveToLocation;
    public int groupCount;
    public int targetCount;
    [HideInInspector]
    public bool running;
    public int countToNextSpawn;
    // Start is called before the first frame update
    void Start()
    {
        InitialTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObjects.Count > 0)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (targets[gameObjects.IndexOf(gameObject)].active == true)
                {
                    int locationValue = Convert.ToInt32(targets[gameObjects.IndexOf(gameObject)].locations[targets[gameObjects.IndexOf(gameObject)].nextPosition]);
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveToLocation[locationValue].position, targets[gameObjects.IndexOf(gameObject)].speed * Time.deltaTime);
                    if (gameObject.transform.position == moveToLocation[locationValue].position)
                    {
                        if (targets[gameObjects.IndexOf(gameObject)].nextPosition + 1 != targets[gameObjects.IndexOf(gameObject)].locations.Count)
                        {
                            targets[gameObjects.IndexOf(gameObject)].nextPosition += 1;
                        }
                        else
                        {
                            if (targets[gameObjects.IndexOf(gameObject)].currentLoop != targets[gameObjects.IndexOf(gameObject)].loopCount)
                            {
                                targets[gameObjects.IndexOf(gameObject)].nextPosition = 0;
                                targets[gameObjects.IndexOf(gameObject)].currentLoop += 1;
                            }
                            else
                            {
                                targets[gameObjects.IndexOf(gameObject)].active = false;
                                NextTargetLoss(gameObject);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    [Serializable]
    public struct Target
    {
        public GameObject target;
        public List<Locations> locations;
        public int loopCount;
        public float speed;
        [HideInInspector]
        public int nextPosition;
        [HideInInspector]
        public int currentLoop;
        public bool active;
    }
    public enum Locations
    {
        A1,
        A2,
        A3,
        B1,
        B2,
        B3,
        C1,
        C2,
        C3,
    }
    private void NextTargetLoss(GameObject target)
    {
        countToNextSpawn--;
        if (countToNextSpawn <= 0)
        {
            if (groupCount < numberToSpawn.Count)
            {
                countToNextSpawn = numberToSpawn[groupCount];
                groupCount++;
                for (int i = 0; i < numberToSpawn[groupCount - 1]; i++)
                {
                    targetCount++;
                    int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[targets[targetCount].nextPosition]);
                    GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
                    gameObjects.Add(createdTarget);
                    targets[gameObjects.IndexOf(createdTarget)].active = true;
                }
            }
        }
        target.GetComponent<Animator>().SetTrigger("TargetDrop");
        targets[gameObjects.IndexOf(target)].active = false;
        return;
    }
    public void NextTargetWin(GameObject target)
    {
        countToNextSpawn--;
        if (countToNextSpawn <= 0)
        {
            if (groupCount < numberToSpawn.Count)
            {
                countToNextSpawn = numberToSpawn[groupCount];
                groupCount++;
                for (int i = 0; i < numberToSpawn[groupCount - 1]; i++)
                {
                    targetCount++;
                    int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[targets[targetCount].nextPosition]);
                    GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
                    gameObjects.Add(createdTarget);
                    targets[gameObjects.IndexOf(createdTarget)].active = true;
                }
            }
        }
        target.GetComponent<Animator>().SetTrigger("TargetDrop");
        targets[gameObjects.IndexOf(target)].active = false;
        return;
    }
    private void InitialTarget()
    {
        countToNextSpawn = numberToSpawn[groupCount];
        groupCount++;
        targetCount = -1;
        for (int i = 0; i < numberToSpawn[groupCount - 1]; i++)
        {
            targetCount++;
            int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[targets[targetCount].nextPosition]);
            GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
            gameObjects.Add(createdTarget);
            targets[gameObjects.IndexOf(createdTarget)].active = true;
        }
    }
}
