namespace DocCN.Utility
{
    public class Configuration
    {
        public string apiHost { get; private set; }

        public static readonly Configuration instance = new Configuration();

        private Configuration()
        {
        }

        static Configuration()
        {
#if DOC_BUILD_LOCAL || UNITY_EDITOR
            instance.apiHost = "https://connect-local.unity.com:8443";
#endif

#if DOC_BUILD_TEST
            instance.apiHost = "https://connect-test.unity.com";
#endif

#if DOC_BUILD_PRD
            instance.apiHost = "https://connect.unity.com";
#endif
        }
    }
}