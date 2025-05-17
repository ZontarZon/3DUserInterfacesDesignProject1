using System.Collections.Generic;
using UnityEngine;

public enum QuestStepType {Talk, Visit};
public class QuestStep
{
  public QuestStepType type;
  public int targetId; // this could be an NPC, collider, etc.
  public string targetName;
  public bool isCompleted = false;

  public QuestStep(QuestStepType newType, int newTargetId, string newTargetName)
  {
    type = newType;
    targetId = newTargetId;
    targetName = newTargetName;
  }
}

public class Quest
{
  public string title;
  public string desc;
  public bool isActive;
  public int questId;
  public bool isOrderImportant;

  public List<QuestStep> questSteps = new List<QuestStep>();

  public Quest(string newTitle, string newDesc, bool newIsActive, int newQuestId, List<QuestStep> newQuestSteps, bool newIsOrderImportant)
  {
    title = newTitle;
    desc = newDesc;
    isActive = newIsActive;
    questId = newQuestId;
    questSteps = newQuestSteps;
    isOrderImportant = newIsOrderImportant;
  }
}
