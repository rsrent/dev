using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models;
using Microsoft.EntityFrameworkCore;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public abstract class ContextDto<Db> where Db : IDto
    {
        public abstract ContextRules GetRules();
        
        protected readonly RentContext RentContext;
        protected readonly PropCondition Condition;

        protected abstract string Permission();
        protected abstract DbSet<Db> GetDb();

        protected virtual IQueryable<Db> SpecialGetRequirement(int requester, IQueryable<Db> query) => query;

        protected virtual IQueryable<Db> SpecialUpdateRequirement(int requester, IQueryable<Db> query) => query;

        protected ContextDto(RentContext rentContext, PropCondition condition)
        {
            RentContext = rentContext;
            Condition = condition;
        }

        private void CheckPermission(int requester, CRUDD crudd)
        {
            if (Unauthorized(requester, Permission(), crudd))
                throw new UnauthorizedAccessException("Requester does not have " + Permission() + " " + crudd.ToString() + " permission");
        }

        private static void CheckIfFound(object obj)
        {
            if (obj == null)
                throw new NotFoundException();
        }

        private dynamic Filtered(dynamic db)
        {
            return Filter.FilterUnpermissioned(db, GetRules().ThisKey(), GetRules().GetUnallowed(Condition));
        }
        

        private static IQueryable<Db> AddInclude(IQueryable<Db> query, string include)
        {
            return query.Include(include);
        }

        public dynamic BasicOne(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        => Basic(requester, condition, includes).FirstOrDefault();

        public dynamic DetailedOne(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        => Detailed(requester, condition, includes).FirstOrDefault();

        public Db DatabaseOne(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        => Database(requester, condition, includes).FirstOrDefault();

        
        public IEnumerable<dynamic> Basic(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        => Get(requester, condition, includes).Select(db => db.Basic()).ToList().Select(Filtered);

        public IEnumerable<dynamic> Detailed(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        => Get(requester, condition, includes).Select(db => db.Detailed()).ToList().Select(Filtered);
        
        public IEnumerable<dynamic> BasicOrdered(int requester, Expression<Func<Db, bool>> condition, Func<IEnumerable<Db>,IEnumerable<Db>> sort, params string[] includes)
        => sort(Get(requester, condition, includes).ToList()).Select((db) => db.Basic()).ToList().Select(Filtered);

        public IQueryable<Db> Database(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        => Get(requester, condition, includes);

        private IQueryable<Db> Get(int requester, Expression<Func<Db, bool>> condition, params string[] includes)
        {
            Condition.SetUser(requester);
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

            if (toUpdate == null || !toUpdate.Any())
                throw new NothingUpdatedException();

            GetDb().UpdateRange(toUpdate);
            await RentContext.SaveChangesAsync();
        }

        private static Db PrepareForUpdate(Db db, Action<Db> prepare)
        {
            prepare(db);
            return db;
        }

        public virtual async Task Delete(int requester, Expression<Func<Db, bool>> condition)
        {
            CheckPermission(requester, CRUDD.Delete);
            var toRemove = GetDb().Where(condition);
            GetDb().RemoveRange(toRemove);
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

    public abstract class ContextRules
    {
        public abstract string ThisKey(string key = null);
        
        public Dictionary<string, List<string>> MergeDictionaries(Dictionary<string, List<string>> d1,
            Dictionary<string, List<string>> d2)
        {
            return d1.Union(d2).ToDictionary(d => d.Key, d => d.Value);
        }
        
        public virtual Dictionary<string,List<string>> GetUnallowed(PropCondition condition, string key = null)
        {
            return new Dictionary<string, List<string>> {{ThisKey(key), new List<string>()}};;
        }
    }
}
