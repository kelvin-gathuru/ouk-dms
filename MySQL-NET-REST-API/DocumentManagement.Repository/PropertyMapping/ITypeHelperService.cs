namespace DocumentManagement.Repository
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
