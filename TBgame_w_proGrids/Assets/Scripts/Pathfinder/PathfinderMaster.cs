using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SA
{
    public class PathfinderMaster : MonoBehaviour
    {
        public static PathfinderMaster masterPathfinder; 
        List<Pathfinder> currJobs = new List<Pathfinder>(); // list of jobs currently active
        List<Pathfinder> toDoJobs = new List<Pathfinder>(); // list of jobs to do that are pending

        public int MaxJobs = 5; // how many threads can be active at a time
        public float timerThreashold = 0.5f; // how long before a job times out

        private void Awake()
        {

            /*
             *Awake is used to initialize any variables or game state before the game starts. 
             *Awake is called only once during the lifetime of the script instance. 
             *Awake is always called before any Start functions.
             * https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
             */

            masterPathfinder = this; // inizialize the game's pathfinding controller
        }

        public void Update()
        {
            int i = 0;
            float delta = Time.deltaTime; //the amount of time between the current fram and the previous update frame
            while (i < currJobs.Count)
            {
                if (currJobs[i].jobDone)
                {
                    currJobs[i].NotifyComplete();
                    currJobs.RemoveAt(i);
                }
                else
                {
                    currJobs[i].timer += delta;

                    //if the job takes too long then there is probably something wrong with the job - force it to be complete (will return NULL for targetPath)
                    if (currJobs[i].timer > timerThreashold)
                    {
                        currJobs[i].jobDone = true;
                    }

                    i++;
                }
            }

            if(toDoJobs.Count > 0 && currJobs.Count < MaxJobs)
            {
                Pathfinder job = toDoJobs[0];
                toDoJobs.RemoveAt(0);
                currJobs.Add(job);

                Thread jobThread = new Thread(job.FindPath);
                jobThread.Start();
            }
        }

        public void RequestPathFind(GridCharacter character, Node start, Node target, Pathfinder.PathfindingComplete callback, GridManager gm)
        {
            Pathfinder newJob = new Pathfinder(character, start, target, callback, gm);
            toDoJobs.Add(newJob);
        }
    }
}
