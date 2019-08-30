using System;
using System.Collections.Generic;
using ModuleLibrary.Shared.Storage;

namespace ModuleLibrary.Shared.Storage
{
    public class LocalStorage<T> : ILocalStorage<T> where T : class
    {
        private Dictionary<string, (T, DateTime)> Storage;
        private readonly ILocalStorageSettings _settings;

        public LocalStorage(ILocalStorageSettings settings)
        {
            _settings = settings;

            if(SaveLoad.LoadText<Dictionary<string, (T, DateTime)>>(_settings.StorageName(), out var storage)) {
                Storage = storage;
            }

            if(Storage == null)
                Storage = new Dictionary<string, (T, DateTime)>();

            var pairsToRemove = new List<string>();
            foreach(var pair in Storage) {
                if(pair.Value.Item2 < DateTime.Now.AddDays(-_settings.CacheDurationInDays())) {
                    pairsToRemove.Add(pair.Key);
                }
            }

            foreach(var toRemove in pairsToRemove) {
                Storage.Remove(toRemove);
            }

            SaveLoad.SaveText(_settings.StorageName(), Storage);
        }


        public void Put(string key, T value) {
            if(!Storage.ContainsKey(key)) {
				Storage.Add(key, (value, DateTime.Now));
            } else {
                Storage[key] = (value, DateTime.Now);
            }
			SaveLoad.SaveText(_settings.StorageName(), Storage);
        }

        public T Get(string key) {
            if(Storage.ContainsKey(key)) { 

                try {
                    var value = Storage[key].Item1;
                    return value;
                } catch {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}
