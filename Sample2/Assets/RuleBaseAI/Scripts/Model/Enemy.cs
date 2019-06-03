using System.Collections;
using UnityEngine;
public class Enemy : Actor {

    private ArrayList usedActionList = new ArrayList();
    private AIRoutine[] routineList;
    // 初期化
    public Enemy(EnemyParams param) : base(param)
    {
        this.routineList = param.RoutineList;
    }

    public int DetermineEnemyAction(int turnCount, float playerHPPecentage)
    {
        int counter = 0;
        // 初期値
        this.action = ActionList.Attack;
        foreach (var routine in this.routineList)
        {
            if (SatisfyAllConditions(routine, turnCount, playerHPPecentage))
            {
                this.action = routine.Action;
                return counter;
            }
            counter++;
        }
        return -1;
    }

    // HACK : インデント整理
    private bool SatisfyAllConditions (
        AIRoutine routine, int turnCount, float playerHPPecentage)
    {
        if (!SatisfyTurnCondition(routine, turnCount)) { return false; }
        if (!SatisfyEnemyHPCondition(routine)) { return false; }
        if (!SatisfyPlayerHPCondition(routine, playerHPPecentage)) { return false; }
        if (!EnableAction(routine)) { return false; }
        return true;
    }

    private bool SatisfyTurnCondition(AIRoutine routine, int turnCount) {
        if (!routine.UseTurnValue) { return true; }
        if (routine.ConstOrMulti == 0) { return turnCount == routine.TurnValue; }
        return turnCount % routine.TurnValue == 0;
    }

    private bool SatisfyEnemyHPCondition(AIRoutine routine) {
        float HPPercentage = this.CurrentHPPercentage;
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
        if (!ActionAlreadyUsed(routine.Action)) {
            usedActionList.Add(routine.Action);
            return true;
        }
        return false;
    }
    private bool ActionAlreadyUsed(ActionList action) {
        return usedActionList.Contains(action);
    }
}