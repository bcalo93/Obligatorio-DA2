namespace IndicatorsManager.Logger.Database
{
    public class ConnectionStringHandler
    {
        private static ConnectionStringHandler instance;
        public string ConnectionString { get; set; }

        private ConnectionStringHandler() { }

        public static ConnectionStringHandler Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ConnectionStringHandler();
                }
                return instance;
            }
        }
    }
}