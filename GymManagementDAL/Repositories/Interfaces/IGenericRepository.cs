using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new() //Means only concrete class not abstract class 
    {                                                                                 //to be sure that it is Entity
        IEnumerable<TEntity> GetAll(Func<TEntity,bool>Condition = null); //Null Means that this Delegate is Optional
        TEntity? GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
