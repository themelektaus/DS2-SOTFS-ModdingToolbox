namespace DS2_SOTFS_ModdingToolbox;

public static class Data
{
    const string DATA_FOLDER = @"Data";

    const string DECRYPTED_REGULATION_FILE = @"Decrypted Regulation File\enc_regulation.bnd.dcx";
    const string ORIGINAL_CHECKSUM_FILE = @"Original Checksum File\DarksoulsII.json";
    const string PROJECT_TEMPLATE_FOLDER = @"Project Template";
    const string UNPACKED_PARAM_FILES_FOLDER = @"Unpacked Param Files";

    public static string decryptedRegulationFile => GetDataPath(DECRYPTED_REGULATION_FILE);
    public static string originalChecksumFile => GetDataPath(ORIGINAL_CHECKSUM_FILE);
    public static string projectTemplateFolder => GetDataPath(PROJECT_TEMPLATE_FOLDER);
    public static string unpackedParamFilesFolder => GetDataPath(UNPACKED_PARAM_FILES_FOLDER);

    static string GetDataPath(params string[] path)
    {
        var path1 = DATA_FOLDER;
        var path2 = Path(path);
        return GetFullPath(Path(path1, path2));
    }
}