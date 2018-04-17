Logue Case Study
=======================

> Project Structure


![capture](https://user-images.githubusercontent.com/17234785/38830550-61b81a94-41c5-11e8-8c39-e370aa304263.PNG)


## SQL server 2014 for database storage 

###### LOGUE.bak file could be used to restore a  copy of the database 


# Case.Study.Api

## AddUser 

###### To add a new requset with user details 
```C#
        [Authorize(Roles = "Administrator")]
        [HttpPost, HttpGet]
        [Route("api/user/insert")]
        public IHttpActionResult AddUser(User UserModel_)
        {
            ResultModel result_ = new ResultModel();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string requestBody_ = js.Serialize(UserModel_);

                if (string.IsNullOrEmpty(UserModel_.User_Email) || string.IsNullOrEmpty(UserModel_.User_Password))
                    throw new Exception("email or pass can not be empty");

                string random_ = System.Guid.NewGuid().ToString();
                Request req = new Api.Request()
                {
                    Request_Body = requestBody_,
                    Request_Guid = random_,
                    Request_StatusID = (int)RequestStatus.QUE,
                    Request_Response = ""
                };
                this._RequestRepository.InsertRequest(req);
                this._RequestRepository.Save();
                result_.SuccessMessage = "ok";
                result_.IsSuccess = true;
                result_.Return = random_;

                DoAddRequestToQueue(req);
            }
            catch (Exception ex_)
            {
                Logger.Log.Add(ex_);
                result_.ErrorMessage = ex_.Message;
                result_.IsSuccess = false;
            }
            return Content<ResultModel>(HttpStatusCode.OK, result_);
        }
```

## CheckUser 

###### Check the status of a request:
``` c#
  [Authorize(Roles = "Administrator")]
        [HttpPost, HttpGet]
        [Route("api/log/{guid}")]
        public IHttpActionResult CheckUser(string guid)
        {
            ResultModel<string> result_ = new ResultModel<string>();
            try
            {
                var request_ = this._RequestRepository.GetRequestByGuid(guid);
                if (request_ != null)
                    result_.Data = Enum.GetName(typeof(RequestStatus), request_.Request_StatusID);
                else
                    throw new Exception("contact with system administratoır");
                result_.IsSuccess = true;
            }
            catch (Exception ex_)
            {
                Logger.Log.Add(ex_);
                result_.ErrorMessage = ex_.Message;
                result_.IsSuccess = false;
            }
            return Content<ResultModel<string>>(HttpStatusCode.OK, result_);
        }
 ```

## DoAddRequestToQueue 

###### To declare a quueue and to add a message to the queue with request details
``` c#
 public void DoAddRequestToQueue(Request request)
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

                string message = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "UserQueue",
                                     basicProperties: null,
                                     body: body);
            }
        }
```

# Case.Study.Listener

##### Classes 
``` c#
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
```
