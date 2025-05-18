using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowWayfindingScript : MonoBehaviour
{
    [Header("Arrow Refs")]
    public GameObject objWithGameScript;

    public GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enabled = false;
    }

    public void CompleteTracking()
    {
        enabled = false;
        target = null;
    }

    public void UpdateTrackingQuestTarget()
    {
        enabled = true;
        List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;
        // find the NPCs
        GameObject[] NPCsList = GameObject.FindGameObjectsWithTag("NPC");

        foreach (Quest quest in activeQuests)
        {
            foreach (QuestStep questStep in quest.questSteps)
            {
                foreach (GameObject npc in NPCsList)
                {
                    if (!questStep.isCompleted)
                    {
                        int NPCId = npc.GetComponent<NPCScript>().NPCId;
                        if (NPCId == questStep.targetId)
                        {
                            print("TARGETING: " + npc);
                            // we found our target.
                            target = npc;
                            return;
                        }
                    }
                }
                // for now, just get the first NPC that shows up as a target.
                // WIP for other target types, like locations.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            /*Vector3 relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;*/
            transform.LookAt(target.transform);
        }
    }
}
