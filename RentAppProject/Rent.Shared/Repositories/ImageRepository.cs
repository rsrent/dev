using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using RentAppProject;

namespace RentApp
{
    public class ImageRepository
    {
        public ImageRepository()
        {
        }

        public async Task<Dictionary<int, object>> GetUserImages(ICollection<User> users, Func<string,Task<object>> loadImage) {
            
            ConcurrentDictionary<int, object> images = new ConcurrentDictionary<int, object>();

            List<Task> TaskList = new List<Task>();
            foreach (var user in users)
            {
                var LastTask = new Task(async () => {
                    var thisUser = user;
                    if (user.ImageLocation != null)
                        images.TryAdd(thisUser.ID, await loadImage.Invoke(thisUser.ImageLocation));
                });
                LastTask.Start();
                TaskList.Add(LastTask);
            }
            await Task.WhenAll(TaskList.ToArray());

            return images.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
