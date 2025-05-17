using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestUIScript : MonoBehaviour
{
    [SerializeField] protected UIDocument uiDocument;
    
    [Header("References")]
    public GameObject objWithGameScript;

    public void OnShowQuest(Quest quest)
    {
        if (uiDocument.enabled)
        {
            return;
        }
        uiDocument.enabled = true;
        VisualElement root = uiDocument.rootVisualElement;
        VisualTreeAsset tree = root.visualTreeAssetSource;
        VisualElement UIContainer = root.Query<VisualElement>(name: "UIContainer");
        VisualElement UIContainerContents = UIContainer.Query<VisualElement>(name: "UIContainerContents");
        VisualElement TitleAndUnderlineContainer = UIContainerContents.Query<VisualElement>(name: "TitleAndUnderlineContainer");
        Label TitleLabel = TitleAndUnderlineContainer.Query<Label>(name: "TitleLabel");
        ListView QuestListView = UIContainerContents.Query<ListView>(name: "QuestListView");
        TitleLabel.text = quest.title;

        foreach (QuestStep step in quest.questSteps)
        {
            Label newLabel = new Label(text: step.type == QuestStepType.Talk ? "Talk to " + step.targetName : "Go to " + step.targetName);
            QuestListView.hierarchy.Add(newLabel);
        }

        // Now we add it to our list of active quests
        objWithGameScript.GetComponent<GameScript>().currentQuests.Add(quest);
    }


    void LateUpdate()
    {
        // detect enter key
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Return))
        {
            uiDocument.enabled = false;
            print("DISABLING");
        }
    }
}
