﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Listener
{
    class Program
    {
        public partial class Request
        {
            public long Request_ID { get; set; }
            public string Request_Guid { get; set; }
            public Nullable<long> Request_StatusID { get; set; }
            public string Request_Response { get; set; }
            public string Request_Body { get; set; }
        }

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
                User user = new User();
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Request request = JsonConvert.DeserializeObject<Request>(message);
                    user = JsonConvert.DeserializeObject<User>(request.Request_Body);

                };

                InsertUser(user);

                channel.BasicConsume(queue: "UserQueue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" İşe Alındınız. Teşekkürler :)");
                Console.ReadLine();
            }
        }


        public static void InsertUser(User user)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "metadata=res://*/MyTestDBContext.csdl|res://*/MyTestDBContext.ssdl|res://*/MyTestDBContext.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=.;initial catalog=BURAK;user id=sa;password=as;multipleactiveresultsets=True;application name=EntityFramework&quot;";
                conn.Open();


                SqlCommand insertCommand = new SqlCommand("INSERT INTO User (User_Email, User_FullName, User_Password) VALUES (@0, @1, @2)", conn);

                insertCommand.Parameters.Add(new SqlParameter("0", user.User_Email));
                insertCommand.Parameters.Add(new SqlParameter("1", user.User_FullName));
                insertCommand.Parameters.Add(new SqlParameter("1", user.User_Password));
                insertCommand.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
