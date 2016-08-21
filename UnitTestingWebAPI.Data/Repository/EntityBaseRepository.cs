using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Data.Infrastructure;

namespace UnitTestingWebAPI.Data.Repository
{
    public class EntityBaseRepository<T> :IEntityBaseRepository<T> where T : class
    {
        private AppEntities dbContext;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected AppEntities DbContext
        {
            get { return dbContext ?? (dbContext = DbFactory.Init()); }
        }

        protected EntityBaseRepository(IDbFactory dbFactory)
        {
            DbFactory = DbFactory;
        }

        public virtual void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry entry = DbContext.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = DbContext.Entry<T>(entity);
            entry.State = EntityState.Deleted;
        }

        public virtual void Delete(Expression<Func<T, bool>> predict)
        {
            IQueryable<T> entities = DbContext.Set<T>().Where(predict);
            foreach (T entity in entities)
                DbContext.Set<T>().Remove(entity);
        }

        public virtual T GetSingle(int id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> predict)
        {
            return dbContext.Set<T>().Where(predict).FirstOrDefault();
        }

        public virtual IQueryable<T> GetAll()
        {
            return dbContext.Set<T>();
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> predict)
        {
            return dbContext.Set<T>().Where(predict);
        }
    }
}
