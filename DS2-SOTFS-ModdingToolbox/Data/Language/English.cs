static class English
{
    static string NATIVE_NAME = "English";
    static string ENGLISH_NAME = "English";

    static class ManagedTask
    {
        static class Name
        {
            static string BUILD = "Build ({0})";
            static string CALC_FOLDER_CHECKSUM = "Calc Folder Checksum ({0})";
            static string CLEAR_BUILD = "Clear Build";
        }

        static class Info
        {
            static string CLEARING = "Clearing...";
            static string DONE = "Done";
        }
    }

    static class Title
    {
        static string TASK = "Task";
        static string CALC_FOLDER_CHECKSUM = "Calc Folder Checksum";
        static string FILE_SYSTEM = "File System";
    }

    static class Text
    {
        static string PROJECT_ALREADY_EXISTS = "Project already exists";

        static string CALC_FOLDER_CHECKSUM_RESULT = "Processed {0} files";
        static string CALC_FOLDER_CHECKSUM_SUCCESS = "All {0} files are original game files";
        static string CALC_FOLDER_CHECKSUM_FAILED = "Not all {0} files are original game files";

        static string DS2_SHOULD_BE_INSIDE_GAME_FOLDER = "{0} should be inside a folder called \"{1}\".";
    }

    static class UI
    {
        static string NEW_PROJECT = "New Project";
        static string LOAD_PROJECT = "Load Project";
        static string CONFIG = "Config";

        static string CHANGE_LANGUAGE = "Change Language";

        static string PROJECT_NAME = "Project Name";

        static string CONTINUE = "Continue";
        static string CREATE = "Create";
        static string LOAD = "Load";
        static string SAVE = "Save";
        static string CANCEL = "Cancel";
        static string BACKUP = "Backup";
        static string CLOSE = "Close";

        static string CUSTOM_BUILD_SETTINGS = "Custom Build Settings";

        static string USE_DS_MAP_STUDIO_PARAMS = "Use DS Map Studio Params";

        static string REBUILD = "Rebuild";
        static string BUILD = "Build";
        static string RELOAD = "Reload";
        static string CLEAR = "Clear";
        static string RUN_GAME = "Run Game";
        static string FREEZE_SAVEGAME = "Freeze Savegame";

        static class Html
        {
            static string CALC_FOLDER_CHECKSUM_INFO = @"
                <p>
                    First, the checksum of the currently installed game is calculated and<br />
                    compared accordingly to see if it matches the original game data.
                </p>
            ";
        }
    }

    static class GameBuilder
    {
        static string MOVE_ORIGINAL_GAME = "Try move original game";
        static string MOVE_ORIGINAL_GAME_FAILED = "Move original game failed";
        static string UNPACK_GAME = "Unpack game";
        static string UNPACK_FAILED = "Unpack failed";
        static string UXM_NOT_FOUND = "UXM Selective Unpack not found";
        static string DELETE_BACKUP = "Delete Backup";
        static string RECREATE_GAME_FOLDER = "Recreate game folder";
        static string PACK_REGULATION_FILE = "Pack regulation file";
        static string ADD_UNPACKED_FILES = "Add unpacked files";
        static string ADD_UNPACKED_PARAM_FILES = "Add unpacked param files";
        static string ADD_PROJECT = "Add project";
        static string DONE = "Done";
    }

    static class FileSystemUtils
    {
        static string PATH_IS_EMPTY = "Path is empty";
        static string PATH_IS_VALID = "\"{0}\" is valid";
        static string PATH_IS_INVALID = "\"{0}\" is invalid";
        static string PATH_EXISTS = "\"{0}\" exists";
        static string PATH_NOT_EXISTS = "\"{0}\" not exists";
        static string FILENAME_IS_VALID = "\"{0}\" is valid";
        static string FILENAME_IS_INVALID = "\"{0}\" is invalid";
        static string SOURCE_NOT_FOUND = "Source not found: \"{0}\"";
        static string DESTINATION_ALREADY_EXISTS = "Destination already exists: \"{0}\"";
    }
}