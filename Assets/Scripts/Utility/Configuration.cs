namespace Unity.DocZh.Utility
{
    public class Configuration
    {
        public string domain { get; private set; }
        public string schema { get; private set; }
        public string apiHost { get; private set; }
        public string pageBase { get; private set; }
        
        public string cdnPrefix { get; private set; }
        

        public static readonly Configuration Instance = new Configuration();

        private Configuration()
        {
        }

        static Configuration()
        {
#if DOC_BUILD_LOCAL
            Instance.domain = "doc.unity.cn";
            Instance.schema = "http";
            Instance.pageBase = "";
            Instance.cdnPrefix = "https://unity-connect-dev.storage.googleapis.com/doc_cn/resources";
#endif

#if DOC_BUILD_TEST
            Instance.domain = "connect-test.unity.com";
            Instance.schema = "https";
            Instance.pageBase = "/doc";
            Instance.cdnPrefix = "https://unity-connect-dev.storage.googleapis.com/doc_cn/resources";
#endif

#if DOC_BUILD_PRD || UNITY_EDITOR
            Instance.domain = "connect.unity.com";
            Instance.schema = "https";
            Instance.pageBase = "/doc";
            Instance.cdnPrefix = "https://connect-cdn-china.unitychina.cn/doc_cn/resources";
#endif
            Instance.apiHost = $"{Instance.schema}://{Instance.domain}";
        }
    }
}