namespace TopAutoSpot.BL
{
    interface IService<TService>
    {
        List<TService> GetAll();
        TService GetById(string TId);
        void Add(TService TEntity);
        void Delete(TService TEntity);
        void DeleteById(string TId);
        void Update(string TId, TService UpdatedTEntity);
        void Update(TService ToUpdateTEntity, TService UpdatedTEntity);
    }
}
