using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace Rent.ContextPoint
{
    public abstract class Context<Db>  where Db : class
    {
        protected readonly RentContext RentContext;
    
        protected abstract string Permission();
        protected abstract DbSet<Db> GetDb();

        //protected abstract Dto ToDto(Db db);

        protected virtual IQueryable<Db> SpecialGetRequirement(int requester, IQueryable<Db> query) => query;
        
        protected virtual IQueryable<Db> SpecialUpdateRequirement(int requester, IQueryable<Db> query) => query;
    
        protected Context(RentContext rentContext)
        {
            RentContext = rentContext;
        }
    
        private void CheckPermission(int requester, CRUDD crudd)
        {
            if (Unauthorized(requester, Permission(), crudd))
                throw new UnauthorizedAccessException();
        }
    
        private static void CheckIfFound(object obj)
        {
            if (obj == null)
                throw new NotFoundException();
        }
    
        private static IQueryable<Db> AddInclude(IQueryable<Db> query, string include)
        {
            return query.Include(include);
        }
        
        /*
        public virtual T GetOne<T>(int requester, Expression<Func<Db, T>> select, Expression<Func<Db, bool>> condition = null, params string[] includes)
        {
            var res = Get(requester, select, condition, includes).FirstOrDefault();
            CheckIfFound(res);
            return res;
        }
        */
        
        public virtual Db GetOne(int requester, Expression<Func<Db, bool>> condition = null, params string[] includes)
        {
            var res = Get(requester, condition, includes).FirstOrDefault();
            CheckIfFound(res);
            return res;
        }
        
        /*
        public virtual ICollection<T> Get<T>(int requester, Expression<Func<Db, T>> select, Expression<Func<Db, bool>> condition = null, params string[] includes)
        {
            CheckPermission(requester, CRUDD.Read);
            var query = SpecialGetRequirement(requester, GetDb());
            
            foreach (var include in includes)
                query = AddInclude(query, include);

            if (condition == null)
                return query.Select(select).ToList();
            
            return query.Where(condition).Select(select).ToList();
        }
        */
        
        public virtual IQueryable<Db> Get(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        {
            CheckPermission(requester, CRUDD.Read);
            var query = SpecialGetRequirement(requester, GetDb());
            
            foreach (var include in includes)
                query = AddInclude(query, include);
            
            if (condition == null)
                return query;
            
            return query.Where(condition);
        }
        
        public virtual async Task<Db> Create(int requester, Db obj)
        {
            CheckPermission(requester, CRUDD.Create);
            GetDb().Add(obj);
            await RentContext.SaveChangesAsync();
            return obj;
        }
        
        public virtual async Task<IEnumerable<Db>> Create(int requester, ICollection<Db> objs)
        {
            CheckPermission(requester, CRUDD.Create);
            GetDb().AddRange(objs);
            await RentContext.SaveChangesAsync();
            return objs.ToList();
        }
        
        public virtual async Task Update(int requester, Expression<Func<Db, bool>> condition, Action<Db> update, params string[] includes)
        {
            CheckPermission(requester, CRUDD.Update);
            
            var query = SpecialUpdateRequirement(requester, GetDb()).Where(condition);

            foreach (var include in includes)
                query = AddInclude(query, include);

            var toUpdate = query.Select(d => PrepareForUpdate(d, update)).ToList();
            
            if(toUpdate == null || !toUpdate.Any())
                throw new NothingUpdatedException();
            
            GetDb().UpdateRange(toUpdate);
            await RentContext.SaveChangesAsync();
        }

        private static Db PrepareForUpdate(Db db, Action<Db> prepare)
        {
            prepare(db);
            return db;
        }
    
        public virtual async Task Delete(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        {
            CheckPermission(requester, CRUDD.Delete);
            var query = GetDb().Where(condition);
            foreach (var include in includes)
                query = AddInclude(query, include);
            GetDb().RemoveRange(query);
            await RentContext.SaveChangesAsync();
        }
        
        public bool Unauthorized(int userId, string permission, CRUDD permissionType)
        {
            if (userId == 0)
                return false;
            
            var userPermissions = RentContext.UserPermissions.Include(up => up.Permission)
                .FirstOrDefault(up => up.UserID == userId && up.Permission.Name.Equals(permission));
            
            if (userPermissions != null)
            {
                switch (permissionType)
                {
                    case CRUDD.Create:
                        return !userPermissions.Create;
                    case CRUDD.Read:
                        return !userPermissions.Read;
                    case CRUDD.Update:
                        return !userPermissions.Update;
                    case CRUDD.Delete:
                        return !userPermissions.Delete;
                }
            }
            return true;
        }
        
        public ICollection<UserPermissions> GetUserPermissions(int userId)
        {
            return RentContext.UserPermissions.Where(up => up.UserID == userId).Include(up => up.Permission).ToList();
        }
    }
}
