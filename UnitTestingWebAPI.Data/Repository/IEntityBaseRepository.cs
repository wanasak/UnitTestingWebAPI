﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestingWebAPI.Data.Repository
{
    public interface IEntityBaseRepository<T> where T :class
    {
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predict);

        T GetSingle(int id);
        T Get(Expression<Func<T, bool>> predict);

        IQueryable<T> GetAll();
        IQueryable<T> GetMany(Expression<Func<T, bool>> predict);
    }
}