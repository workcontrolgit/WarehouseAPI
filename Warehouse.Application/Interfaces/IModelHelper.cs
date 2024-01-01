namespace Warehouse.Application.Interfaces
{
    public interface IModelHelper
    {
        string GetModelFields<Entity>();

        string ValidateModelFields<Entity>(string fields);
    }
}