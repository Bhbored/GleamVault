using GleamVaultApi.DAL.Services;
using GleamVaultApi.DB;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace GleamVaultApi.DAL.Contracts
{
    public abstract class BaseServiceClass<T> : IRepository<T> where T : class, IAudit
    {
        public BaseServiceClass() { }

        public Task<T> Delete(Guid id)
        {
            return Task.Run(async () =>
            {
                using (var db = DatabaseService.GetDB())
                {
                    var original = await Get(id);
                    if (original != null)
                    {
                        var set = db.Set<T>();
                        set.Attach(original);
                        set.Remove(original);

                    }


                    db.SaveChanges();
                    return original;
                }
            });
        }

        public Task<T> Get(Guid id)
        {
            return Task.Run(() =>
            {
                using (var db = DatabaseService.GetDB())
                    return db.Set<T>().Find(id);
            });
        }

        public Task<List<T>> GetAll()
        {
            return Task.Run(() =>
            {
                using (var db = DatabaseService.GetDB())
                {
                    return db.Set<T>().ToList();
                }
            });
        }

        public string GetEntitySetName()
        {
            return typeof(T).Name;
        }

        public Task<T> Update(T entity, IIdentity user)
        {
            return Task.Run(async () =>
            {
                using (var db = DatabaseService.GetDB())
                {
                  
                    var original = db.Set<T>().Find(entity.Id);

                    if (entity.Id == Guid.Empty || original == null)
                    {
                        // CREATE
                        entity.Id = Guid.NewGuid();
                        entity.CreatedBy = user?.Name ?? "System";
                        entity.CreatedDate = DateTime.Now;
                        entity.ModifiedBy = user?.Name ?? "System";
                        entity.ModifiedDate = DateTime.Now;
                        db.Set<T>().Add(entity);
                    }
                    else
                    {
                       
                        original = Map(original, entity);
                        original.ModifiedBy = user?.Name ?? "System";
                        original.ModifiedDate = DateTime.Now;
                     
                        db.Entry(original).State = EntityState.Modified;
                    }

                    await db.SaveChangesAsync();
                    return original ?? entity;
                }
            });
        }

        protected abstract T Map(T original, T sourceEntity);
    }
}