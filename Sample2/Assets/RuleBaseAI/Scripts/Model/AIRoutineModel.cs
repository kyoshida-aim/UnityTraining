public class AIRoutineModel
{
    private AIRoutine aiRoutine;
    private bool isOneShot;

    public AIRoutineModel(AIRoutine routine)
    {
        aiRoutine = routine;
    }

    public bool IsAplicable(int turn, float playerHp, float enemyHp)
    {
        if (!Usable())
            return false;

        return IsAplicableTurnCondition(turn)
            || IsAplicableEneryHpCondition(enemyHp)
            || IsAplicablePlayerHpCondition(playerHp);
    }

    bool IsAplicableTurnCondition(int turn)
    {
        if (!aiRoutine.UseTurnValue)
            return false;

        if (aiRoutine.ConstOrMulti == 0)
            return turn == aiRoutine.TurnValue;

        return turn % aiRoutine.TurnValue == 0;
    }

    bool IsAplicableEneryHpCondition(float percentage)
    {
        if (!aiRoutine.EnemyHPTrigger)
            return false;
        if (aiRoutine.EnemyHP_ConditionRange == 0)
            return percentage > aiRoutine.EnemyHP_ConditionValue;
        return percentage < aiRoutine.EnemyHP_ConditionValue;
    }

    bool IsAplicablePlayerHpCondition(float percentage)
    {
        if (!aiRoutine.PlayerHPTrigger)
            return false;
        if (aiRoutine.PlayerHP_ConditionRange == 0)
            return percentage > aiRoutine.PlayerHP_ConditionValue;
        return percentage < aiRoutine.PlayerHP_ConditionValue;
    }

    bool Usable()
    {
        if (!aiRoutine.ActionOnce)
            return true;

        if (!isOneShot)
        {
            isOneShot = true;
            return true;
        }

        return false;
    }

    public bool IsSame(AIRoutine routine)
    {
        return aiRoutine == routine;
    }

    public string CreateAction()
    {
        if (aiRoutine.ActionID == 0)
            return"Attack";
        else if (aiRoutine.ActionID == 1)
            return"Heal";
        return"Wait";
    }
}