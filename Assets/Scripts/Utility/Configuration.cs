namespace DocCN.Utility
{
    public class Configuration
    {
        public string domain { get; private set; }
        public string schema { get; private set; }
        public string apiHost { get; private set; }
        public string pageBase { get; private set; }
        

        public static readonly Configuration Instance = new Configuration();

        private Configuration()
        {
        }

        static Configuration()
        {
#if DOC_BUILD_LOCAL || UNITY_EDITOR
            Instance.domain = "doc.unity.cn";
            Instance.schema = "http";
            Instance.pageBase = "";
#endif

#if DOC_BUILD_TEST || UNITY_EDITOR
            Instance.domain = "connect-test.unity.com";
            Instance.schema = "https";
            Instance.pageBase = "/doc";
#endif

#if DOC_BUILD_PRD
            Instance.domain = "connect.unity.com";
            Instance.schema = "https";
            Instance.pageBase = "/doc";
#endif
            Instance.apiHost = $"{Instance.schema}://{Instance.domain}";
        }
    }
}