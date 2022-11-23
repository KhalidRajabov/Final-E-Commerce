using Final_E_Commerce.Entities;
using Final_E_Commerce.Hubs;
using TableDependency.SqlClient;

namespace Final_E_Commerce.SubscribeTableDependency
{
    public class SubscribeMessagesTableDependency
    {
        SqlTableDependency<Messages>? tableDependency;
        MessagesHub messageHub;

        public SubscribeMessagesTableDependency(MessagesHub messageHub)
        {
            this.messageHub = messageHub;
        }

        public void SubscribeTableDependency()
        {
            string connectionString= "Server=DESKTOP-PEBTDQ1\\SQLEXPRESS;Database=FinalProject;Trusted_Connection=True;MultipleActiveResultSets=true";
            tableDependency = new SqlTableDependency<Messages>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Messages> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                messageHub.SendMessages();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Messages)} SqlTableDependency error: {e.Error.Message}");
        }
        
    }
}