namespace BizStream.Kentico.Xperience.Administration.StatusCodePages.Abstractions
{

    /// <summary> Describes service that can ensure the import of data and settings required by StatusCodePages.  </summary>
    public interface IStatusCodePagesImportProvider
    {

        /// <summary> Imports all dependant objects are imported. </summary>
        void ImportObjectsData( );

        /// <summary> Determines whether dependant objects have been imported. </summary>
        bool IsObjectsDataImported( );

    }

}
