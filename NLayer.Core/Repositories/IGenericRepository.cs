using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();

        //productRepository.where(x => x.id > 5).OrderBy.ToListAsync() ile aynı işlevi yapmak için kullanıldı
        //Delege ile T değişkeni alıp karşılığında bool ifade döndürür
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddRangeAsync (IEnumerable<T> entities);
        Task AddAsync(T entity);
        void Update(T entity);

        void Remove(T entity);

        void RemoveRange (IEnumerable<T> entities);
    }
}
