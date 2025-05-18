using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public bool isPartOfActiveQuest = false;

    [Header("NPC traits")]
    public string NPCName;
    public int NPCId;
    public TextMeshProUGUI nameText;
    public GameObject questArrow;
    public GameObject objWithGameScript;
    public GameObject player;

    [SerializeField] private LineRenderer Path;
    [SerializeField] private float pathSpeed;
    private float pathHeightOffset = 1.5f;
    private float pathUpdateSpeed = 1f;

    private NavMeshTriangulation triangulation;
    private Coroutine drawPathCoroutine;


    public Quest questNPCCanGive;

  void Awake()
  {
        triangulation = NavMesh.CalculateTriangulation();
  }

    private System.Collections.IEnumerator DrawPath()
    {
        WaitForSeconds wait = new WaitForSeconds(pathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(player.transform.position, transform.position, NavMesh.AllAreas, path))
        {
            print("SUCCESSFULLY CALCULATED PATH FOR " + nameText);
            Path.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                Path.SetPosition(i, path.corners[i] + Vector3.up * pathHeightOffset);
            }
        }
        else
        {
            print("FAILED TO CALCULATE PATH!");
        }
        yield return wait;
  }


    public void createBreadCrumbPath()
    {
        if (drawPathCoroutine != null)
        {
            StopCoroutine(drawPathCoroutine);
        }

        drawPathCoroutine = StartCoroutine(DrawPath());
    }


  // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //createBreadCrumbPath();
        nameText.text = NPCName;
        // begin with a quest via the guide
        if (NPCId == 0)
        {
            List<QuestStep> newQuestSteps = new List<QuestStep>
        {
          new QuestStep(
            newType: QuestStepType.Talk,
            newTargetId: 1,
            newTargetName: "The merchant"
            ),
        new QuestStep(
            newType: QuestStepType.Talk,
            newTargetId: 2,
            newTargetName: "The blacksmith"

            ),
        new QuestStep(
            newType: QuestStepType.Talk,
            newTargetId: 3,
            newTargetName: "The The village elder"
            )
        };
            Quest newQuest = new Quest(
            newTitle: "Talk to the people",
            newDesc: "Talk to each of the merchants: The merchant, the blacksmith, and the village elder.",
            newIsActive: false,
            newQuestId: 1,
            newQuestSteps: newQuestSteps,
            newIsOrderImportant: false
            );
            questNPCCanGive = newQuest;
        }
    }

    // Update is called once per frame
    void Update()
    {
        createBreadCrumbPath();
        if (!isPartOfActiveQuest)
        {
            List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;
            foreach (Quest quest in activeQuests)
            {
                foreach (QuestStep questStep in quest.questSteps)
                {
                    if (questStep.targetId == NPCId && !questStep.isCompleted)
                    {
                        // This person is the target of a quest, so enable the arrow.
                        questArrow.SetActive(true);
                        isPartOfActiveQuest = true;
                        return;
                    }
                }
            }
        }
    }
}
