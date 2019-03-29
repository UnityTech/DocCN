//----------------------------------------------
//            Hbx: WebGL
// Copyright © 2017-2018 Hogbox Studios
// WebGLRetinaTools.cs v3.2
// Developed against WebGL build from Unity 2017.3.1f1
// Tested against Unity 5.6.0f3, 2017.3.1f1, 2018.1.6f1, 2018.2.0f2
//----------------------------------------------

//#define BROTLISTREAM_AVALIABLE

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if BROTLISTREAM_AVALIABLE
using Brotli;
#endif

namespace Hbx.WebGL
{
    /// <summary>
    /// Wizard to edit the webgl fix settings
    /// </summary>

    public class EditWebGLSettingsWizard : ScriptableWizard
    {

#if !UNITY_5_5_OR_NEWER
        public enum WebGLCompressionFormat
        {
            Gzip,
            Brotli,
            Disabled
        };
#endif

        // fix settings
        public bool _autoRunAfterBuild = false;
        public float _desktopScale = 1f;
        public float _mobileScale = 1f;
        public bool _createCopy = false;
        public string _copyAppendString = "-fixed";
        public bool _disableMobileCheck = false;
        public bool _autoLaunchBrowser = false;
        public string _autoLaunchURL = "";

        // messages
        public string _mobileWarningMessage = "";
        public string _genericErrorMessage = "";
        public string _unhandledExceptionMessage = "";
        public string _outOfMemoryMessage = "";
        public string _notEnoughMemoryMessage = "";

        // unity build settings
        public WebGLCompressionFormat _compressionFormat = WebGLCompressionFormat.Gzip;
        public string _emscriptenArgs = "";

        bool _unappliedChanges = false;

        /// <summary>
        /// Static helper to create a wizard.
        /// </summary>

        public static void CreateWizard()
        {
            EditWebGLSettingsWizard w = ScriptableWizard.DisplayWizard<EditWebGLSettingsWizard>("WebGL Tools Settings", "Close", "Apply");

            // fix settings
            w._autoRunAfterBuild = WebGLRetinaTools.AutoRunFix;
            w._desktopScale = WebGLRetinaTools.DesktopScale;
            w._mobileScale = WebGLRetinaTools.MobileScale;
            w._createCopy = WebGLRetinaTools.ShouldCreateCopy;
            w._copyAppendString = WebGLRetinaTools.CopyAppendString;
            w._disableMobileCheck = WebGLRetinaTools.DisableMobileCheck;
            w._autoLaunchBrowser = WebGLRetinaTools.AutoLaunchBrowser;
            w._autoLaunchURL = WebGLRetinaTools.AutoLaunchBrowserURL;

            // messages
            w._mobileWarningMessage = WebGLRetinaTools.MobileWarningMessage;
            w._genericErrorMessage = WebGLRetinaTools.GenericErrorMessage;
            w._unhandledExceptionMessage = WebGLRetinaTools.UnhandledExceptionMessage;
            w._outOfMemoryMessage = WebGLRetinaTools.OutOfMemoryMessage;
            w._notEnoughMemoryMessage = WebGLRetinaTools.NotEnoughMemoryMessage;

            // unity build settings
#if UNITY_5_5_OR_NEWER
            w._compressionFormat = UnityEditor.PlayerSettings.WebGL.compressionFormat;
            w._emscriptenArgs = UnityEditor.PlayerSettings.WebGL.emscriptenArgs;
#endif
        }

        /// <summary>
        /// Called by unity each to a value etc change
        /// </summary>

        void OnWizardUpdate()
        {
            helpString = "";
            if (_unappliedChanges) helpString = "You have unapplied changes.";
            errorString = "";
            if (_compressionFormat == WebGLCompressionFormat.Brotli) errorString = "Brotli compression not supported by this tool, please use Gzip or Disabled";
        }

        protected override bool DrawWizardGUI()
        {
            int originalIndent = EditorGUI.indentLevel;

            GUIStyle staticinfostyle = new GUIStyle();
            staticinfostyle.normal.textColor = Color.gray;

            // fix settings
            EditorGUILayout.LabelField("Fix Settings", staticinfostyle);

            bool lastautorun = _autoRunAfterBuild;
            string autoruntip = "Should the WebGL Retina Fix be automatically run after each build?";
            _autoRunAfterBuild = EditorGUILayout.Toggle(new GUIContent("Auto Run Fix", autoruntip), _autoRunAfterBuild);

            float lastdeskdpr = _desktopScale;
            string deskdprtip = "Scale of full resolution used on desktop, value of 1.0 is full resolution, value of 0.5 is half resolution.";
            _desktopScale = EditorGUILayout.FloatField(new GUIContent("Desktop Scale", deskdprtip), _desktopScale);

            float lastmobdpr = _mobileScale;
            string mobdprtip = "Scale of full resolution used on mobile, value of 1.0 is full resolution, value of 0.5 is half resolution.";
            _mobileScale = EditorGUILayout.FloatField(new GUIContent("Mobile Scale", mobdprtip), _mobileScale);


            bool lastcreatecopy = _createCopy;
            string createcopytip = "Should a copy of the build be created, with the fix being applied to the copy?";
            _createCopy = EditorGUILayout.Toggle(new GUIContent("Create Copy", createcopytip), _createCopy);

            string lastcopystr = _copyAppendString;
            if (_createCopy)
            {
                string copystrtip = "String to append to the build folders name when copying.";
                _copyAppendString = EditorGUILayout.TextField(new GUIContent("Copy Append", copystrtip), _copyAppendString);
            }

            bool lastdisablemobile = _disableMobileCheck;
            string disablemobiletip = "Disable the warning message displayed when running WebGL builds on Mobile devices.";
            _disableMobileCheck = EditorGUILayout.Toggle(new GUIContent("Disable Mobile Check", disablemobiletip), _disableMobileCheck);

            bool lastautolanuch = _autoLaunchBrowser;
            string autolaunch = "Should the browser be automatically opened after a fix is applied?";
            _autoLaunchBrowser = EditorGUILayout.Toggle(new GUIContent("Launch Browser", autolaunch), _autoLaunchBrowser);

            string lastlaunchurl = _autoLaunchURL;
            if (_autoLaunchBrowser)
            {
                string launchurltip = "The base URL for your build, e.g. if my build was called mygame and I can goto http://localhost/mysites/mygame to view it, then just enter http://localhost/mysites/.";
                _autoLaunchURL = EditorGUILayout.TextField(new GUIContent("Base URL", launchurltip), _autoLaunchURL);
            }

            // messages
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Error Messages", staticinfostyle);

            string lastmobilewarning = _mobileWarningMessage;
            if (!_disableMobileCheck)
            {
                string mobilewarntip = "Change the warning message displayed when WebGL build runs on a Mobile device, empty uses Unity default.";
                _mobileWarningMessage = EditorGUILayout.TextField(new GUIContent("Mobile Warning", mobilewarntip), _mobileWarningMessage);
            }

            string lastgenericmessage = _genericErrorMessage;
            string genericerrortip = "Change the generic error message displayed when WebGL build throws an error, empty uses Unity default.";
            _genericErrorMessage = EditorGUILayout.TextField(new GUIContent("Generic Error", genericerrortip), _genericErrorMessage);

            string lastunhandledmessage = _unhandledExceptionMessage;
            string unhandledexceptip = "Change the error message displayed when WebGL build throws an exception but exceptions are not enabled in Player Settings, empty uses Unity default.";
            _unhandledExceptionMessage = EditorGUILayout.TextField(new GUIContent("Unhandled Exception", unhandledexceptip), _unhandledExceptionMessage);

            string lastoutofmemorymessage = _outOfMemoryMessage;
            string outofmemorytip = "Change the error message displayed when WebGL build runs out of Memory, empty uses Unity default.";
            _outOfMemoryMessage = EditorGUILayout.TextField(new GUIContent("Out Of Memory", outofmemorytip), _outOfMemoryMessage);

            string lastnotenoughmemorymessage = _notEnoughMemoryMessage;
            string notenoughmemorytip = "Change the error message displayed when WebGL build can't allocate it's inital memory block, empty uses Unity default.";
            _notEnoughMemoryMessage = EditorGUILayout.TextField(new GUIContent("Not Enough Memory", notenoughmemorytip), _notEnoughMemoryMessage);

            WebGLCompressionFormat lastcompression = _compressionFormat;
#if UNITY_5_5_OR_NEWER
            // webgl build settings
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Unity WebGL Settings", staticinfostyle);

            string compressiontip = "Compression format used for WebGL builds.";
            _compressionFormat = (WebGLCompressionFormat)EditorGUILayout.EnumPopup(new GUIContent("Compression Format", compressiontip), _compressionFormat);

            //string lastemscriptargs = _emscriptenArgs;
            //string emscriptargstip = "Arguments passed to emscripten for WebGL builds.";
            //_emscriptenArgs = EditorGUILayout.TextField(new GUIContent("Emscripten Args", emscriptargstip), _emscriptenArgs);

#endif

            EditorGUI.indentLevel = originalIndent;

            bool changed = lastautorun != _autoRunAfterBuild || lastdeskdpr != _desktopScale || lastmobdpr != _mobileScale ||
                            lastcreatecopy != _createCopy || lastdisablemobile != _disableMobileCheck ||
                            lastautolanuch != _autoLaunchBrowser || lastlaunchurl != _autoLaunchURL ||
                           lastmobilewarning != _mobileWarningMessage || lastgenericmessage != _genericErrorMessage ||
                           lastunhandledmessage != _unhandledExceptionMessage || lastoutofmemorymessage != _outOfMemoryMessage ||
                           lastnotenoughmemorymessage != _notEnoughMemoryMessage || lastcompression != _compressionFormat;
            _unappliedChanges |= changed;
            OnWizardUpdate();
            return changed;
        }

        void OnWizardCreate()
        {

        }

        // When the user presses the "Apply" button OnWizardOtherButton is called.
        void OnWizardOtherButton()
        {
            // fix settings
            WebGLRetinaTools.AutoRunFix = _autoRunAfterBuild;
            WebGLRetinaTools.DesktopScale = _desktopScale;
            WebGLRetinaTools.MobileScale = _mobileScale;
            WebGLRetinaTools.DisableMobileCheck = _disableMobileCheck;
            WebGLRetinaTools.ShouldCreateCopy = _createCopy;
            WebGLRetinaTools.CopyAppendString = _copyAppendString;
            WebGLRetinaTools.AutoLaunchBrowser = _autoLaunchBrowser;
            WebGLRetinaTools.AutoLaunchBrowserURL = _autoLaunchURL;

            // messages
            WebGLRetinaTools.MobileWarningMessage = _mobileWarningMessage;
            WebGLRetinaTools.GenericErrorMessage = _genericErrorMessage;
            WebGLRetinaTools.UnhandledExceptionMessage = _unhandledExceptionMessage;
            WebGLRetinaTools.OutOfMemoryMessage = _outOfMemoryMessage;
            WebGLRetinaTools.NotEnoughMemoryMessage = _notEnoughMemoryMessage;

            // webgl build settings
#if UNITY_5_5_OR_NEWER
            UnityEditor.PlayerSettings.WebGL.compressionFormat = _compressionFormat;
            UnityEditor.PlayerSettings.WebGL.emscriptenArgs = _emscriptenArgs;
#endif
            _unappliedChanges = false;
        }
    }


    /// <summary>
    /// Main WegbGL tools class
    /// </summary>

    public static class WebGLRetinaTools
    {
        const string VERSION_STR = "2.3";

        static WebGLRetinaTools()
        {
#if BROTLISTREAM_AVALIABLE
		    String currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
			String dllPath = Path.Combine(Path.Combine(Environment.CurrentDirectory, "Assets"), "Plugins");
		    if(!currentPath.Contains(dllPath))
		    {
		        Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
		    }
#endif
        }

        // editor prefs keys
        const string AutoRunFix_Key = "Hbx.WebGL.AutoRunFix";
        const string DesktopScale_Key = "Hbx.WebGL.DesktopScale";
        const string MobileScale_Key = "Hbx.WebGL.MobileScale";
        const string Disable_MoblieCheck_Key = "Hbx.WebGL.DisableMobileCheck";
        const string CreateCopy_Key = "Hbx.WebGL.CreateCopy";
        const string CopyAppendString_Key = "Hbx.WebGL.CopyAppendString";
        const string LastFixFolder_Key = "Hbx.WebGL.LastFixFolder";
        const string AutoLaunchBrowser_Key = "Hbx.WebGL.AutoLaunchBrowser";
        const string AutoLaunchBrowserURL_Key = "Hbx.WebGL.AutoLaunchBrowserURL";
        const string MobileWarningMessage_Key = "Hbx.WebGL.MobileWarningMessage";
        const string GenericErrorMessage_Key = "Hbx.WebGL.GenericErrorMessage";
        const string UnhandledExceptionMessage_Key = "Hbx.WebGL.UnhandledExceptionMessage";
        const string OutOfMemoryMessage_Key = "Hbx.WebGL.OutOfMemoryMessage";
        const string NotEnoughMemoryMessage_Key = "Hbx.WebGL.NotEnoughMemoryMessage";

        // original error messages
        const string OriginalMobileWarningMessage = @"Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway.";
        const string OriginalGenericErrorMessage = @"An error occurred running the Unity content on this page. See your browser JavaScript console for more info. The error was:";
        const string OriginalUnhandledExceptionMessage = @"An exception has occurred, but exception handling has been disabled in this build. If you are the developer of this content, enable exceptions in your project WebGL player settings to be able to catch the exception or see the stack trace.";
        const string OriginalOutOfMemoryMessage = @"Out of memory. If you are the developer of this content, try allocating more memory to your WebGL build in the WebGL player settings.";
        const string OriginalNotEnoughMemoryMessage = @"The browser could not allocate enough memory for the WebGL content. If you are the developer of this content, try allocating less memory to your WebGL build in the WebGL player settings.";

        // folder and extension names
        const string ProgressTitle = "Applying WebGL Fix";
        const string JsExt = ".js";
        const string JsgzExt = ".jsgz";
        const string JsbrExt = ".jsbr";
        const string UnitywebExt = ".unityweb";
#if UNITY_5_6_OR_NEWER
        const string RelFolder = "Build";
        const string DevFolder = "Build";
        static readonly string[] SourceFileTypes = { JsExt, UnitywebExt };
        static readonly string[] ExcludeFileNames = { "asm.memory", ".asm.code", ".data", "wasm.code" };
#else
        const string RelFolder = "Release";
        const string DevFolder = "Development";
        static readonly string[] SourceFileTypes = { JsExt, JsgzExt, JsbrExt };
        static readonly string[] ExcludeFileNames = { "UnityLoader" };
#endif

        // saved prefs
        // fix settings
        public static bool AutoRunFix { get { return EditorPrefs.GetBool(AutoRunFix_Key, false); } set { EditorPrefs.SetBool(AutoRunFix_Key, value); } }
        public static float DesktopScale { get { return EditorPrefs.GetFloat(DesktopScale_Key, 1.0f); } set { EditorPrefs.SetFloat(DesktopScale_Key, value); } }
        public static float MobileScale { get { return EditorPrefs.GetFloat(MobileScale_Key, 1.0f); } set { EditorPrefs.SetFloat(MobileScale_Key, value); } }

        public static bool DisableMobileCheck { get { return EditorPrefs.GetBool(Disable_MoblieCheck_Key, false); } set { EditorPrefs.SetBool(Disable_MoblieCheck_Key, value); } }
        public static bool ShouldCreateCopy { get { return EditorPrefs.GetBool(CreateCopy_Key, false); } set { EditorPrefs.SetBool(CreateCopy_Key, value); } }
        public static string CopyAppendString { get { return EditorPrefs.GetString(CopyAppendString_Key, "-fixed"); } set { EditorPrefs.SetString(CopyAppendString_Key, value); } }
        public static string LastFixFolder { get { return EditorPrefs.GetString(LastFixFolder_Key, ""); } set { EditorPrefs.SetString(LastFixFolder_Key, value); } }
        public static bool AutoLaunchBrowser { get { return EditorPrefs.GetBool(AutoLaunchBrowser_Key, false); } set { EditorPrefs.SetBool(AutoLaunchBrowser_Key, value); } }
        public static string AutoLaunchBrowserURL { get { return EditorPrefs.GetString(AutoLaunchBrowserURL_Key, ""); } set { EditorPrefs.SetString(AutoLaunchBrowserURL_Key, value); } }

        // messages
        public static string MobileWarningMessage { get { return EditorPrefs.GetString(MobileWarningMessage_Key, ""); } set { EditorPrefs.SetString(MobileWarningMessage_Key, value); } }
        public static string GenericErrorMessage { get { return EditorPrefs.GetString(GenericErrorMessage_Key, ""); } set { EditorPrefs.SetString(GenericErrorMessage_Key, value); } }
        public static string UnhandledExceptionMessage { get { return EditorPrefs.GetString(UnhandledExceptionMessage_Key, ""); } set { EditorPrefs.SetString(UnhandledExceptionMessage_Key, value); } }
        public static string OutOfMemoryMessage { get { return EditorPrefs.GetString(OutOfMemoryMessage_Key, ""); } set { EditorPrefs.SetString(OutOfMemoryMessage_Key, value); } }
        public static string NotEnoughMemoryMessage { get { return EditorPrefs.GetString(NotEnoughMemoryMessage_Key, ""); } set { EditorPrefs.SetString(NotEnoughMemoryMessage_Key, value); } }

        public static bool _quiteMode = false;

        public static List<string> _debugMessages = new List<string>();

        enum CompressionType
        {
            None,
            GZip,
            Brotli
        };

        //
        // Post build event if active, try and happen last so as not to mess with others
        [PostProcessBuild(10000)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (AutoRunFix && target == BuildTarget.WebGL)
            {
                _quiteMode = true;
                RetinaFixExistingBuild(pathToBuiltProject);
                _quiteMode = false;
            }
        }

        //
        // Show settings window
        [MenuItem("Hbx/WebGL/Settings", false, 20)]
        public static void DisplaySettings()
        {
            EditWebGLSettingsWizard.CreateWizard();
        }

        //
        // Run fix on the last build made by unity
        [MenuItem("Hbx/WebGL/Fix Last Build", false, 0)]
        public static void RetinaFixLastBuild()
        {
            if (EditorUserBuildSettings.development)
            {
                RetinaFixCodeFolder(DevFolder);
            }
            else
            {
                RetinaFixCodeFolder(RelFolder);
            }
        }

        //
        // Run fix on a selected build folder
        [MenuItem("Hbx/WebGL/Fix Existing Build", false, 1)]
        public static void RetinaFixExistingBuild()
        {
            string path = EditorUtility.OpenFolderPanel("Select a WebGL build folder", LastFixFolder, "");
            if (string.IsNullOrEmpty(path))
            {
                UnityEngine.Debug.LogWarning("WebGLRetinaTools: No build folder selected.");
                return;
            }

            RetinaFixExistingBuild(path);
        }

        //
        // Run fix on a specific build folder
        public static void RetinaFixExistingBuild(string aBuildPath)
        {
#if UNITY_5_6_OR_NEWER
            if (Directory.Exists(Path.Combine(aBuildPath, RelFolder)))
            {
                RetinaFixCodeFolder(RelFolder, aBuildPath);
            }
#else
            // look for release and/or development folders
            if (Directory.Exists(Path.Combine(aBuildPath, RelFolder)))
            {
                RetinaFixCodeFolder(RelFolder, aBuildPath);
            }
            if (Directory.Exists(Path.Combine(aBuildPath, DevFolder)))
            {
                RetinaFixCodeFolder(DevFolder, aBuildPath);
            }
#endif
        }


        //
        // Opens the jsgz and/or the js file in the current webgl build folder 
        // and inserts devicePixelRatio accordingly to add support for retina/hdpi 
        //
        public static void RetinaFixCodeFolder(string codeFolder, string buildOverridePath = "")
        {
            _debugMessages.Clear();

            if (!_quiteMode) UnityEngine.Debug.Log("WebGLRetinaTools: Fix build started.");

            // get path of the last webgl build or use override path
            string webglBuildPath = string.IsNullOrEmpty(buildOverridePath) ? EditorUserBuildSettings.GetBuildLocation(BuildTarget.WebGL) : buildOverridePath;

            LastFixFolder = webglBuildPath; // cache the folder

            // do we need to make a copy
            if (ShouldCreateCopy)
            {
                string copyname = webglBuildPath + CopyAppendString;
                // check if the copy already exists
                if (Directory.Exists(copyname))
                {
                    Directory.Delete(copyname, true);
                }
                FileUtil.CopyFileOrDirectory(webglBuildPath, copyname);
                webglBuildPath = copyname;
            }

            string codeFolderPath = Path.Combine(webglBuildPath, codeFolder);

            if (string.IsNullOrEmpty(codeFolderPath))
            {
                UnityEngine.Debug.LogError("WebGLRetinaTools: WebGL build path is empty, have you created a WebGL build yet?");
                return;
            }

            // check there is a release folder
            if (!Directory.Exists(codeFolderPath))
            {
                UnityEngine.Debug.LogError("WebGLRetinaTools: Couldn't find folder for WebGL build at path:\n" + codeFolderPath);
                return;
            }

            // find source files in release folder and fix
            string[] sourceFiles = FindSourceFilesInBuildFolder(codeFolderPath);
            foreach (string sourceFile in sourceFiles)
            {
                FixSourceFile(sourceFile);
            }

            if (!_quiteMode) UnityEngine.Debug.Log("WebGLRetinaTools: Complete fixed " + sourceFiles.Length + " source files.");

            EditorUtility.ClearProgressBar();

            // Print report
            if (!_quiteMode && _debugMessages.Count > 0)
            {
                string report = "Following fixes applied...\n";
                foreach (string msg in _debugMessages)
                {
                    report += "    " + msg + "\n";
                }
                Debug.Log(report);
            }

            if (AutoLaunchBrowser && !string.IsNullOrEmpty(AutoLaunchBrowserURL))
            {
                var folder = new DirectoryInfo(webglBuildPath).Name;
                Application.OpenURL(Path.Combine(AutoLaunchBrowserURL, folder));
            }
        }

        //
        // Fix a source file based on it's extension type
        //
        static void FixSourceFile(string aSourceFile)
        {
            if (!_quiteMode) UnityEngine.Debug.Log("WebGLRetinaTools: Fixing " + aSourceFile);
            CompressionType ct = GetCompressionType(aSourceFile);
            if (ct == CompressionType.None)
            {
                FixJSFile(aSourceFile);
            }
            else
            {
                FixCompressedJSFile(aSourceFile, ct);
            }
        }

        //
        // Fix a standard .js file
        //
        static void FixJSFile(string jsPath)
        {
            string fileName = Path.GetFileName(jsPath);

            EditorUtility.DisplayProgressBar(ProgressTitle, "Opening " + fileName + "...", 0.0f);

            if (!_quiteMode) UnityEngine.Debug.Log("WebGLRetinaTools: Fixing raw JS file " + jsPath);

            // load the uncompressed js code (this might trip over on large projects)
            string sourcestr = File.ReadAllText(jsPath);
            bool ismin = IsMinified(ref sourcestr);
            StringBuilder source = new StringBuilder(sourcestr);
            sourcestr = "";

            EditorUtility.DisplayProgressBar(ProgressTitle, "Fixing js source in " + fileName + "...", 0.5f);

            if (ismin)
            {
                FixJSFileContentsMinified(ref source);
            }
            else
            {
                FixJSFileContents(fileName.Contains(".wasm."), ref source);
            }

            EditorUtility.DisplayProgressBar(ProgressTitle, "Saving js " + fileName + "...", 1.0f);

            // save the file
            File.WriteAllText(jsPath, source.ToString());
        }

        //
        // Fix a compressed jsgz file, decompresses and recompress accordingly
        //
        static void FixCompressedJSFile(string jsgzPath, CompressionType compressType)
        {
            string fileName = Path.GetFileName(jsgzPath);

            EditorUtility.DisplayProgressBar(ProgressTitle, "Uncompressing file " + fileName + "...", 0.0f);

            if (!_quiteMode) UnityEngine.Debug.Log("WebGLRetinaTools: Fixing Compressed file " + jsgzPath);

            const int size = 134217728; //128MB should be more than enough :/
            byte[] sourcebytes = new byte[size];
            int readcount = 0;

            byte[] headerbytes = null;

            // open jsgz file
            using (FileStream inputFileStream = new FileStream(jsgzPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // create demcompress stream from opened jsgz file
                if (compressType == CompressionType.GZip)
                {
                    // read header bytes to copy back later
                    headerbytes = new byte[10];
                    inputFileStream.Read(headerbytes, 0, 10);
                    inputFileStream.Position = 0;
                    // 
                    using (GZipStream decompressionStream = new GZipStream(inputFileStream, CompressionMode.Decompress))
                    {
                        // read decompressed buffer
                        readcount = decompressionStream.Read(sourcebytes, 0, size);
                    }
                }
                else
                {
#if BROTLISTREAM_AVALIABLE
	            	using (BrotliStream decompressionStream = new BrotliStream(inputFileStream, CompressionMode.Decompress))
					{
						// read decompressed buffer
						readcount = decompressionStream.Read(sourcebytes, 0, size);
					}
#endif
                }
                if (readcount <= 0)
                {
                    UnityEngine.Debug.LogError("WebGLRetinaTools: Failed to read from compressed file " + jsgzPath + " can't continue."); return;
                }
                inputFileStream.Close();
            }

            // create a string builder to edit from the decompressed buffer
            string decompressedSourceStr = Encoding.UTF8.GetString(sourcebytes, 0, readcount);
            sourcebytes = null;
            bool ismin = IsMinified(ref decompressedSourceStr);

            StringBuilder source = new StringBuilder(decompressedSourceStr);
            decompressedSourceStr = "";

            EditorUtility.DisplayProgressBar(ProgressTitle, "Fixing compressed source " + fileName + "...", 0.5f);

            // fix the source
            if (ismin)
            {
                FixJSFileContentsMinified(ref source);
            }
            else
            {
                FixJSFileContents(fileName.Contains(".wasm."), ref source);
            }

            EditorUtility.DisplayProgressBar(ProgressTitle, "Recompressing file " + fileName + "...", 1.0f);

            sourcebytes = Encoding.UTF8.GetBytes(source.ToString());
            source = null;

            // write out a compressed file with custom header
            using (FileStream fileOutputStream = File.Create(jsgzPath))
            {
                if (compressType == CompressionType.GZip)
                {

#if UNITY_5_6_OR_NEWER
                    // write the gzip header
                    fileOutputStream.Write(headerbytes, 0, 10);
                    // write the file name and comment (the comment is important as Unityloader.js looks for it)
                    byte[] fnbytes = Encoding.UTF8.GetBytes(fileName);
                    fileOutputStream.Write(fnbytes, 0, fnbytes.Length);
                    fileOutputStream.WriteByte(0); //zero terminate
                    byte[] cmbytes = Encoding.UTF8.GetBytes("UnityWeb Compressed Content (gzip)");
                    fileOutputStream.Write(cmbytes, 0, cmbytes.Length);
                    fileOutputStream.WriteByte(0); //zero terminate
#endif

                    // compress the sourc bytes and add to the output file
                    using (MemoryStream compressedMemoryStream = new MemoryStream())
                    {
                        using (GZipStream compressStream = new GZipStream(compressedMemoryStream, CompressionMode.Compress))
                        {
                            compressStream.Write(sourcebytes, 0, sourcebytes.Length);
                            compressStream.Close(); compressedMemoryStream.Close();
                            byte[] compressedbytes = compressedMemoryStream.ToArray();
#if UNITY_5_6_OR_NEWER
                            // now copy the compressed bytes excludeing the gzipstream generated header as we're using our own
                            fileOutputStream.Write(compressedbytes, 10, compressedbytes.Length - 10);
#else
                            fileOutputStream.Write(compressedbytes, 0, compressedbytes.Length);
#endif
                        }
                    }
                }
                else
                {
#if BROTLISTREAM_AVALIABLE
	        		using (BrotliStream output = new BrotliStream(fileOutputStream, CompressionMode.Compress))
	        		{
	            		output.Write(sourcebytes, 0, sourcebytes.Length);
	        		}
#endif
                }

                fileOutputStream.Close();
            }

        }

        public static string GetCodeFolder(bool isRelease)
        {
            return isRelease ? RelFolder : DevFolder;
        }

        //
        // Search folder path for all supported SourceFileTypes
        // excluding any with names containing ExcludeFileNames
        //
        static string[] FindSourceFilesInBuildFolder(string aBuildPath)
        {
            string[] files = Directory.GetFiles(aBuildPath);
            List<string> found = new List<string>();
            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);
                if (Array.IndexOf(SourceFileTypes, ext) == -1) continue;
                string name = Path.GetFileNameWithoutExtension(file);
                bool exclude = false;
                foreach (string exname in ExcludeFileNames)
                {
                    if (name.Contains(exname)) { exclude = true; break; }
                }
                if (!exclude) found.Add(file);
            }
            return found.ToArray();
        }

        //
        // returns true if passed source is minified (bit flaky but should work)
        //
        static bool IsMinified(ref string source)
        {
            return !source.Contains("},\n");
        }

        //
        // determine the compression type of a file
        //
        static CompressionType GetCompressionType(string aSourceFile)
        {
            string ext = Path.GetExtension(aSourceFile);
            if (ext == JsExt) return CompressionType.None;
            if (ext == JsgzExt) return CompressionType.GZip;
            if (ext == JsbrExt) return CompressionType.Brotli;

            // unityweb can be compressed or uncompressed so 
            // open a stream and determine type from header,
            // only supports gzip test for now
            if (ext == UnitywebExt)
            {
                using (FileStream s = File.Open(aSourceFile, FileMode.Open))
                {
                    bool isGZip = IsGzipCompressed(s);
                    s.Seek(0, SeekOrigin.Begin); //reset for when we do next check
                    s.Close();
                    return isGZip ? CompressionType.GZip : CompressionType.None;
                }
            }
            return CompressionType.None;
        }

        //
        // check if a stream contains the gzip header bytes
        static byte[] GZipHeaderBytes = { 31, 139, 0, 0, 0, 0, 0, 0, 0, 3 };
        static bool IsGzipCompressed(Stream stream)
        {
            byte[] headerbuf = new byte[10];
            int res = stream.Read(headerbuf, 0, 10);
            stream.Position = 0;
            if (res != 10) return false;

            return headerbuf[0] == GZipHeaderBytes[0] && headerbuf[1] == GZipHeaderBytes[1];//  System.Linq.Enumerable.SequenceEqual(headerbuf, GZipHeaderBytes);
        }

        //
        // Perform the find and replace hack for a release source
        //
        static void FixJSFileContentsMinified(ref StringBuilder source)
        {
            int slength = source.Length;

            // fix fillMouseEventData
            slength = source.Length;
#if UNITY_2018_2_OR_NEWER
            source.Replace("fillMouseEventData:(function(eventStruct,e,target){HEAPF64[eventStruct>>3]=JSEvents.tick();HEAP32[eventStruct+8>>2]=e.screenX;HEAP32[eventStruct+12>>2]=e.screenY;HEAP32[eventStruct+16>>2]=e.clientX;HEAP32[eventStruct+20>>2]=e.clientY;HEAP32[eventStruct+24>>2]=e.ctrlKey;HEAP32[eventStruct+28>>2]=e.shiftKey;HEAP32[eventStruct+32>>2]=e.altKey;HEAP32[eventStruct+36>>2]=e.metaKey;HEAP16[eventStruct+40>>1]=e.button;HEAP16[eventStruct+42>>1]=e.buttons;HEAP32[eventStruct+44>>2]=e[\"movementX\"]||e[\"mozMovementX\"]||e[\"webkitMovementX\"]||e.screenX-JSEvents.previousScreenX;HEAP32[eventStruct+48>>2]=e[\"movementY\"]||e[\"mozMovementY\"]||e[\"webkitMovementY\"]||e.screenY-JSEvents.previousScreenY;if(Module[\"canvas\"]){var rect=Module[\"canvas\"].getBoundingClientRect();HEAP32[eventStruct+60>>2]=e.clientX-rect.left;HEAP32[eventStruct+64>>2]=e.clientY-rect.top}else{HEAP32[eventStruct+60>>2]=0;HEAP32[eventStruct+64>>2]=0}if(target){var rect=JSEvents.getBoundingClientRectOrZeros(target);HEAP32[eventStruct+52>>2]=e.clientX-rect.left;HEAP32[eventStruct+56>>2]=e.clientY-rect.top}else{HEAP32[eventStruct+52>>2]=0;HEAP32[eventStruct+56>>2]=0}if(e.type!==\"wheel\"&&e.type!==\"mousewheel\"){JSEvents.previousScreenX=e.screenX;JSEvents.previousScreenY=e.screenY}})",
                           "fillMouseEventData:(function(eventStruct,e,target){var dpr=window.hbxDpr;HEAPF64[eventStruct>>3]=JSEvents.tick();HEAP32[eventStruct+8>>2]=e.screenX*dpr;HEAP32[eventStruct+12>>2]=e.screenY*dpr;HEAP32[eventStruct+16>>2]=e.clientX*dpr;HEAP32[eventStruct+20>>2]=e.clientY*dpr;HEAP32[eventStruct+24>>2]=e.ctrlKey;HEAP32[eventStruct+28>>2]=e.shiftKey;HEAP32[eventStruct+32>>2]=e.altKey;HEAP32[eventStruct+36>>2]=e.metaKey;HEAP16[eventStruct+40>>1]=e.button;HEAP16[eventStruct+42>>1]=e.buttons;HEAP32[eventStruct+44>>2]=e[\"movementX\"]||e[\"mozMovementX\"]||e[\"webkitMovementX\"]||(e.screenX*dpr)-JSEvents.previousScreenX;HEAP32[eventStruct+48>>2]=e[\"movementY\"]||e[\"mozMovementY\"]||e[\"webkitMovementY\"]||(e.screenY*dpr)-JSEvents.previousScreenY;if(Module[\"canvas\"]){var rect=Module[\"canvas\"].getBoundingClientRect();HEAP32[eventStruct+60>>2]=(e.clientX-rect.left)*dpr;HEAP32[eventStruct+64>>2]=(e.clientY-rect.top)*dpr}else{HEAP32[eventStruct+60>>2]=0;HEAP32[eventStruct+64>>2]=0}if(target){var rect=JSEvents.getBoundingClientRectOrZeros(target);HEAP32[eventStruct+52>>2]=(e.clientX-rect.left)*dpr;HEAP32[eventStruct+56>>2]=(e.clientY-rect.top)*dpr;}else{HEAP32[eventStruct+52>>2]=0;HEAP32[eventStruct+56>>2]=0}if(e.type!==\"wheel\"&&e.type!==\"mousewheel\"){JSEvents.previousScreenX=e.screenX*dpr;JSEvents.previousScreenY=e.screenY*dpr}})");
#else
            source.Replace("fillMouseEventData:(function(eventStruct,e,target){HEAPF64[eventStruct>>3]=JSEvents.tick();HEAP32[eventStruct+8>>2]=e.screenX;HEAP32[eventStruct+12>>2]=e.screenY;HEAP32[eventStruct+16>>2]=e.clientX;HEAP32[eventStruct+20>>2]=e.clientY;HEAP32[eventStruct+24>>2]=e.ctrlKey;HEAP32[eventStruct+28>>2]=e.shiftKey;HEAP32[eventStruct+32>>2]=e.altKey;HEAP32[eventStruct+36>>2]=e.metaKey;HEAP16[eventStruct+40>>1]=e.button;HEAP16[eventStruct+42>>1]=e.buttons;HEAP32[eventStruct+44>>2]=e[\"movementX\"]||e[\"mozMovementX\"]||e[\"webkitMovementX\"]||e.screenX-JSEvents.previousScreenX;HEAP32[eventStruct+48>>2]=e[\"movementY\"]||e[\"mozMovementY\"]||e[\"webkitMovementY\"]||e.screenY-JSEvents.previousScreenY;if(Module[\"canvas\"]){var rect=Module[\"canvas\"].getBoundingClientRect();HEAP32[eventStruct+60>>2]=e.clientX-rect.left;HEAP32[eventStruct+64>>2]=e.clientY-rect.top}else{HEAP32[eventStruct+60>>2]=0;HEAP32[eventStruct+64>>2]=0}if(target){var rect=JSEvents.getBoundingClientRectOrZeros(target);HEAP32[eventStruct+52>>2]=e.clientX-rect.left;HEAP32[eventStruct+56>>2]=e.clientY-rect.top}else{HEAP32[eventStruct+52>>2]=0;HEAP32[eventStruct+56>>2]=0}JSEvents.previousScreenX=e.screenX;JSEvents.previousScreenY=e.screenY})",
                           "fillMouseEventData:(function(eventStruct,e,target){var dpr=window.hbxDpr;HEAPF64[eventStruct>>3]=JSEvents.tick();HEAP32[eventStruct+8>>2]=e.screenX*dpr;HEAP32[eventStruct+12>>2]=e.screenY*dpr;HEAP32[eventStruct+16>>2]=e.clientX*dpr;HEAP32[eventStruct+20>>2]=e.clientY*dpr;HEAP32[eventStruct+24>>2]=e.ctrlKey;HEAP32[eventStruct+28>>2]=e.shiftKey;HEAP32[eventStruct+32>>2]=e.altKey;HEAP32[eventStruct+36>>2]=e.metaKey;HEAP16[eventStruct+40>>1]=e.button;HEAP16[eventStruct+42>>1]=e.buttons;HEAP32[eventStruct+44>>2]=e[\"movementX\"]||e[\"mozMovementX\"]||e[\"webkitMovementX\"]||(e.screenX*dpr)-JSEvents.previousScreenX;HEAP32[eventStruct+48>>2]=e[\"movementY\"]||e[\"mozMovementY\"]||e[\"webkitMovementY\"]||(e.screenY*dpr)-JSEvents.previousScreenY;if(Module[\"canvas\"]){var rect=Module[\"canvas\"].getBoundingClientRect();HEAP32[eventStruct+60>>2]=(e.clientX-rect.left)*dpr;HEAP32[eventStruct+64>>2]=(e.clientY-rect.top)*dpr}else{HEAP32[eventStruct+60>>2]=0;HEAP32[eventStruct+64>>2]=0}if(target){var rect=JSEvents.getBoundingClientRectOrZeros(target);HEAP32[eventStruct+52>>2]=(e.clientX-rect.left)*dpr;HEAP32[eventStruct+56>>2]=(e.clientY-rect.top)*dpr;}else{HEAP32[eventStruct+52>>2]=0;HEAP32[eventStruct+56>>2]=0}JSEvents.previousScreenX=e.screenX*dpr;JSEvents.previousScreenY=e.screenY*dpr})");
#endif

            if (slength != source.Length) _debugMessages.Add("Applied fix 01");

            // fix SystemInfo screen width height 
            slength = source.Length;
#if UNITY_2018_2_OR_NEWER // There's a stray newline in a release build (asm.js)
            source.Replace("return{width:screen.width?screen.width:0,\nheight:screen.height?screen.height:0,browser:i,",
                           "return{dpr:window.hbxDpr,width:screen.width?screen.width*this.dpr:0,height:screen.height?screen.height*this.dpr:0,browser:i,");
#elif UNITY_5_6_OR_NEWER
            source.Replace("return{width:screen.width?screen.width:0,height:screen.height?screen.height:0,browser:i,",
                            "return{dpr:window.hbxDpr,width:screen.width?screen.width*this.dpr:0,height:screen.height?screen.height*this.dpr:0,browser:i,");
#else
            source.Replace("var systemInfo={get:(function(){if(systemInfo.hasOwnProperty(\"hasWebGL\"))return this;var unknown=\"-\";this.width=screen.width?screen.width:0;this.height=screen.height?screen.height:0;",
                           "var systemInfo={get:(function(){if(systemInfo.hasOwnProperty(\"hasWebGL\"))return this;var unknown=\"-\";var dpr=window.hbxDpr;this.width=screen.width?screen.width*dpr:0;this.height=screen.height?screen.height*dpr:0;");

#endif
            if (slength != source.Length) _debugMessages.Add("Applied fix 02");

            // fix _JS_SystemInfo_GetCurrentCanvasHeight
            slength = source.Length;
            source.Replace("function _JS_SystemInfo_GetCurrentCanvasHeight(){return Module[\"canvas\"].clientHeight}",
                           "function _JS_SystemInfo_GetCurrentCanvasHeight(){return Module[\"canvas\"].clientHeight*window.hbxDpr;}");

            if (slength != source.Length) _debugMessages.Add("Applied fix 03");

            // fix get _JS_SystemInfo_GetCurrentCanvasWidth
            slength = source.Length;
            source.Replace("function _JS_SystemInfo_GetCurrentCanvasWidth(){return Module[\"canvas\"].clientWidth}",
                           "function _JS_SystemInfo_GetCurrentCanvasWidth(){return Module[\"canvas\"].clientWidth*window.hbxDpr;}");

            if (slength != source.Length) _debugMessages.Add("Applied fix 04");

            // fix updateCanvasDimensions (it removes the canvas style width height which prevents the fullscreen toggle via style)
            slength = source.Length;
            source.Replace("if((document[\"fullscreenElement\"]||document[\"mozFullScreenElement\"]||document[\"msFullscreenElement\"]||document[\"webkitFullscreenElement\"]||document[\"webkitCurrentFullScreenElement\"])===canvas.parentNode&&typeof screen!=\"undefined\"){var factor=Math.min(screen.width/w,screen.height/h);w=Math.round(w*factor);h=Math.round(h*factor)}if(Browser.resizeCanvas){if(canvas.width!=w)canvas.width=w;if(canvas.height!=h)canvas.height=h;if(typeof canvas.style!=\"undefined\"){canvas.style.removeProperty(\"width\");canvas.style.removeProperty(\"height\")}}else{if(canvas.width!=wNative)canvas.width=wNative;if(canvas.height!=hNative)canvas.height=hNative;if(typeof canvas.style!=\"undefined\"){if(w!=wNative||h!=hNative){canvas.style.setProperty(\"width\",w+\"px\",\"important\");canvas.style.setProperty(\"height\",h+\"px\",\"important\")}else{canvas.style.removeProperty(\"width\");canvas.style.removeProperty(\"height\")}}",
                           "var dpr=window.hbxDpr;if((document[\"fullscreenElement\"]||document[\"mozFullScreenElement\"]||document[\"msFullscreenElement\"]||document[\"webkitFullscreenElement\"]||document[\"webkitCurrentFullScreenElement\"])===canvas.parentNode&&typeof screen!=\"undefined\"){var factor=Math.min((screen.width*dpr)/w,(screen.height*dpr)/h);w=Math.round(w*factor);h=Math.round(h*factor)}if(Browser.resizeCanvas){if(canvas.width!=w)canvas.width=w;if(canvas.height!=h)canvas.height=h;if(typeof canvas.style!=\"undefined\"){canvas.style.removeProperty(\"width\");canvas.style.removeProperty(\"height\")}}else{if(canvas.width!=wNative)canvas.width=wNative;if(canvas.height!=hNative)canvas.height=hNative;if(typeof canvas.style!=\"undefined\"){if(!canvas.style.getPropertyValue(\"width\").includes(\"%\")){canvas.style.setProperty(\"width\",(w/dpr)+\"px\",\"important\");}if(!canvas.style.getPropertyValue(\"height\").includes(\"%\")){canvas.style.setProperty(\"height\",(h/dpr)+\"px\",\"important\")}}");

            if (slength != source.Length) _debugMessages.Add("Applied fix 05");

            // fix full screen dimensions
            slength = source.Length;
            source.Replace("HEAP32[eventStruct+264>>2]=reportedElement?reportedElement.clientWidth:0;HEAP32[eventStruct+268>>2]=reportedElement?reportedElement.clientHeight:0;HEAP32[eventStruct+272>>2]=screen.width;HEAP32[eventStruct+276>>2]=screen.height;",
                            "HEAP32[eventStruct+264>>2]=reportedElement?reportedElement.clientWidth:0;HEAP32[eventStruct+268>>2]=reportedElement?reportedElement.clientHeight:0;HEAP32[eventStruct+272>>2]=screen.width*window.hbxDpr;HEAP32[eventStruct+276>>2]=screen.height*window.hbxDpr;");

            if (slength != source.Length) _debugMessages.Add("Applied fix 06");

#if UNITY_5_6_OR_NEWER
            // fix touches
            slength = source.Length;
            source.Replace("for(var i in touches){var t=touches[i];HEAP32[ptr>>2]=t.identifier;HEAP32[ptr+4>>2]=t.screenX;HEAP32[ptr+8>>2]=t.screenY;HEAP32[ptr+12>>2]=t.clientX;HEAP32[ptr+16>>2]=t.clientY;HEAP32[ptr+20>>2]=t.pageX;HEAP32[ptr+24>>2]=t.pageY;HEAP32[ptr+28>>2]=t.changed;HEAP32[ptr+32>>2]=t.onTarget;if(canvasRect){HEAP32[ptr+44>>2]=t.clientX-canvasRect.left;HEAP32[ptr+48>>2]=t.clientY-canvasRect.top}else{HEAP32[ptr+44>>2]=0;HEAP32[ptr+48>>2]=0}HEAP32[ptr+36>>2]=t.clientX-targetRect.left;HEAP32[ptr+40>>2]=t.clientY-targetRect.top;ptr+=52;if(++numTouches>=32){break}}",
                           "var dpr=window.hbxDpr; for(var i in touches){var t=touches[i];HEAP32[ptr>>2]=t.identifier;HEAP32[ptr+4>>2]=t.screenX*dpr;HEAP32[ptr+8>>2]=t.screenY*dpr;HEAP32[ptr+12>>2]=t.clientX*dpr;HEAP32[ptr+16>>2]=t.clientY*dpr;HEAP32[ptr+20>>2]=t.pageX*dpr;HEAP32[ptr+24>>2]=t.pageY*dpr;HEAP32[ptr+28>>2]=t.changed;HEAP32[ptr+32>>2]=t.onTarget;if(canvasRect){HEAP32[ptr+44>>2]=(t.clientX-canvasRect.left)*dpr;HEAP32[ptr+48>>2]=(t.clientY-canvasRect.top)*dpr}else{HEAP32[ptr+44>>2]=0;HEAP32[ptr+48>>2]=0}HEAP32[ptr+36>>2]=(t.clientX-targetRect.left)*dpr;HEAP32[ptr+40>>2]=(t.clientY-targetRect.top)*dpr;ptr+=52;if(++numTouches>=32){break}}");

            if (slength != source.Length) _debugMessages.Add("Applied fix 07");
#endif

            // conditional edits

            // this only needs to apply to UnityLoader.js

            // insert dpr calc
            slength = source.Length;
#if UNITY_2018_1_OR_NEWER
            source.Replace("compatibilityCheck:function(e,t,r){",
               "compatibilityCheck:function(e,t,r){var dprs=UnityLoader.SystemInfo.mobile?" + MobileScale + ":" + DesktopScale + ";window.devicePixelRatio=window.devicePixelRatio||1;window.hbxDpr=window.devicePixelRatio*dprs;");
#else
            source.Replace("var UnityLoader=UnityLoader||{compatibilityCheck:function(e,t,r){",
               "var UnityLoader=UnityLoader||{compatibilityCheck:function(e,t,r){var dprs=UnityLoader.SystemInfo.mobile?" + MobileScale + ":" + DesktopScale + ";window.devicePixelRatio=window.devicePixelRatio||1;window.hbxDpr=window.devicePixelRatio*dprs;");

#endif
            if (slength != source.Length) _debugMessages.Add("Applied fix 08");

            if (DisableMobileCheck)
            {
                slength = source.Length;
                source.Replace("UnityLoader.SystemInfo.mobile?e.popup(\"Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway.\",[{text:\"OK\",callback:t}]):", "");
                if (slength != source.Length) _debugMessages.Add("Applied fix 09");
            }
            ApplyErrorMessageEdits(ref source);
        }

        //
        // Perform the find and replace hack for a development source
        //
        static void FixJSFileContents(bool iswasm, ref StringBuilder source)
        {
            int slength = source.Length;

#if UNITY_2018_2_OR_NEWER
            iswasm = false;
#endif

            // fix fill mouse event
            string findFillMouseString = "", replaceFillMouseString = "";
            if (!iswasm)
            {
#if UNITY_2018_2_OR_NEWER
                findFillMouseString =
@" fillMouseEventData: (function(eventStruct, e, target) {
  HEAPF64[eventStruct >> 3] = JSEvents.tick();
  HEAP32[eventStruct + 8 >> 2] = e.screenX;
  HEAP32[eventStruct + 12 >> 2] = e.screenY;
  HEAP32[eventStruct + 16 >> 2] = e.clientX;
  HEAP32[eventStruct + 20 >> 2] = e.clientY;
  HEAP32[eventStruct + 24 >> 2] = e.ctrlKey;
  HEAP32[eventStruct + 28 >> 2] = e.shiftKey;
  HEAP32[eventStruct + 32 >> 2] = e.altKey;
  HEAP32[eventStruct + 36 >> 2] = e.metaKey;
  HEAP16[eventStruct + 40 >> 1] = e.button;
  HEAP16[eventStruct + 42 >> 1] = e.buttons;
  HEAP32[eventStruct + 44 >> 2] = e[""movementX""] || e[""mozMovementX""] || e[""webkitMovementX""] || e.screenX - JSEvents.previousScreenX;
  HEAP32[eventStruct + 48 >> 2] = e[""movementY""] || e[""mozMovementY""] || e[""webkitMovementY""] || e.screenY - JSEvents.previousScreenY;
  if (Module[""canvas""]) {
   var rect = Module[""canvas""].getBoundingClientRect();
   HEAP32[eventStruct + 60 >> 2] = e.clientX - rect.left;
   HEAP32[eventStruct + 64 >> 2] = e.clientY - rect.top;
  } else {
   HEAP32[eventStruct + 60 >> 2] = 0;
   HEAP32[eventStruct + 64 >> 2] = 0;
  }
  if (target) {
   var rect = JSEvents.getBoundingClientRectOrZeros(target);
   HEAP32[eventStruct + 52 >> 2] = e.clientX - rect.left;
   HEAP32[eventStruct + 56 >> 2] = e.clientY - rect.top;
  } else {
   HEAP32[eventStruct + 52 >> 2] = 0;
   HEAP32[eventStruct + 56 >> 2] = 0;
  }
  if (e.type !== ""wheel"" && e.type !== ""mousewheel"") {
   JSEvents.previousScreenX = e.screenX;
   JSEvents.previousScreenY = e.screenY;
  }
 }),";

                replaceFillMouseString =
    @" fillMouseEventData: (function(eventStruct, e, target) {
  var devicePixelRatio = window.hbxDpr;
  HEAPF64[eventStruct >> 3] = JSEvents.tick();
  HEAP32[eventStruct + 8 >> 2] = e.screenX*devicePixelRatio;
  HEAP32[eventStruct + 12 >> 2] = e.screenY*devicePixelRatio;
  HEAP32[eventStruct + 16 >> 2] = e.clientX*devicePixelRatio;
  HEAP32[eventStruct + 20 >> 2] = e.clientY*devicePixelRatio;
  HEAP32[eventStruct + 24 >> 2] = e.ctrlKey;
  HEAP32[eventStruct + 28 >> 2] = e.shiftKey;
  HEAP32[eventStruct + 32 >> 2] = e.altKey;
  HEAP32[eventStruct + 36 >> 2] = e.metaKey;
  HEAP16[eventStruct + 40 >> 1] = e.button;
  HEAP16[eventStruct + 42 >> 1] = e.buttons;
  HEAP32[eventStruct + 44 >> 2] = e[""movementX""] || e[""mozMovementX""] || e[""webkitMovementX""] || (e.screenX*devicePixelRatio) - JSEvents.previousScreenX;
  HEAP32[eventStruct + 48 >> 2] = e[""movementY""] || e[""mozMovementY""] || e[""webkitMovementY""] || (e.screenY*devicePixelRatio) - JSEvents.previousScreenY;
  if (Module[""canvas""]) {
   var rect = Module[""canvas""].getBoundingClientRect();
   HEAP32[eventStruct + 60 >> 2] = (e.clientX - rect.left) * devicePixelRatio;
   HEAP32[eventStruct + 64 >> 2] = (e.clientY - rect.top) * devicePixelRatio;
  } else {
   HEAP32[eventStruct + 60 >> 2] = 0;
   HEAP32[eventStruct + 64 >> 2] = 0;
  }
  if (target) {
   var rect = JSEvents.getBoundingClientRectOrZeros(target);
   HEAP32[eventStruct + 52 >> 2] = (e.clientX - rect.left) * devicePixelRatio;
   HEAP32[eventStruct + 56 >> 2] = (e.clientY - rect.top) * devicePixelRatio;
  } else {
   HEAP32[eventStruct + 52 >> 2] = 0;
   HEAP32[eventStruct + 56 >> 2] = 0;
  }
  if (e.type !== ""wheel"" && e.type !== ""mousewheel"") {
   JSEvents.previousScreenX = e.screenX*devicePixelRatio;
   JSEvents.previousScreenY = e.screenY*devicePixelRatio;
  }
 }),";

#else
                findFillMouseString =
    @" fillMouseEventData: (function(eventStruct, e, target) {
  HEAPF64[eventStruct >> 3] = JSEvents.tick();
  HEAP32[eventStruct + 8 >> 2] = e.screenX;
  HEAP32[eventStruct + 12 >> 2] = e.screenY;
  HEAP32[eventStruct + 16 >> 2] = e.clientX;
  HEAP32[eventStruct + 20 >> 2] = e.clientY;
  HEAP32[eventStruct + 24 >> 2] = e.ctrlKey;
  HEAP32[eventStruct + 28 >> 2] = e.shiftKey;
  HEAP32[eventStruct + 32 >> 2] = e.altKey;
  HEAP32[eventStruct + 36 >> 2] = e.metaKey;
  HEAP16[eventStruct + 40 >> 1] = e.button;
  HEAP16[eventStruct + 42 >> 1] = e.buttons;
  HEAP32[eventStruct + 44 >> 2] = e[""movementX""] || e[""mozMovementX""] || e[""webkitMovementX""] || e.screenX - JSEvents.previousScreenX;
  HEAP32[eventStruct + 48 >> 2] = e[""movementY""] || e[""mozMovementY""] || e[""webkitMovementY""] || e.screenY - JSEvents.previousScreenY;
  if (Module[""canvas""]) {
   var rect = Module[""canvas""].getBoundingClientRect();
   HEAP32[eventStruct + 60 >> 2] = e.clientX - rect.left;
   HEAP32[eventStruct + 64 >> 2] = e.clientY - rect.top;
  } else {
   HEAP32[eventStruct + 60 >> 2] = 0;
   HEAP32[eventStruct + 64 >> 2] = 0;
  }
  if (target) {
   var rect = JSEvents.getBoundingClientRectOrZeros(target);
   HEAP32[eventStruct + 52 >> 2] = e.clientX - rect.left;
   HEAP32[eventStruct + 56 >> 2] = e.clientY - rect.top;
  } else {
   HEAP32[eventStruct + 52 >> 2] = 0;
   HEAP32[eventStruct + 56 >> 2] = 0;
  }
  JSEvents.previousScreenX = e.screenX;
  JSEvents.previousScreenY = e.screenY;
 }),";

                replaceFillMouseString =
    @" fillMouseEventData: (function(eventStruct, e, target) {
  var devicePixelRatio = window.hbxDpr;
  HEAPF64[eventStruct >> 3] = JSEvents.tick();
  HEAP32[eventStruct + 8 >> 2] = e.screenX*devicePixelRatio;
  HEAP32[eventStruct + 12 >> 2] = e.screenY*devicePixelRatio;
  HEAP32[eventStruct + 16 >> 2] = e.clientX*devicePixelRatio;
  HEAP32[eventStruct + 20 >> 2] = e.clientY*devicePixelRatio;
  HEAP32[eventStruct + 24 >> 2] = e.ctrlKey;
  HEAP32[eventStruct + 28 >> 2] = e.shiftKey;
  HEAP32[eventStruct + 32 >> 2] = e.altKey;
  HEAP32[eventStruct + 36 >> 2] = e.metaKey;
  HEAP16[eventStruct + 40 >> 1] = e.button;
  HEAP16[eventStruct + 42 >> 1] = e.buttons;
  HEAP32[eventStruct + 44 >> 2] = e[""movementX""] || e[""mozMovementX""] || e[""webkitMovementX""] || (e.screenX*devicePixelRatio) - JSEvents.previousScreenX;
  HEAP32[eventStruct + 48 >> 2] = e[""movementY""] || e[""mozMovementY""] || e[""webkitMovementY""] || (e.screenY*devicePixelRatio) - JSEvents.previousScreenY;
  if (Module[""canvas""]) {
   var rect = Module[""canvas""].getBoundingClientRect();
   HEAP32[eventStruct + 60 >> 2] = (e.clientX - rect.left) * devicePixelRatio;
   HEAP32[eventStruct + 64 >> 2] = (e.clientY - rect.top) * devicePixelRatio;
  } else {
   HEAP32[eventStruct + 60 >> 2] = 0;
   HEAP32[eventStruct + 64 >> 2] = 0;
  }
  if (target) {
   var rect = JSEvents.getBoundingClientRectOrZeros(target);
   HEAP32[eventStruct + 52 >> 2] = (e.clientX - rect.left) * devicePixelRatio;
   HEAP32[eventStruct + 56 >> 2] = (e.clientY - rect.top) * devicePixelRatio;
  } else {
   HEAP32[eventStruct + 52 >> 2] = 0;
   HEAP32[eventStruct + 56 >> 2] = 0;
  }
  JSEvents.previousScreenX = e.screenX*devicePixelRatio;
  JSEvents.previousScreenY = e.screenY*devicePixelRatio;
 }),";

#endif
            }
            else
            {

                findFillMouseString =
    @"fillMouseEventData:function (eventStruct, e, target) {
        HEAPF64[((eventStruct)>>3)]=JSEvents.tick();
        HEAP32[(((eventStruct)+(8))>>2)]=e.screenX;
        HEAP32[(((eventStruct)+(12))>>2)]=e.screenY;
        HEAP32[(((eventStruct)+(16))>>2)]=e.clientX;
        HEAP32[(((eventStruct)+(20))>>2)]=e.clientY;
        HEAP32[(((eventStruct)+(24))>>2)]=e.ctrlKey;
        HEAP32[(((eventStruct)+(28))>>2)]=e.shiftKey;
        HEAP32[(((eventStruct)+(32))>>2)]=e.altKey;
        HEAP32[(((eventStruct)+(36))>>2)]=e.metaKey;
        HEAP16[(((eventStruct)+(40))>>1)]=e.button;
        HEAP16[(((eventStruct)+(42))>>1)]=e.buttons;
        HEAP32[(((eventStruct)+(44))>>2)]=e[""movementX""] || e[""mozMovementX""] || e[""webkitMovementX""] || (e.screenX-JSEvents.previousScreenX);
        HEAP32[(((eventStruct)+(48))>>2)]=e[""movementY""] || e[""mozMovementY""] || e[""webkitMovementY""] || (e.screenY-JSEvents.previousScreenY);
  
        if (Module['canvas']) {
          var rect = Module['canvas'].getBoundingClientRect();
          HEAP32[(((eventStruct)+(60))>>2)]=e.clientX - rect.left;
          HEAP32[(((eventStruct)+(64))>>2)]=e.clientY - rect.top;
        } else { // Canvas is not initialized, return 0.
          HEAP32[(((eventStruct)+(60))>>2)]=0;
          HEAP32[(((eventStruct)+(64))>>2)]=0;
        }
        if (target) {
          var rect = JSEvents.getBoundingClientRectOrZeros(target);
          HEAP32[(((eventStruct)+(52))>>2)]=e.clientX - rect.left;
          HEAP32[(((eventStruct)+(56))>>2)]=e.clientY - rect.top;        
        } else { // No specific target passed, return 0.
          HEAP32[(((eventStruct)+(52))>>2)]=0;
          HEAP32[(((eventStruct)+(56))>>2)]=0;
        }
        JSEvents.previousScreenX = e.screenX;
        JSEvents.previousScreenY = e.screenY;
      },";

                replaceFillMouseString =
    @"fillMouseEventData:function (eventStruct, e, target) {
		var devicePixelRatio = window.hbxDpr;
        HEAPF64[((eventStruct)>>3)]=JSEvents.tick();
        HEAP32[(((eventStruct)+(8))>>2)]=e.screenX*devicePixelRatio;
        HEAP32[(((eventStruct)+(12))>>2)]=e.screenY*devicePixelRatio;
        HEAP32[(((eventStruct)+(16))>>2)]=e.clientX*devicePixelRatio;
        HEAP32[(((eventStruct)+(20))>>2)]=e.clientY*devicePixelRatio;
        HEAP32[(((eventStruct)+(24))>>2)]=e.ctrlKey;
        HEAP32[(((eventStruct)+(28))>>2)]=e.shiftKey;
        HEAP32[(((eventStruct)+(32))>>2)]=e.altKey;
        HEAP32[(((eventStruct)+(36))>>2)]=e.metaKey;
        HEAP16[(((eventStruct)+(40))>>1)]=e.button;
        HEAP16[(((eventStruct)+(42))>>1)]=e.buttons;
        HEAP32[(((eventStruct)+(44))>>2)]=e[""movementX""] || e[""mozMovementX""] || e[""webkitMovementX""] || ((e.screenX*devicePixelRatio)-JSEvents.previousScreenX);
        HEAP32[(((eventStruct)+(48))>>2)]=e[""movementY""] || e[""mozMovementY""] || e[""webkitMovementY""] || ((e.screenY*devicePixelRatio)-JSEvents.previousScreenY);
  
        if (Module['canvas']) {
          var rect = Module['canvas'].getBoundingClientRect();
          HEAP32[(((eventStruct)+(60))>>2)]=(e.clientX - rect.left)*devicePixelRatio;
          HEAP32[(((eventStruct)+(64))>>2)]=(e.clientY - rect.top)*devicePixelRatio;
        } else { // Canvas is not initialized, return 0.
          HEAP32[(((eventStruct)+(60))>>2)]=0;
          HEAP32[(((eventStruct)+(64))>>2)]=0;
        }
        if (target) {
          var rect = JSEvents.getBoundingClientRectOrZeros(target);
          HEAP32[(((eventStruct)+(52))>>2)]=(e.clientX - rect.left)*devicePixelRatio;
          HEAP32[(((eventStruct)+(56))>>2)]=(e.clientY - rect.top)*devicePixelRatio;        
        } else { // No specific target passed, return 0.
          HEAP32[(((eventStruct)+(52))>>2)]=0;
          HEAP32[(((eventStruct)+(56))>>2)]=0;
        }
        JSEvents.previousScreenX = e.screenX*devicePixelRatio;
        JSEvents.previousScreenY = e.screenY*devicePixelRatio;
      },";

            }

            source.Replace(findFillMouseString, replaceFillMouseString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 01");

#if UNITY_5_6_OR_NEWER
			// fix SystemInfo screen width height 
			string findSystemInfoString = 
@"    return {
      width: screen.width ? screen.width : 0,
      height: screen.height ? screen.height : 0,
      browser: browser,";

			string replaceSystemInfoString = 
@"    return {
      devicePixelRatio: window.hbxDpr,
      width: screen.width ? screen.width * this.devicePixelRatio : 0,
      height: screen.height ? screen.height * this.devicePixelRatio : 0,
      browser: browser,";
#else
            // fix SystemInfo screen width height 
            string findSystemInfoString =
@"var systemInfo = {
 get: (function() {
  if (systemInfo.hasOwnProperty(""hasWebGL"")) return this;
  var unknown = ""-"";
  this.width = screen.width ? screen.width : 0;
  this.height = screen.height ? screen.height : 0;";

            string replaceSystemInfoString =
@"var systemInfo = {
 get: (function() {
  if (systemInfo.hasOwnProperty(""hasWebGL"")) return this;
  var unknown = ""-"";
  var devicePixelRatio = window.hbxDpr;
  this.width = screen.width ? screen.width*devicePixelRatio : 0;
  this.height = screen.height ? screen.height*devicePixelRatio : 0;";
#endif

            slength = source.Length;
            source.Replace(findSystemInfoString, replaceSystemInfoString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 02");


            // fix _JS_SystemInfo_GetCurrentCanvasHeight

            string findGetCurrentCanvasHeightString = !iswasm ?
@"function _JS_SystemInfo_GetCurrentCanvasHeight() {
 return Module[""canvas""].clientHeight;
}" :
@"function _JS_SystemInfo_GetCurrentCanvasHeight() 
  	{
  		return Module['canvas'].clientHeight;
  	}";

            string replaceGetCurrentCanvasHeightString =
@"function _JS_SystemInfo_GetCurrentCanvasHeight() {
 return Module[""canvas""].clientHeight*window.hbxDpr;
}";

            slength = source.Length;
            source.Replace(findGetCurrentCanvasHeightString, replaceGetCurrentCanvasHeightString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 03");


            // fix get _JS_SystemInfo_GetCurrentCanvasWidth

            string findGetCurrentCanvasWidthString = !iswasm ?
@"function _JS_SystemInfo_GetCurrentCanvasWidth() {
 return Module[""canvas""].clientWidth;
}" :
@"function _JS_SystemInfo_GetCurrentCanvasWidth() 
  	{
  		return Module['canvas'].clientWidth;
  	}";

            string replaceGetCurrentCanvasWidthString =
@"function _JS_SystemInfo_GetCurrentCanvasWidth() {
 return Module[""canvas""].clientWidth*window.hbxDpr;
}";

            slength = source.Length;
            source.Replace(findGetCurrentCanvasWidthString, replaceGetCurrentCanvasWidthString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 04");


            // fix updateCanvasDimensions

            string findUpdateCanvasString = !iswasm ?
@"if ((document[""fullscreenElement""] || document[""mozFullScreenElement""] || document[""msFullscreenElement""] || document[""webkitFullscreenElement""] || document[""webkitCurrentFullScreenElement""]) === canvas.parentNode && typeof screen != ""undefined"") {
   var factor = Math.min(screen.width / w, screen.height / h);
   w = Math.round(w * factor);
   h = Math.round(h * factor);
  }
  if (Browser.resizeCanvas) {
   if (canvas.width != w) canvas.width = w;
   if (canvas.height != h) canvas.height = h;
   if (typeof canvas.style != ""undefined"") {
    canvas.style.removeProperty(""width"");
    canvas.style.removeProperty(""height"");
   }
  } else {
   if (canvas.width != wNative) canvas.width = wNative;
   if (canvas.height != hNative) canvas.height = hNative;
   if (typeof canvas.style != ""undefined"") {
    if (w != wNative || h != hNative) {
     canvas.style.setProperty(""width"", w + ""px"", ""important"");
     canvas.style.setProperty(""height"", h + ""px"", ""important"");
    } else {
     canvas.style.removeProperty(""width"");
     canvas.style.removeProperty(""height"");
    }
   }
  }" :
@"if (((document['fullscreenElement'] || document['mozFullScreenElement'] ||
             document['msFullscreenElement'] || document['webkitFullscreenElement'] ||
             document['webkitCurrentFullScreenElement']) === canvas.parentNode) && (typeof screen != 'undefined')) {
           var factor = Math.min(screen.width / w, screen.height / h);
           w = Math.round(w * factor);
           h = Math.round(h * factor);
        }
        if (Browser.resizeCanvas) {
          if (canvas.width  != w) canvas.width  = w;
          if (canvas.height != h) canvas.height = h;
          if (typeof canvas.style != 'undefined') {
            canvas.style.removeProperty( ""width"");
            canvas.style.removeProperty(""height"");
          }
        } else {
          if (canvas.width  != wNative) canvas.width  = wNative;
          if (canvas.height != hNative) canvas.height = hNative;
          if (typeof canvas.style != 'undefined') {
            if (w != wNative || h != hNative) {
              canvas.style.setProperty( ""width"", w + ""px"", ""important"");
              canvas.style.setProperty(""height"", h + ""px"", ""important"");
            } else {
              canvas.style.removeProperty( ""width"");
              canvas.style.removeProperty(""height"");
            }
          }
        }";

            string replaceUpdateCanvasString =
@"var dpr = window.hbxDpr;
  if ((document[""fullscreenElement""] || document[""mozFullScreenElement""] || document[""msFullscreenElement""] || document[""webkitFullscreenElement""] || document[""webkitCurrentFullScreenElement""]) === canvas.parentNode && typeof screen != ""undefined"") {
   var factor = Math.min((screen.width*dpr) / w, (screen.height*dpr) / h);
   w = Math.round(w * factor);
   h = Math.round(h * factor);
  }
  if (Browser.resizeCanvas) {
   if (canvas.width != w) canvas.width = w;
   if (canvas.height != h) canvas.height = h;
   if (typeof canvas.style != ""undefined"") {
    canvas.style.removeProperty(""width"");
    canvas.style.removeProperty(""height"");
   }
  } else {
   if (canvas.width != wNative) canvas.width = wNative;
   if (canvas.height != hNative) canvas.height = hNative;
   if (typeof canvas.style != ""undefined"") {
     if(!canvas.style.getPropertyValue(""width"").includes(""%""))canvas.style.setProperty(""width"", (w/dpr) + ""px"", ""important"");
     if(!canvas.style.getPropertyValue(""height"").includes(""%""))canvas.style.setProperty(""height"", (h/dpr) + ""px"", ""important"");
   }
  }";

            slength = source.Length;
            source.Replace(findUpdateCanvasString, replaceUpdateCanvasString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 05");


            string findFullscreenEventString = !iswasm ?
@"  HEAP32[eventStruct + 264 >> 2] = reportedElement ? reportedElement.clientWidth : 0;
  HEAP32[eventStruct + 268 >> 2] = reportedElement ? reportedElement.clientHeight : 0;
  HEAP32[eventStruct + 272 >> 2] = screen.width;
  HEAP32[eventStruct + 276 >> 2] = screen.height;" :
@"        HEAP32[(((eventStruct)+(264))>>2)]=reportedElement ? reportedElement.clientWidth : 0;
        HEAP32[(((eventStruct)+(268))>>2)]=reportedElement ? reportedElement.clientHeight : 0;
        HEAP32[(((eventStruct)+(272))>>2)]=screen.width;
        HEAP32[(((eventStruct)+(276))>>2)]=screen.height;";

            string replaceFullscreenEventString =
@"  HEAP32[eventStruct + 264 >> 2] = reportedElement ? reportedElement.clientWidth : 0;
  HEAP32[eventStruct + 268 >> 2] = reportedElement ? reportedElement.clientHeight : 0;
  HEAP32[eventStruct + 272 >> 2] = screen.width * window.hbxDpr;
  HEAP32[eventStruct + 276 >> 2] = screen.height * window.hbxDpr;";

            slength = source.Length;
            source.Replace(findFullscreenEventString, replaceFullscreenEventString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 06");


#if UNITY_5_6_OR_NEWER
		//
		// touches
			string findTouchesString = 
@"for (var i in touches) {
    var t = touches[i];
    HEAP32[ptr >> 2] = t.identifier;
    HEAP32[ptr + 4 >> 2] = t.screenX;
    HEAP32[ptr + 8 >> 2] = t.screenY;
    HEAP32[ptr + 12 >> 2] = t.clientX;
    HEAP32[ptr + 16 >> 2] = t.clientY;
    HEAP32[ptr + 20 >> 2] = t.pageX;
    HEAP32[ptr + 24 >> 2] = t.pageY;
    HEAP32[ptr + 28 >> 2] = t.changed;
    HEAP32[ptr + 32 >> 2] = t.onTarget;
    if (canvasRect) {
     HEAP32[ptr + 44 >> 2] = t.clientX - canvasRect.left;
     HEAP32[ptr + 48 >> 2] = t.clientY - canvasRect.top;
    } else {
     HEAP32[ptr + 44 >> 2] = 0;
     HEAP32[ptr + 48 >> 2] = 0;
    }
    HEAP32[ptr + 36 >> 2] = t.clientX - targetRect.left;
    HEAP32[ptr + 40 >> 2] = t.clientY - targetRect.top;
    ptr += 52;
    if (++numTouches >= 32) {
     break;
    }
   }";

			string replaceTouchesString = 
@" var devicePixelRatio = window.hbxDpr;
   for (var i in touches) {
    var t = touches[i];
    HEAP32[ptr >> 2] = t.identifier;
    HEAP32[ptr + 4 >> 2] = t.screenX*devicePixelRatio;
    HEAP32[ptr + 8 >> 2] = t.screenY*devicePixelRatio;
    HEAP32[ptr + 12 >> 2] = t.clientX*devicePixelRatio;
    HEAP32[ptr + 16 >> 2] = t.clientY*devicePixelRatio;
    HEAP32[ptr + 20 >> 2] = t.pageX*devicePixelRatio;
    HEAP32[ptr + 24 >> 2] = t.pageY*devicePixelRatio;
    HEAP32[ptr + 28 >> 2] = t.changed;
    HEAP32[ptr + 32 >> 2] = t.onTarget;
    if (canvasRect) {
     HEAP32[ptr + 44 >> 2] = (t.clientX - canvasRect.left) * devicePixelRatio;
     HEAP32[ptr + 48 >> 2] = (t.clientY - canvasRect.top) * devicePixelRatio;
    } else {
     HEAP32[ptr + 44 >> 2] = 0;
     HEAP32[ptr + 48 >> 2] = 0;
    }
    HEAP32[ptr + 36 >> 2] = (t.clientX - targetRect.left) * devicePixelRatio;
    HEAP32[ptr + 40 >> 2] = (t.clientY - targetRect.top) * devicePixelRatio;
    ptr += 52;
    if (++numTouches >= 32) {
     break;
    }
   }";
            slength = source.Length;
			source.Replace(findTouchesString, replaceTouchesString);
            if (slength != source.Length) _debugMessages.Add("Applied fix 07");

#endif


            // conditional edits
#if UNITY_2018_1_OR_NEWER
            // this only needs to apply to UnityLoader.js
            string findDPRInsertPoint =
@"compatibilityCheck: function (gameInstance, onsuccess, onerror) {";

			string replaceDPRInsertPoint = 
@"compatibilityCheck: function (gameInstance, onsuccess, onerror) {
    var dprs = UnityLoader.SystemInfo.mobile ? " + MobileScale + " : " + DesktopScale + @";
    window.devicePixelRatio = window.devicePixelRatio || 1;
    window.hbxDpr = window.devicePixelRatio * dprs;";

            slength = source.Length;
			source.Replace(findDPRInsertPoint, replaceDPRInsertPoint);
            if (slength != source.Length) _debugMessages.Add("Applied fix 08");
#else
            // this only needs to apply to UnityLoader.js
            string findDPRInsertPoint =
@"var UnityLoader = UnityLoader || {
  compatibilityCheck: function (gameInstance, onsuccess, onerror) {";

            string replaceDPRInsertPoint =
@"var UnityLoader = UnityLoader || {
  compatibilityCheck: function (gameInstance, onsuccess, onerror) {
    var dprs = UnityLoader.SystemInfo.mobile ? " + MobileScale + " : " + DesktopScale + @";
    window.devicePixelRatio = window.devicePixelRatio || 1;
    window.hbxDpr = window.devicePixelRatio * dprs;";

            slength = source.Length;
            source.Replace(findDPRInsertPoint, replaceDPRInsertPoint);
            if (slength != source.Length) _debugMessages.Add("Applied fix 08");
#endif


            string findMobileCheckString = 
@"else if (UnityLoader.SystemInfo.mobile) {
      gameInstance.popup(""Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway."",
        [{text: ""OK"", callback: onsuccess}]);
    } ";

			if(DisableMobileCheck)
			{
                slength = source.Length;
				source.Replace(findMobileCheckString, "");
                if (slength != source.Length) _debugMessages.Add("Applied fix 09");
			}

			ApplyErrorMessageEdits(ref source);
		}

		//
		// Edit the UnityLoader.js error messages
		static void ApplyErrorMessageEdits(ref StringBuilder source)
		{
			// mobile check, only edit this if it's not being removed
			if(!DisableMobileCheck)
			{
				if(!string.IsNullOrEmpty(MobileWarningMessage))
				{
					source.Replace(OriginalMobileWarningMessage, MobileWarningMessage);
				}
			}

			if(!string.IsNullOrEmpty(GenericErrorMessage))
			{
				source.Replace(OriginalGenericErrorMessage, GenericErrorMessage);
			}

			if(!string.IsNullOrEmpty(UnhandledExceptionMessage))
			{
				source.Replace(OriginalUnhandledExceptionMessage, UnhandledExceptionMessage);
			}

			if(!string.IsNullOrEmpty(OutOfMemoryMessage))
			{
				source.Replace(OriginalOutOfMemoryMessage, OutOfMemoryMessage);
			}

			if(!string.IsNullOrEmpty(NotEnoughMemoryMessage))
			{
				source.Replace(OriginalNotEnoughMemoryMessage, NotEnoughMemoryMessage);
			}
		}
	}

}
