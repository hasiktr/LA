Logue Case Study
=======================

> Project Structure


![capture](https://user-images.githubusercontent.com/17234785/38830550-61b81a94-41c5-11e8-8c39-e370aa304263.PNG)


SQL server 2014 for database storage 

LOGUE.bak file could be used to restore a  copy of the database 


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

