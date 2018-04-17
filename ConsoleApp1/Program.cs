using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Case.Study.Listener
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

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Request request = JsonConvert.DeserializeObject<Request>(message);
                    InsertUserAndUpdateRequestStatus(request);
                };

                channel.BasicConsume(queue: "UserQueue",
                                     autoAck: true,
                                     consumer: consumer);


                Console.WriteLine("İşlem tamamlandı. Kullanıcı kaydı açıldı");
                Console.ReadLine();
            }
        }

        private static void InsertUserAndUpdateRequestStatus(Request request)
        {
            using (SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = "data source=.;initial catalog=LOGUE;user id=sa;password=as;multipleactiveresultsets=True;application name=EntityFramework&quot;";
                conn.Open();

                User user = JsonConvert.DeserializeObject<User>(request.Request_Body);

                SqlCommand insertCommand = new SqlCommand("INSERT INTO dbo.Users (User_Email,User_Password,User_UserName,User_FullName) VALUES (@0,@1,@2,@3)", conn);
                insertCommand.Parameters.Add(new SqlParameter("0", user.User_Email ?? ""));
                insertCommand.Parameters.Add(new SqlParameter("1", user.User_Password ?? ""));
                insertCommand.Parameters.Add(new SqlParameter("2", user.User_UserName ?? ""));
                insertCommand.Parameters.Add(new SqlParameter("3", user.User_FullName ?? ""));
                insertCommand.ExecuteNonQuery();

                SqlCommand sqlCommand = new SqlCommand("UPDATE Requests set Request_StatusID = 3 where" + "  Request_ID = " + request.Request_ID, conn);
                sqlCommand.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
