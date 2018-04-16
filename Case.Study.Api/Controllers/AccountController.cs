using Case.Study.Api.Models;
using Case.Study.Api.Utilities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Case.Study.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private IUserRepository _UserRepository;
        private IRequestRepository _RequestRepository;

        public AccountController()
        {
            this._UserRepository = new UserRepository(new MyTestDBEntities());
            this._RequestRepository = new RequestRepository(new MyTestDBEntities());
        }

        public AccountController(IUserRepository UserRepository_)
        {
            this._UserRepository = UserRepository_;
        }

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

                DoAddToQueue(req);
            }
            catch (Exception ex_)
            {
                Logger.Log.Add(ex_);
                result_.ErrorMessage = ex_.Message;
                result_.IsSuccess = false;
            }
            return Content<ResultModel>(HttpStatusCode.OK, result_);
        }

        private bool IsSessionUser(string Email_)
        {
            var identity_ = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var username = identity_.Claims.Where(c => c.Type == ClaimTypes.Name)
                   .Select(c => c.Value).SingleOrDefault();
            var email = identity_.Claims.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).SingleOrDefault();
            if (Email_ == email)
                return true;
            else
                return false;
        }

        public void DoAddToQueue(Request request)
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
    }
}
