using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetManagerVarient : MonoBehaviour
{
    public Target[] targets;
    public List<int> numberToSpawn;
    public List<GameObject> gameObjects;
    public List<GameObject> badTargets;
    public List<Transform> moveToLocation;
    [HideInInspector]
    public int groupCount;
    [HideInInspector]
    public int targetCount;
    public bool running;
    [HideInInspector]
    int countToNextSpawn;
    public int startCountToNextSpawn;
    XRInteractionManager interactionManager;
    ScoreManager scoreManager;  

    //debug
    public bool active = true;

    void Start()
    {
        interactionManager = FindObjectOfType<XRInteractionManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        active = true;
        countToNextSpawn = startCountToNextSpawn;
        InitialTarget();
    }

    void Update()
    {
        if (gameObjects.Count <= 0 || !running)
            return;

        foreach (GameObject gameObject in gameObjects)
        {
            if (targets[gameObjects.IndexOf(gameObject)].active == true)
            {
                if (targets[gameObjects.IndexOf(gameObject)].locations.Count > 1)
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
                                DropTarget(gameObject, 0);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (targets[gameObjects.IndexOf(gameObject)].timing == false)
                    {
                        targets[gameObjects.IndexOf(gameObject)].startTime = Time.time;
                        targets[gameObjects.IndexOf(gameObject)].timing = true;
                        return;
                    }
                    if (Time.time > (targets[gameObjects.IndexOf(gameObject)].startTime + targets[gameObjects.IndexOf(gameObject)].lifeTime))
                    {
                        targets[gameObjects.IndexOf(gameObject)].active = false;
                        targets[gameObjects.IndexOf(gameObject)].timing = false;
                        DropTarget(gameObject, 0);
                        return;
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
        public float lifeTime;
        [HideInInspector]
        public int nextPosition;
        [HideInInspector]
        public int currentLoop;
        [HideInInspector]
        public bool timing;
        [HideInInspector]
        public float startTime;
        [HideInInspector]
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

    public void DropTarget(GameObject target, int score)
    {
        if (target.gameObject.tag != "BadTarget")
        {
            countToNextSpawn--;
        }
        target.GetComponent<Animator>().SetTrigger("TargetDrop");
        targets[gameObjects.IndexOf(target)].active = false;
        if (countToNextSpawn <= 0)
        {
            if (groupCount < numberToSpawn.Count && running)
            {
                ClearTargets();
                countToNextSpawn = numberToSpawn[groupCount];
                groupCount++;
                for (int i = 0; i < numberToSpawn[groupCount - 1]; i++)
                {
                    targetCount++;
                    int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[0]);
                    GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
                    gameObjects.Add(createdTarget);
                    targets[gameObjects.IndexOf(createdTarget)].active = true;
                    targets[gameObjects.IndexOf(createdTarget)].timing = false;
                    createdTarget.GetComponent<XRSimpleInteractable>().interactionManager = interactionManager;
                    if (createdTarget.tag == "BadTarget")
                    {
                        countToNextSpawn--;
                    }
                    //scoreManager.AddScore(score);
                }
            }
            else
            {
                ClearTargets();
                running = false;
                return;
                //REMOVE THIS TO SETUP RESET
                //End of Round
                //InitialTarget();
            }
        }
        return;
    }
    //Start of round
    public void InitialTarget()
    {
        if (!running)
        {
            running = true;
            if (gameObjects.Count > 0)
            {
                foreach (GameObject gameObj in gameObjects)
                {
                    Destroy(gameObj);
                }
                gameObjects.Clear();
            }
            groupCount = 0;
            countToNextSpawn = numberToSpawn[groupCount];
            groupCount++;
            targetCount = -1;
            for (int i = 0; i < numberToSpawn[groupCount - 1]; i++)
            {
                targetCount++;
                int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[0]);
                GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
                gameObjects.Add(createdTarget);
                targets[gameObjects.IndexOf(createdTarget)].active = true;
                targets[gameObjects.IndexOf(createdTarget)].timing = false;
                createdTarget.GetComponent<XRSimpleInteractable>().interactionManager = interactionManager;
                if (createdTarget.tag == "BadTarget")
                {
                    countToNextSpawn--;
                }
            }
        }
    }

    public void Reset()
    {
        active = true;
        countToNextSpawn = startCountToNextSpawn;
    }

    public void Stop()
    {
        foreach (GameObject target in gameObjects)
        {
            running = false;
            DropTarget(target, 0);
        }
    }
    public void ClearTargets()
    {
        foreach (GameObject target in gameObjects)
        {
            target.GetComponent<Animator>().SetTrigger("TargetDrop");
            targets[gameObjects.IndexOf(target)].active = false;
        }
    }
}
