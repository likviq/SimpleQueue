namespace SimpleQueue.DatabaseControll.Jobs
{
    public interface IDatabaseManager
    {
        public void SendMessage();
        public void CheckUserInQueueTable();
    }
}
