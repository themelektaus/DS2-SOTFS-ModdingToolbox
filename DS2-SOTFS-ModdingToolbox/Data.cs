﻿namespace DS2_SOTFS_ModdingToolbox;

public static class Data
{
    public static string coreLibrary
        => GetDataPath(Lang.System.CORE_LIBRARY);

    public static string decryptedRegulationFile
        => GetDataPath(Lang.System.DECRYPTED_REGULATION_FILE);

    public static string languageFolder
        => GetDataPath(Lang.System.LANGUAGE_FOLDER);

    public static string originalChecksumFile
        => GetDataPath(Lang.System.ORIGINAL_CHECKSUM_FILE);

    public static string projectTemplateFolder
        => GetDataPath(Lang.System.PROJECT_TEMPLATE_FOLDER);

    public static string unpackedParamFilesFolder
        => GetDataPath(Lang.System.UNPACKED_PARAM_FILES_FOLDER);

    static string GetDataPath(params string[] path)
    {
        var path1 = Lang.System.DATA_FOLDER;
        var path2 = Path(path);
        return GetFullPath(Path(path1, path2));
    }
}