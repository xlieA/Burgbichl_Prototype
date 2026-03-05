public class SubTask
{
    public TaskDescription taskDescription;

    public delegate bool Condition();
    public delegate void SetUp();
    public delegate void Reset();
    public TaskCondition taskCondition;


    // Adds new condition
    public void AddCondition(Condition condition, SetUp setUp, Reset reset)
    {
        taskCondition = new TaskCondition(condition, setUp, reset);
    }
}
