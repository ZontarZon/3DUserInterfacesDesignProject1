using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class CompassBarImageScript : MonoBehaviour
{
    [Header ("References")]
    public GameObject objWithGameScript;

    private List<GameObject> NPCsToTrack = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearCompassMarkers()
    {
        NPCsToTrack.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void ClearCompassMarkerForTarget(int targetId) 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int targetObjId = transform.GetChild(i).GetComponent<CompassMarkerScript>().associatedTargetObjId;
            if (targetObjId == targetId)
            {
                Destroy(transform.GetChild(i).gameObject);
                return;
            }
        }
    }

    public void AddCompassMarkers()
    {
        print("ADDING COMPASS MARKERS");
        List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;

        GameObject[] NPCsList = GameObject.FindGameObjectsWithTag("NPC");

        foreach (Quest quest in activeQuests)
        {
            foreach (QuestStep questStep in quest.questSteps)
            {
                foreach (GameObject npc in NPCsList)
                {
                    int NPCId = npc.GetComponent<NPCScript>().NPCId;

                    if (!questStep.isCompleted && NPCId == questStep.targetId)
                    {
                        // add the npc.
                        NPCsToTrack.Add(npc);
                        GameObject newCompassMarkerContainer = new GameObject("CompassMarkerContainer"); //Create the GameObject
                        TextMeshProUGUI textObj = newCompassMarkerContainer.AddComponent<TextMeshProUGUI>();
                        textObj.text = "V";

                        textObj.color = Color.black;
                        textObj.fontSize = 30;
                        newCompassMarkerContainer.transform.SetParent(transform);
                        textObj.tag = "CompassMarker";
                        newCompassMarkerContainer.AddComponent<CompassMarkerScript>();
                        newCompassMarkerContainer.GetComponent<CompassMarkerScript>().setAssociatedTargetId(npc, NPCId);

                        RectTransform rectTransform = newCompassMarkerContainer.GetComponent<RectTransform>();
                        rectTransform.localScale = new Vector3(1, 1, 1);
                        rectTransform.sizeDelta = new Vector2(20, 20);

                        rectTransform.anchoredPosition.Set(0, 0);
                        rectTransform.anchorMin.Set(0, 1);
                        rectTransform.anchorMax.Set(0, 1);
                        rectTransform.pivot.Set(0, 0);
                        rectTransform.transform.localPosition = new Vector3(0, 0, 0);
                        newCompassMarkerContainer.SetActive(true); //Activate the GameObject

                        print("ADDED COMPASS MARKER");
                    }
                }
            }
        }
    }
}
