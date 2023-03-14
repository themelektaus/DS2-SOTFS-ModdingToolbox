public static class German
{
    static string NATIVE_NAME = "Deutsch";
    static string ENGLISH_NAME = "German";

    static class ManagedTask
    {
        static class Name
        {
            static string BUILD = "Ersellen ({0})";
            static string CALC_FOLDER_CHECKSUM = "Prüfsummenberechnung ({0})";
            static string CLEAR_BUILD = "Build leeren";
        }

        static class Info
        {
            static string CLEARING = "Säubern...";
            static string RUNNING = "Läuft...";
            static string DONE = "Fertig";
        }
    }

    static class Title
    {
        static string TASK = "Aufgabe";
        static string CALC_FOLDER_CHECKSUM = "Prüfsummenberechnung";
        static string FILE_SYSTEM = "Dateisystem";
        static string PROJECT = "Projekt";
        static string BACKUP = "Sicherung";
    }

    static class Text
    {
        static string PROJECT_ALREADY_EXISTS = "Projekt existiert bereits";

        static string CALC_FOLDER_CHECKSUM_RESULT = "{0} Dateien berechnet";
        static string CALC_FOLDER_CHECKSUM_SUCCESS = "Alle {0} Dateien sind Original-Dateien";
        static string CALC_FOLDER_CHECKSUM_FAILED = "Nicht alle {0} Dateien sind Original-Dateien";

        static string DS2_SHOULD_BE_INSIDE_GAME_FOLDER = "{0} sollte sich innerhalb eines Ordners befinden der \"{1}\" heißt.";

        static string BACKUP_SUCCESS = "Sicherung erfolgreich gespeichert: \"{0}\"";
        static string BACKUP_FAILED = "Sicherung von \"{0}\" fehlgeschlagen";
    }

    static class UI
    {
        static string NEW_PROJECT = "Neues Projekt";
        static string LOAD_PROJECT = "Projekt laden";
        static string LOAD_SCRIPT = "Script laden";
        static string CONFIG = "Konfiguration";

        static string UI_SCALE = "UI Skalierung";
        static string LANGUAGE = "Sprache";
        static string CHANGE = "Ändern";
        static string CHANGE_LANGUAGE = "Sprache ändern";

        static string PROJECT_NAME = "Projektname";

        static string START = "Starten";
        static string CONTINUE = "Fortsetzen";
        static string SKIP = "Überspringen";
        static string CREATE = "Erstellen";
        static string LOAD = "Laden";
        static string SAVE = "Speichern";
        static string APPLY = "Übernehmen";
        static string CANCEL = "Abbrechen";
        static string BACKUP = "Sichern";
        static string CLOSE = "Schließen";

        static string CUSTOM_BUILD_SETTINGS = "Build Einstellungen";

        static string USE_DS_MAP_STUDIO_PARAMS = "DS Map Studio Params";

        static string REBUILD = "Neu erstellen";
        static string BUILD = "Erstellen";
        static string RELOAD = "Neu laden";
        static string CLEAR = "Leeren";
        static string RUN_GAME = "Spiel starten";
        static string FREEZE_SAVEGAME = "Savegame einfrieren";

        static class Html
        {
            static string CALC_FOLDER_CHECKSUM_INFO = @"
                <p>
                    Zuerst werden Prüfsummen vom installierten Spiel berechnet und verglichen,<br>
                    um zu überprüfen, ob diese mit den original Spieldateien übereinstimmen.
                </p>
            ";
        }
    }

    static class GameBuilder
    {
        static string MOVE_ORIGINAL_GAME = "Versuch: Original-Spiel verschieben";
        static string MOVE_ORIGINAL_GAME_FAILED = "Original-Spiel verschieben fehlgeschlagen";
        static string UNPACK_GAME = "Spiel extrahieren";
        static string UNPACK_FAILED = "Extrahieren fehlgeschlagen";
        static string UXM_NOT_FOUND = "UXM Selective Unpack nicht gefunden";
        static string DELETE_BACKUP = "Sicherung löschen";
        static string RECREATE_GAME_FOLDER = "Spieleordner neu erstellen";
        static string PACK_REGULATION_FILE = "Packe Regulation File";
        static string ADD_UNPACKED_FILES = "Extrahierte Dateien hinzufügen";
        static string ADD_UNPACKED_PARAM_FILES = "Extrahierte Param-Dateien hinzufügen";
        static string ADD_PROJECT = "Projekt hinzufügen";
        static string DONE = "Fertig";
    }

    static class FileSystemUtils
    {
        static string PATH_IS_EMPTY = "Pfad ist leer";
        static string PATH_IS_VALID = "\"{0}\" ist gültig";
        static string PATH_IS_INVALID = "\"{0}\" ist ungültig";
        static string PATH_EXISTS = "\"{0}\" existiert";
        static string PATH_NOT_EXISTS = "\"{0}\" existiert nicht";
        static string FILENAME_IS_VALID = "\"{0}\" ist gültig";
        static string FILENAME_IS_INVALID = "\"{0}\" ist ungültig";
        static string SOURCE_NOT_FOUND = "Quelle nicht gefunden: \"{0}\"";
        static string DESTINATION_ALREADY_EXISTS = "Ziel nicht gefunden: \"{0}\"";
    }
}