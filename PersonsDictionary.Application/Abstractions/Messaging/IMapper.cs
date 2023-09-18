namespace Application.Abstractions.Messaging
{
    public interface IMapper<TEntity, TModel>
    {
        TModel MapToModel(TEntity entity);
        TEntity MapToEntity(TModel model);
        List<TModel> MapToModelList(IEnumerable<TEntity> entities);
        List<TEntity> MapToEntityList(IEnumerable<TModel> models);
    }
}
