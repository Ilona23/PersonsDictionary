using Application.Abstractions.Messaging;

namespace Application.Mapping
{
    public abstract class BaseMapper<TEntity, TModel> : IMapper<TEntity, TModel>
    {
        public abstract TModel MapToModel(TEntity entity);
        public abstract TEntity MapToEntity(TModel model);
        public abstract List<TModel> MapToModelList(IEnumerable<TEntity> entities);
        public abstract List<TEntity> MapToEntityList(IEnumerable<TModel> models);
    }
}
