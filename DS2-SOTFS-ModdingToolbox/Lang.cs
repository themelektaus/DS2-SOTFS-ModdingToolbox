namespace DS2_SOTFS_ModdingToolbox;

public static class Lang
{
    public static string ENGLISH = "English";

    public static string NATIVE_NAME = "NATIVE_NAME";
    public static string ENGLISH_NAME = "ENGLISH_NAME";

    public static class Window
    {
        public static string TITLE = "DS2 SOTFS Modding Toolbox";
    }

    public static class Internal
    {
        public static string INDEX_HTML_PATH = @"wwwroot\index.html";
    }

    public static class Format
    {
        public static string DATETIME = "yyyy-MM-dd HH:mm:ss";
        public static string BACKUP_FOLDER = "{0}-{1}";
    }

    public static class System
    {
        public static string GAME_FOLDER = "Game";
        public static string GAME_ORIGIN_FOLDER = "Game.origin";
        public static string UNPACKED_GAME_FOLDER = "Game.unpacked";

        public static string DARK_SOULS_2_PROCESS_NAME = "DarkSoulsII";
        public static string DARK_SOULS_2_EXE = $"{DARK_SOULS_2_PROCESS_NAME}.exe";
        public static string DARK_SOULS_2_SAVEGAME_FILE = "DS2SOFS0000.sl2";
        public static string USERCONFIG_PROPERTIES_FILE = "userconfig.properties";

        public static string UNPACKED_GAME_BACKUP_FOLDER = "_backup";

        public static string COMPRESSED_REGULATION_FILE = "enc_regulation.bnd.dcx";
        public static string REGULATION_FILE = "enc_regulation.bnd";
        public static string REGULATION_FOLDER = "enc_regulation-bnd";
        public static string REGULATION_FILE_YABBER_XML = "enc_regulation.bnd-yabber-dcx.xml";

        public static string PARAM_FOLDER = "Param";
        public static string PARAM_FILE_EXT = ".param";

        public static string BDT_FILE_EXT = ".bdt";

        public static string INSTALL_LOCATION_REGISTRY_KEY_NAME = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 335300";
        public static string INSTALL_LOCATION_REGISTRY_VALUE_NAME = "InstallLocation";

        public static string UXM_SELECTIVE_UNPACK_EXE = "UXM Selective Unpack.exe";

        public static string DS_MAP_STUDIO_PROCESS_NAME = "DSMapStudio";
        public static string DS_MAP_STUDIO_EXE = $"{DS_MAP_STUDIO_PROCESS_NAME}.exe";

        public static string DS_MAP_STUDIO_PROJECT_FILE = "project.json";
        public static string PROJECT_FILE = "project.extended.json";

        public static string BAK_FILE_EXT = ".bak";
        public static string PREV_FILE_EXT = ".prev";
        public static string JSON_FILE_EXT = ".json";
        public static string BND_DCX_FILE_EXT = ".bnd.dcx";

        public static string DATA_FOLDER = "Data";
        public static string DECRYPTED_REGULATION_FILE = @$"Decrypted Regulation File\{COMPRESSED_REGULATION_FILE}";
        public static string LANGUAGE_FOLDER = "Language";
        public static string LIBRARY_FOLDER = "Library";
        public static string ORIGINAL_CHECKSUM_FILE = @"Original Checksum File\DarksoulsII.json";
        public static string PROJECT_TEMPLATE_FOLDER = "Project Template";
        public static string SCRIPTS_FOLDER = "Scripts";
        public static string UNPACKED_PARAM_FILES_FOLDER = "Unpacked Param Files";

        public static string USER_DATA_FOLDER = "UserData";
        public static string BACKUP_FOLDER = "Backup";
        public static string PROJECTS_FOLDER = "Projects";
        public static string CONFIG_FILE = "Config.json";
    }

    public static class ManagedTask
    {
        public static class Name
        {
            public static string BUILD = "BUILD ({0})";
            public static string CALC_FOLDER_CHECKSUM = "CALC_FOLDER_CHECKSUM ({0})";
            public static string CLEAR_BUILD = "CLEAR_BUILD";
            public static string SCRIPT = "SCRIPT";
        }

        public static class Info
        {
            public static string CLEARING = "CLEARING...";
            public static string RUNNING = "RUNNING...";
            public static string DONE = "DONE";
        }
    }

    public static class Title
    {
        public static string TASK = "TASK";
        public static string CALC_FOLDER_CHECKSUM = "CALC_FOLDER_CHECKSUM";
        public static string FILE_SYSTEM = "FILE_SYSTEM";
    }
    
    public static class Text
    {
        public static string PROJECT_ALREADY_EXISTS = "PROJECT_ALREADY_EXISTS";

        public static string CALC_FOLDER_CHECKSUM_RESULT = "CALC_FOLDER_CHECKSUM_RESULT ({0})";
        public static string CALC_FOLDER_CHECKSUM_SUCCESS = "CALC_FOLDER_CHECKSUM_SUCCESS ({0})";
        public static string CALC_FOLDER_CHECKSUM_FAILED = "CALC_FOLDER_CHECKSUM_FAILED ({0})";

        public static string DS2_SHOULD_BE_INSIDE_GAME_FOLDER = "DS2_SHOULD_BE_INSIDE_GAME_FOLDER";
    }

    public static class UI
    {
        public static string NEW_PROJECT = "NEW_PROJECT";
        public static string LOAD_PROJECT = "LOAD_PROJECT";
        public static string CONFIG = "CONFIG";
        public static string SCRIPTING = "SCRIPTING";

        public static string CHANGE_LANGUAGE = "CHANGE_LANGUAGE";

        public static string PROJECT_NAME = "PROJECT_NAME";

        public static string START = "START";
        public static string CONTINUE = "CONTINUE";
        public static string SKIP = "SKIP";
        public static string CREATE = "CREATE";
        public static string LOAD = "LOAD";
        public static string SAVE = "SAVE";
        public static string CANCEL = "CANCEL";
        public static string BACKUP = "BACKUP";
        public static string CLOSE = "CLOSE";

        public static string UXM_SELECTIVE_UNPACK = "UXM Selective Unpack";
        public static string DS_MAP_STUDIO = "DS Map Studio";
        public static string YABBER = "Yabber";
        public static string YABBER_DCX = "Yabber DCX";

        public static string CUSTOM_BUILD_SETTINGS = "CUSTOM_BUILD_SETTINGS";

        public static string USE_DS_MAP_STUDIO_PARAMS = "USE_DS_MAP_STUDIO_PARAMS";

        public static string REBUILD = "REBUILD";
        public static string BUILD = "BUILD";
        public static string RELOAD = "RELOAD";
        public static string CLEAR = "CLEAR";
        public static string RUN_GAME = "RUN_GAME";
        public static string FREEZE_SAVEGAME = "FREEZE_SAVEGAME";

        public static class Html
        {
            public static string CALC_FOLDER_CHECKSUM_INFO = "<p>CALC_FOLDER_CHECKSUM_INFO</p>";
        }
    }

    public static class GameBuilder
    {
        public static string MOVE_ORIGINAL_GAME = "MOVE_ORIGINAL_GAME";
        public static string MOVE_ORIGINAL_GAME_FAILED = "MOVE_ORIGINAL_GAME_FAILED";
        public static string UNPACK_GAME = "UNPACK_GAME";
        public static string UNPACK_FAILED = "UNPACK_FAILED";
        public static string UXM_NOT_FOUND = "UXM_NOT_FOUND";
        public static string DELETE_BACKUP = "DELETE_BACKUP";
        public static string RECREATE_GAME_FOLDER = "RECREATE_GAME_FOLDER";
        public static string PACK_REGULATION_FILE = "PACK_REGULATION_FILE";
        public static string ADD_UNPACKED_FILES = "ADD_UNPACKED_FILES";
        public static string ADD_UNPACKED_PARAM_FILES = "ADD_UNPACKED_PARAM_FILES";
        public static string ADD_PROJECT = "ADD_PROJECT";
        public static string DONE = "DONE";
    }

    public static class FileSystemUtils
    {
        public static string PATH_IS_EMPTY = "PATH_IS_EMPTY";
        public static string PATH_IS_VALID = "PATH_IS_VALID: \"{0}\"";
        public static string PATH_IS_INVALID = "PATH_IS_INVALID: \"{0}\"";
        public static string PATH_EXISTS = "PATH_EXISTS: \"{0}\"";
        public static string PATH_NOT_EXISTS = "PATH_NOT_EXISTS: \"{0}\"";
        public static string FILENAME_IS_VALID = "FILENAME_IS_VALID: \"{0}\"";
        public static string FILENAME_IS_INVALID = "FILENAME_IS_INVALID: \"{0}\"";
        public static string SOURCE_TO_DESTINATION = "\"{0}\" ▶ \"{1}\"";
        public static string SOURCE_NOT_FOUND = "SOURCE_NOT_FOUND: \"{0}\"";
        public static string DESTINATION_ALREADY_EXISTS = "DESTINATION_ALREADY_EXISTS: \"{0}\"";
    }
}