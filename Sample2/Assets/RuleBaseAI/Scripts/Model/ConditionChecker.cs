using System;
using System.Collections;

public class ConditionChecker {

    ArrayList usedActionList = new ArrayList();

    // HACK : インデント整理
    public AIRoutine DetermineEnemyAction (
        AIRoutine[] routineList, int turnCount,
        float playerHPPecentage, float enemyHPPercentage)
    {
        foreach (var routine in routineList)
        {
            if (SatisfyAllConditions(routine, turnCount, playerHPPecentage, enemyHPPercentage))
            {
               return routine;
            }
        }
        return new AIRoutine();
    }

    // HACK : インデント整理
    private bool SatisfyAllConditions (
        AIRoutine routine, int turnCount,
        float playerHPPecentage, float enemyHPPercentage)
    {
        if (!SatisfyTurnCondition(routine, turnCount)) { return false; }
        if (!SatisfyEnemyHPCondition(routine, enemyHPPercentage)) { return false; }
        if (!SatisfyPlayerHPCondition(routine, playerHPPecentage)) { return false; }
        if (!EnableAction(routine)) { return false; }
        return true;
    }

    private bool SatisfyTurnCondition(AIRoutine routine, int turnCount) {
        if (!routine.UseTurnValue) { return true; }
        if (routine.ConstOrMulti == 0) { return turnCount == routine.TurnValue; }
        return turnCount % routine.TurnValue == 0;
    }

    private bool SatisfyEnemyHPCondition(AIRoutine routine, float HPPercentage) {
        if (!routine.EnemyHPTrigger) { return true; }
        if (routine.EnemyHP_ConditionRange == 0) { return HPPercentage > routine.EnemyHP_ConditionValue; }
        return HPPercentage < routine.EnemyHP_ConditionValue;
    }

    private bool SatisfyPlayerHPCondition(AIRoutine routine, float HPPercentage) {
        if (!routine.PlayerHPTrigger) { return true; }
        if (routine.PlayerHP_ConditionRange == 0) { return HPPercentage > routine.PlayerHP_ConditionValue; }
        return HPPercentage < routine.PlayerHP_ConditionValue;
    }

    private bool EnableAction(AIRoutine routine) {
        if (!routine.ActionOnce) { return true; }
        if (!ActionAlreadyUsed(routine.ActionID)) {
            usedActionList.Add(routine.ActionID);
            return true;
        }
        return false;
    }
    private bool ActionAlreadyUsed(int routineListindex) {
        return usedActionList.Contains(routineListindex);
    }
}