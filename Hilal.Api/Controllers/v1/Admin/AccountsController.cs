//using system;
//using system.linq;
//using system.threading.tasks;
//using castle.core.configuration;
//using hilal.dataviewmodel.request;
//using hilal.dataviewmodel.request.admin.v1;
//using hilal.service.interface.v1.admin;
//using hilal.service.interface.v1;
//using microsoft.aspnetcore.http;
//using microsoft.aspnetcore.mvc;
//using newtonsoft.json;

//namespace hilal.api.controllers.v1.admin
//{
//    [apiversion("1")]
//    [route("api/v{version:apiversion}/[controller]")]
//    [apicontroller]
//    public class accountscontroller : controllerbase
//    {
//        private readonly iconfiguration configuration;
//        private iaccountservice _accountservice;

//        public accountscontroller(iconfiguration configuration)
//        {
//            this.configuration = configuration;
//        }

//        #region roles
//        [httpget]
//        [route("getroles")]
//        public iactionresult getroles()
//        {
//            try
//            {
//                var res = _accountservice.getrole();
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httpget]
//        [route("rolebyid")]
//        public iactionresult rolebyid(raw id)
//        {
//            try
//            {
//                var res = _accountservice.getrolebyid(id.id.value);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httppost]
//        [route("saveandupdaterole")]
//        public iactionresult saveandupdaterole(rolesviewmodel rolesobject)
//        {
//            try
//            {
//                if (!modelstate.isvalid)
//                {
//                    return statuscode(statuscodes.status400badrequest, modelstate.values.selectmany(v => v.errors.select(z => z.errormessage)));
//                }

//                var userid = routedata.values["userid"].tostring();
//                bool res = _accountservice.saveandupdaterole(rolesobject);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httpget]
//        [route("deleterole")]
//        public iactionresult deleterole(raw id)
//        {
//            try
//            {
//                bool res = _accountservice.deleterole(id.id.value);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }
//        #endregion

//        #region assignrolesrights
//        [httpget]
//        [route("getassignrolesrights")]
//        public iactionresult getassignrolesrights()
//        {
//            try
//            {
//                var res = _accountservice.getassignrolesrights();
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httpget]
//        [route("adminassignrolesrightsbyid")]
//        public iactionresult adminassignrolesrightsbyid(raw id)
//        {
//            try
//            {
//                var res = _accountservice.getassignrolesrightsbyid(id.id.value);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httpget]
//        [route("getassignrolesrightsbyrolid")]
//        public iactionresult getassignrolesrightsbyrolid(raw id)
//        {
//            try
//            {
//                var res = _accountservice.getassignrolesrightsbyrolid(id.id.value);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httppost]
//        [route("saveandupdateassignrolesrights")]
//        public iactionresult saveandupdateassignrolesrights(assignrolesrightsviewmodel rightsobject)
//        {
//            try
//            {
//                if (!modelstate.isvalid)
//                {
//                    return statuscode(statuscodes.status400badrequest, modelstate.values.selectmany(v => v.errors.select(z => z.errormessage)));
//                }


//                var userid = routedata.values["userid"].tostring();
//                bool res = _accountservice.saveandupdateassignrolesrights(rightsobject);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }
//        #endregion

//        #region users
//        [httppost]
//        [route("saveandupdateusers")]
//        public async task<iactionresult> saveandupdateusers()
//        {
//            try
//            {
//                var obj = request.form["obj"].tostring();
//                var userobject = jsonconvert.deserializeobject<usersviewmodel>(obj);

//                dataviewmodel.common.fileurlresponce filee = new dataviewmodel.common.fileurlresponce();

//                if (!modelstate.isvalid)
//                {
//                    return statuscode(statuscodes.status400badrequest, modelstate.values.selectmany(v => v.errors.select(z => z.errormessage)));
//                }

//                //if (request.form.files.count > 0)
//                //{
//                //    for (int i = 0; i < request.form.files.count; i++)
//                //    {
//                //        var file = request.form.files[i];
//                //        //  instalment.chacqpic = await grenalservices.savefiel(file, enviroment);
//                //        filee = await savefiles.sendmyfiletos3(file, configuration.getvalue<string>("amazon:bucket"), "profilepicture", configuration.getvalue<string>("amazon:accesskey"), configuration.getvalue<string>("amazon:accesssecret"), configuration.getvalue<string>("amazon:baseurl"));

//                //    }
//                //}


//                var userid = routedata.values["userid"].tostring();
//                bool res = _accountservice.saveandupdateusers(userobject);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httpget]
//        [route("getusers")]
//        public async task<iactionresult> getusers()
//        {
//            try
//            {

//                var res = _accountservice.getusers();
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        [httpget]
//        [route("getcustomeresusers")]
//        public async task<iactionresult> getcustomeresusers()
//        {
//            try
//            {

//                var res = _accountservice.getcustomeresusers();
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }
//        [httpget]
//        [route("getusersforsuperadmin")]
//        public async task<iactionresult> getusersforsuperadmin()
//        {
//            try
//            {

//                var res = _accountservice.getusersforsuperadmin();
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }
//        [httpget]
//        [route("getusersbycustomerid")]
//        public async task<iactionresult> getusersbycustomerid(raw raw)
//        {
//            try
//            {

//                var res = _accountservice.getusersbycustomerid(raw);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }
//        [httpget]
//        [route("getuserbyid")]
//        public async task<iactionresult> getuserbyid(raw raw)
//        {
//            try
//            {

//                var res = _accountservice.getusersbycustomerid(raw);
//                return ok(res);
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        //[httppost]
//        //[route("login")]
//        //public iactionresult login(loginrequestviewmodel login)
//        //{
//        //    try
//        //    {
//        //        if (!modelstate.isvalid)
//        //        {
//        //            return statuscode(statuscodes.status400badrequest, modelstate.values.selectmany(v => v.errors.select(z => z.errormessage)));
//        //        }

//        //        var res = _accountservice.login(login);

//        //        if (res.item3)
//        //        {
//        //            return statuscode(statuscodes.status403forbidden, new response<loginviewmodel>() { iserror = true, messages = error.accountblocked, data = new loginviewmodel() });
//        //        }

//        //        if (res.item2)
//        //        {
//        //            return statuscode(statuscodes.status200ok, new response<loginviewmodel>() { iserror = false, messages = "", data = res.item1 });
//        //        }

//        //        return statuscode(statuscodes.status403forbidden, new response<loginviewmodel>() { iserror = true, messages = error.loginfailed, data = new loginviewmodel() });
//        //    }
//        //    catch (exception ex)
//        //    {
//        //        throw ex;
//        //    }
//        //}

//        //[httppost]
//        //[route("changepassword")]
//        //public async task<iactionresult> changepassword(changepasswordviewmodel changepassword)
//        //{
//        //    try
//        //    {
//        //        if (!modelstate.isvalid)
//        //        {
//        //            return statuscode(statuscodes.status400badrequest, modelstate.values.selectmany(v => v.errors.select(z => z.errormessage)));
//        //        }
//        //        var userid = routedata.values["userid"].tostring();
//        //        //var res = await _accountservice.changepassword(changepassword, userid);

//        //        return ok();
//        //    }
//        //    catch (exception ex)
//        //    {
//        //        throw ex;
//        //    }
//        //}
//        #endregion
//    }
//}
