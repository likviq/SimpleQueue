using Hangfire;
using SimpleQueue.Data;

namespace SimpleQueue.DatabaseControll.Jobs
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly SimpleQueueDBContext _context;
        public DatabaseManager(SimpleQueueDBContext context)
        {
            _context = context;
        }

        public void SendMessage()
        {
            Console.WriteLine("Some message");
        }

        public void CheckUserInQueueTable()
        {
            var userInQueues = _context.UserInQueues.ToList();

            var queuesId = userInQueues.Select(userInQueue => userInQueue.QueueId).Distinct().ToList();

            foreach (var qId in queuesId)
            {
                try
                {
                    var queuesParticipants = _context.UserInQueues.Where(q => q.QueueId.Equals(qId));

                    var nextParticipantsIsNull = queuesParticipants
                        .Where(participant => participant.NextId.Equals(null))
                        .Select(participant => participant.NextId).ToList();

                    var previousParticipantsIsNull = queuesParticipants
                        .Where(participant => participant.NextId.Equals(null))
                        .Select(participant => participant.PreviousId).ToList();

                    if (nextParticipantsIsNull.Count != previousParticipantsIsNull.Count)
                    {
                        Console.WriteLine($"Something wrong with {qId} queue");
                        throw new Exception(
                            $"Something wrong with {qId} queue");
                    }

                    if (nextParticipantsIsNull.Count != 1)
                    {
                        Console.WriteLine($"There are too much nullable NextId field in queue - {qId}");
                        throw new Exception(
                            $"There are too much nullable NextId field in queue - {qId}");
                    }

                    if (previousParticipantsIsNull.Count != 1)
                    {
                        Console.WriteLine($"There are too much nullable PreviousId field in queue - {qId}");
                        throw new Exception(
                            $"There are too much nullable PreviousId field in queue - {qId}");
                    }

                    if (nextParticipantsIsNull.Count == 1
                        && nextParticipantsIsNull.Count == previousParticipantsIsNull.Count)
                    {
                        Console.WriteLine($"Queue with id - {qId} is ok");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    continue;
                }
            }
        }
    }
}
