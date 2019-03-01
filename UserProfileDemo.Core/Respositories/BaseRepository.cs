﻿using System;
using System.Collections.Generic;
using System.IO;
using Couchbase.Lite;
using Couchbase.Lite.Mapping;
using Newtonsoft.Json;
using UserProfileDemo.Core.Models;

namespace UserProfileDemo.Core.Respositories
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        string DatabaseName { get; set; }

        DatabaseConfiguration _databaseConfig;
        DatabaseConfiguration DatabaseConfig
        {
            get
            {
                if (_databaseConfig == null)
                {
                    _databaseConfig = new DatabaseConfiguration
                    {
                        Directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                                        AppInstance.User.Username)
                    };
                }

                return _databaseConfig;
            }
        }

        Database _database;
        Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database(DatabaseName, DatabaseConfig);
                }

                return _database;
            }
        }

        protected BaseRepository(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public virtual T Get(string id)
        {
            var document = Database.GetDocument(id);
            return document?.ToObject<T>();
        }

        public virtual void Set(T obj)
        {
            var document = obj?.ToMutableDocument(obj.Id);

            //document.SetBlob("test", new Blob("image/jpeg", new byte[256]));

            /*
            try
            {
                var blob = new Blob("image/jpeg", new byte[256]);

                var json = JsonConvert.SerializeObject(blob);
            }
            catch (Exception ex)
            {
                string test = "";
            }*/


            Database.Save(document);
        }
    }
}