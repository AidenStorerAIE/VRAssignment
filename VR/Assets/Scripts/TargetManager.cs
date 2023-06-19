using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetManager : MonoBehaviour
{
    public Target[] targets;
    public List<int> numberToSpawn;
    public List<GameObject> gameObjects;
    public List<Transform> moveToLocation;
    [HideInInspector]
    public int groupCount;
    [HideInInspector]
    public int targetCount;
    public bool running;
    [HideInInspector]
    public int countToNextSpawn;
    XRInteractionManager interactionManager;
    // Start is called before the first frame update
    void Start()
    {
        InitialTarget();
        interactionManager = FindObjectOfType<XRInteractionManager>();
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
                                    LossTarget(gameObject);
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
                            LossTarget(gameObject);
                            return;
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
    private void LossTarget(GameObject target)
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
                    int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[0]);
                    GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
                    gameObjects.Add(createdTarget);
                    targets[gameObjects.IndexOf(createdTarget)].active = true;
                    targets[gameObjects.IndexOf(createdTarget)].timing = false;
                    createdTarget.GetComponent<XRSimpleInteractable>().interactionManager = interactionManager;
                }
            }
            else
            {
                running = false;
                //REMOVE THIS TO SETUP RESET
                InitialTarget();
            }
        }
        target.GetComponent<Animator>().SetTrigger("TargetDrop");
        targets[gameObjects.IndexOf(target)].active = false;
        return;
    }
    public void DropTarget(GameObject target)
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
                    int initialLocationValue = Convert.ToInt32(targets[targetCount].locations[0]);
                    GameObject createdTarget = Instantiate(targets[targetCount].target, moveToLocation[initialLocationValue].position, Quaternion.identity);
                    gameObjects.Add(createdTarget);
                    targets[gameObjects.IndexOf(createdTarget)].active = true;
                    targets[gameObjects.IndexOf(createdTarget)].timing = false;
                    createdTarget.GetComponent<XRSimpleInteractable>().interactionManager = interactionManager;
                }
            }
            else
            {
                running = false;
                //REMOVE THIS TO SETUP RESET
                InitialTarget();
            }
        }
        target.GetComponent<Animator>().SetTrigger("TargetDrop");
        targets[gameObjects.IndexOf(target)].active = false;
        return;
    }
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
            }
        }
    }
}
