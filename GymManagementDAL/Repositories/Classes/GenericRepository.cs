﻿using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext; // private Field To Cannot Access to the DbContext outside


        //Dependency Injection
        //Ask CLR to inject object from GymDbcontext
        //DbContext Object is Injected not Created Manually
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Add(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);
        public void Delete(TEntity entity) =>_dbContext.Set<TEntity>().Remove(entity);
           
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> Condition = null)
        {
            if (Condition is  null) 
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();
            else
                return _dbContext.Set<TEntity>().AsNoTracking().Where(Condition).ToList();
        }

        public TEntity? GetById(int id) => _dbContext.Set<TEntity>().Find(id);  //Find() Because it searches in Local Memory First
       
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
           
    }
}
