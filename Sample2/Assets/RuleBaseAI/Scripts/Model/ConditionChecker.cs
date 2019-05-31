using System;
using System.Collections;

public class ConditionChecker {

    ArrayList usedActionList = new ArrayList();

    public string ReturnOK() {
        return "OK!";
    }

    // HACK : インデント整理
    public AIRoutine DetermineEnemyAction (
        AIRoutine[] routineList, int turnCount,
        float playerHPPecentage, float enemyHPPercentage)
    {
        foreach (var routine in routineList)
        {
            if (satisfyAllConditions(routine, turnCount, playerHPPecentage, enemyHPPercentage))
            {
               return routine;
            }
        }
        return new AIRoutine();
    }

    // HACK : インデント整理
    private bool satisfyAllConditions (
        AIRoutine routine, int turnCount,
        float playerHPPecentage, float enemyHPPercentage)
    {
        if (!satisfyTurnCondition(routine, turnCount)) { return false; }
        if (!satisfyEnemyHPCondition(routine, enemyHPPercentage)) { return false; }
        if (!satisfyPlayerHPCondition(routine, playerHPPecentage)) { return false; }
        if (!enableAction(routine)) { return false; }
        return true;
    }

    private bool satisfyTurnCondition(AIRoutine routine, int turnCount) {
        if (!routine.UseTurnValue) { return true; }
        if (routine.ConstOrMulti == 0) { return turnCount == routine.TurnValue; }
        return turnCount % routine.TurnValue == 0;
    }

    private bool satisfyEnemyHPCondition(AIRoutine routine, float HPPercentage) {
        if (!routine.EnemyHPTrigger) { return true; }
        if (routine.EnemyHP_ConditionRange == 0) { return HPPercentage > routine.EnemyHP_ConditionValue; }
        return HPPercentage < routine.EnemyHP_ConditionValue;
    }

    private bool satisfyPlayerHPCondition(AIRoutine routine, float HPPercentage) {
        if (!routine.PlayerHPTrigger) { return true; }
        if (routine.PlayerHP_ConditionRange == 0) { return HPPercentage > routine.PlayerHP_ConditionValue; }
        return HPPercentage < routine.PlayerHP_ConditionValue;
    }

    private bool enableAction(AIRoutine routine) {
        if (!routine.ActionOnce) { return true; }
        if (!actionAlreadyUsed(routine.ActionID)) {
            usedActionList.Add(routine.ActionID);
            return true;
        }
        return false;
    }
    private bool actionAlreadyUsed(int routineListindex) {
        return usedActionList.Contains(routineListindex);
    }
}