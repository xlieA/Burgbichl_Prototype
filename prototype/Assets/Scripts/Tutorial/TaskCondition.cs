using static SubTask;

public class TaskCondition
{
    public Condition Condition { get; set; }
    public SetUp SetUp { get; set; }
    public Reset Reset { get; set; }

    public TaskCondition(Condition condition, SetUp setUp, Reset reset)
    {
        Condition = condition;
        SetUp = setUp;
        Reset = reset;
    }
}
