using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiBelle.Models;
using Plugin.CloudFirestore;
using System.Linq;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Extensions;

//Queries https://www.youtube.com/watch?v=sKFLI5FOOHs
//Pagination https://firebase.google.com/docs/firestore/query-data/query-cursors
//FirebaseCloudStore https://www.youtube.com/watch?v=v_hR4K4auoQ

namespace DepiBelle.Services.Data
{
    public class CFDataCollectionService<T> : IDataCollectionService<T> where T  : EntityBase, new()
    {

        private DataServiceConfig Config { get; set; }
        protected string Uri { get { return Config.Uri; } }
        protected string Key { get { return Config.Key; } }

        public virtual bool Initialize(DataServiceConfig config)
        {
            if (Config == null)
                Config = config;

            return true;
        }

        public virtual async Task<List<T>> GetAll(string token = null, 
                                                  int limit=20,
                                                  object offset = null,
                                                  QueryLike queryLike = null,
                                                  List<QueryOrderBy> querysOrderBy = null,
                                                  List<QueryWhere> querysWhere=null)
        {
            try
            {
                //OFFSET CON START AFTER, LIKE CON START AT END AT

                var collectionQuery = CrossCloudFirestore.Current
                             .Instance.GetCollection(Key)
                             .LimitTo(limit);
              
                if (querysOrderBy != null)
                {
                 
                    foreach(var queryOrderBy in querysOrderBy)
                        collectionQuery = collectionQuery.OrderBy(queryOrderBy.OrderByField, queryOrderBy.IsDescending);

                }

                if (queryLike != null)
                {
                    AddOrderBy(collectionQuery, queryLike.LikeField, querysOrderBy);

                    collectionQuery = collectionQuery.StartAt(new List<object>() { queryLike.LikeValue });
                }

                if(querysWhere != null)
                {
                    foreach (var queryWhere in querysWhere)
                    {
                        switch (queryWhere.Type)
                        {
                            case QueryWhereEnum.Equals:
                                {
                                    collectionQuery = collectionQuery.WhereEqualsTo(queryWhere.WhereField,queryWhere.ValueField);

                                    break;
                                }

                            case QueryWhereEnum.GreaterThan:
                                {
                                    collectionQuery = collectionQuery.WhereGreaterThan(queryWhere.WhereField, queryWhere.ValueField);

                                    break;
                                }

                            case QueryWhereEnum.GreaterThanOrEquals:
                                {
                                    collectionQuery = collectionQuery.WhereGreaterThanOrEqualsTo(queryWhere.WhereField, queryWhere.ValueField);

                                    break;
                                }
                            case QueryWhereEnum.LessThan:
                                {
                                    collectionQuery = collectionQuery.WhereLessThan(queryWhere.WhereField, queryWhere.ValueField);

                                    break;
                                }

                            case QueryWhereEnum.LessThanOrEquals:
                                {
                                    collectionQuery = collectionQuery.WhereLessThanOrEqualsTo(queryWhere.WhereField, queryWhere.ValueField);

                                    break;
                                }

                        }

                    }

                }

                if (offset!=null)
                {
                    collectionQuery = collectionQuery.StartAfter(offset as IDocumentSnapshot);
                }

                /*
                collectionQuery = CrossCloudFirestore.Current
                             .Instance.GetCollection($"ordersInProcess/yHFyQIeV7UlkQjzEWcMw/saracatanga")
                             .LimitTo(limit)
                             //.OrderBy("price",false)
                             .WhereEqualsTo("keyvalues.petfriendly", true)
                             .WhereEqualsTo("keyvalues.gayfriendly", true);
                             //.WhereGreaterThan("price", 50)
                             //.WhereLessThan("price", 120);
                */                          
                                          

                var items = await collectionQuery.GetDocumentsAsync();

                //var prueba = (await (items.Documents.First().Data["prueba"] as IDocumentReference).GetDocumentAsync()).ToObject<T>();

                return items.ToObjects<T>().ToList();
            }
            catch(Exception ex)
            {
                throw ex; 
            }

        }

        private void AddOrderBy(IQuery collectionQuery, string orderByField, List<QueryOrderBy> querysOrderBy)
        {
            if (querysOrderBy == null || !querysOrderBy.Any(q => q.OrderByField.Equals(orderByField)))
                collectionQuery = collectionQuery.OrderBy(orderByField, false);
        }


        public virtual async Task<T> Get(string id, string token = null)
        {
            try
            {
                var item = await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .GetDocument(id)
                                        .GetDocumentAsync();

                return item.ToObject<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> AddOrReplace(T item, bool autoKey = true, string token = null)
        {
            try
            {
                //TODO
                //GET ID OF CREATED ITEM? NO SE PUEDE!! CONFIRMADO

                if (string.IsNullOrEmpty(item.Id))
                {

                    await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection(Key)
                                        .AddDocumentAsync(item);
                }
                else
                {
                    await CrossCloudFirestore.Current
                                       .Instance.GetCollection(Key)
                                       .GetDocument(item.Id)
                                       .SetDataAsync(item);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> Remove(string id, string token = null)
        {
            try
            {
                await CrossCloudFirestore.Current
                                        .Instance.GetCollection(Key)
                                        .GetDocument(id)
                                        .DeleteDocumentAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual async Task<bool> RemoveAll(string token = null)
        {
            try
            {
                //TODO
                //COMO BORRO TODOS?

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual Task<bool> Subscribe(Action<ServiceSubscriberEventParam<T>> action, string token = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> UnSubscribe()
        {
            throw new NotImplementedException();
        }
    }
}
