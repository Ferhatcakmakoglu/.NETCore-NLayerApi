using System.Linq.Expressions;

namespace NLayer.Core.Services
{
    //IGenericRepositor ile aynı Featurler, Service katmanında bu veriler donıs tipi vs işlenebilir
    public interface IService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        //productRepository.where(x => x.id > 5).OrderBy.ToListAsync() ile aynı işlevi yapmak için kullanıldı
        //Delege ile T değişkeni alıp karşılığında bool ifade döndürür
        IQueryable<T> Where(Expression<Func<T, bool>> exception);
        Task<bool> AnyAsync(Expression<Func<T, bool>> exception);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
