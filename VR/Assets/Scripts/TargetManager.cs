using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetManager : MonoBehaviour
{
    public Target[] targets;
    public List<GameObject> gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        GameObject target = Instantiate(targets[0].target, targets[0].transforms[0].position, Quaternion.identity);
        gameObjects.Add(target);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targets[gameObjects.IndexOf(gameObject)].transforms[targets[gameObjects.IndexOf(gameObject)].nextPosition].position, targets[gameObjects.IndexOf(gameObject)].speed * Time.deltaTime);
            if (gameObject.transform.position == targets[gameObjects.IndexOf(gameObject)].transforms[targets[gameObjects.IndexOf(gameObject)].nextPosition].position)
            {
                if (targets[gameObjects.IndexOf(gameObject)].nextPosition + 1 != targets[gameObjects.IndexOf(gameObject)].transforms.Count)
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
                        DestroyTarget(gameObject);
                    }
                }
            }
        }
    }
    [Serializable]
    public struct Target
    {
        public GameObject target;
        public List<Transform> transforms;
        public int loopCount;
        public float speed;
        [HideInInspector]
        public int nextPosition;
        [HideInInspector]
        public int currentLoop;
    }
    private void DestroyTarget(GameObject target)
    {
        target.SetActive(false);
    }
}
