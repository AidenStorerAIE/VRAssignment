using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetManager : MonoBehaviour
{
    [Header("Target Management")]
    public Target[] targets;
    public List<int> numberToSpawn;
    public List<GameObject> gameObjects;
    public List<Transform> moveToLocation;
    private int groupCount;
    private int targetCount;

    public bool running;
    private int countToNextSpawn;
    public int startCountToNextSpawn;

    //references
    XRInteractionManager interactionManager;
    ScoreManager scoreManager;
    AudioSource audioSource;
    MusicManager musicManager;

    void Start()
    {
        interactionManager = FindObjectOfType<XRInteractionManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        audioSource = GetComponent<AudioSource>();
        musicManager = FindObjectOfType<MusicManager>();
        countToNextSpawn = startCountToNextSpawn;
    }

    void Update()
    {
        //don't run if no targets or not active
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
                    //if at target location begin going to next location
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
                                gameObject.GetComponent<TargetObj>().audioSourceTwo.volume = 0;
                                gameObject.GetComponent<TargetObj>().score = 0;
                                DropTarget(gameObject, true);
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
                        gameObject.GetComponent<TargetObj>().audioSourceTwo.volume = 0;
                        gameObject.GetComponent<TargetObj>().score = 0;
                        DropTarget(gameObject, true);
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
        D1,
        D2,
        D3,
    }

    //if target is shot
    public void DropTarget(GameObject target, bool addScore)
    {
        //if bad target is hit
        if (target.gameObject.tag != "BadTarget")
        {
            countToNextSpawn--;
        }

        //plays drop animation
        target.GetComponent<Animator>().SetTrigger("TargetDrop");
        target.GetComponent<TargetObj>().audioSourceOne.Play();
        target.GetComponent<TargetObj>().audioSourceTwo.Play();
        targets[gameObjects.IndexOf(target)].active = false;

        //spawn next wave if no more targets
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
                }
                CheckIfMoving();
            }
            //if current wave was last wave (Game Over)
            else
            {
                ClearTargets();
                musicManager.Stop();
                running = false;
                CheckIfMoving();
                scoreManager.Done();
                return;
            }
        }
        //add score
        int score = target.GetComponent<TargetObj>().score;
        if (addScore) scoreManager.AddScore(score);
    }

    //Initial spawn
    public void InitialTarget()
    {
        if (!running)
        {
            musicManager.Play();
            running = true;
            if (gameObjects.Count > 0)
            {
                foreach (GameObject gameObj in gameObjects)
                {
                    targets[gameObjects.IndexOf(gameObj)].nextPosition = 0;
                    targets[gameObjects.IndexOf(gameObj)].currentLoop = 0;
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
            CheckIfMoving();
            scoreManager.StartTimer();
        }
    }

    public void Stop()
    {
        running = false;
        musicManager.Stop();
        audioSource.Stop();
        //drops targets
        foreach (GameObject target in gameObjects)
        {
            target.GetComponent<Animator>().SetTrigger("TargetDrop");
            target.GetComponent<TargetObj>().audioSourceOne.Play();
            target.GetComponent<TargetObj>().audioSourceTwo.Play();
            targets[gameObjects.IndexOf(target)].active = false;
        }
    }
    public void ClearTargets()
    {
        foreach (GameObject target in gameObjects)
        {
            if (targets[gameObjects.IndexOf(target)].active == true)
            {
                target.GetComponent<TargetObj>().audioSourceTwo.volume = 0;
                target.GetComponent<Animator>().SetTrigger("TargetDrop");
                targets[gameObjects.IndexOf(target)].active = false;
            }
        }
    }
    private void CheckIfMoving()
    {
        foreach (GameObject go in gameObjects)
        {
            if (targets[gameObjects.IndexOf(go)].active == true && targets[gameObjects.IndexOf(go)].locations.Count > 1)
            {
                audioSource.Play();
                return;
            }
        }
        audioSource.Stop();
        return;
    }
}
