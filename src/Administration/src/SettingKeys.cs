namespace BizStream.Kentico.Xperience.Administration.StatusCodePages;

/// <summary> Static constants class for <see cref="CMS.DataEngine.SettingsKeyInfo.KeyName"/>s. </summary>
public static class SettingKeys
{
    /// <summary> Static constants class for <see cref="CMS.DataEngine.SettingsKeyInfo.KeyName"/>s within the "General" category. </summary>
    public static class General
    {
        /// <summary> KeyName of a global settings key that is used to determine whether auto-import has been run. </summary>
        public const string AreObjectsImported = Prefix + "AreObjectsImported";

        /// <summary> KeyName prefix for keys within the "General" category. </summary>
        public const string Prefix = SettingKeys.Prefix + "General_";
    }

    /// <summary> KeyName prefix for keys within representing StatusCodePages settings. </summary>
    public const string Prefix = "BizStream_StatusCodePages_";
}
