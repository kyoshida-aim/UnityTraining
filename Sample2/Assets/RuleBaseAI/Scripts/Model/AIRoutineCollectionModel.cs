using System.Collections.Generic;
using System.Linq;

public class AIRoutineCollectionModel
{
    private List<AIRoutineModel> routines = new List<AIRoutineModel>();

    public AIRoutineCollectionModel(AIRoutine[] list)
    {
        routines = list.Select(r => new AIRoutineModel(r)).ToList();
    }

    public AIRoutineModel FindActionBy(int turnCount, float playerHPPecentage, float enemyHPPercentage)
    {
        return routines.FirstOrDefault(m => m.IsAplicable(turnCount, playerHPPecentage, enemyHPPercentage))
            ?? new AIRoutineModel(new AIRoutine());
    }
}