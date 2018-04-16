using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Listener
{
    class Program
    {
        public partial class User
        {
            public long User_ID { get; set; }
            public string User_Email { get; set; }
            public Nullable<bool> User_EmailConfirmed { get; set; }
            public string User_Password { get; set; }
            public string User_PasswordHash { get; set; }
            public string User_UserName { get; set; }
            public Nullable<int> User_RoleID { get; set; }
            public Nullable<bool> User_Active { get; set; }
            public Nullable<System.DateTime> User_CreateDate { get; set; }
            public string User_FullName { get; set; }

            public virtual User User1 { get; set; }
            public virtual User User2 { get; set; }
        }

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "UserQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    User user = JsonConvert.DeserializeObject<User>(message);
                    Console.WriteLine($" Username: {user.User_UserName} Fullname:{user.User_FullName} ");
                };
                channel.BasicConsume(queue: "UserQueue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" İşe Alındınız. Teşekkürler :)");
                Console.ReadLine();
            }
        }
    }
}
