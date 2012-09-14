﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SecurityInnovation.TeamMentor.WebClient;
using System.Web;
using O2.DotNetWrappers.ExtensionMethods;
using System.Web.Security;
using SecurityInnovation.TeamMentor.WebClient.WebServices;
using System.IO;

namespace SecurityInnovation.TeamMentor.Authentication
{
    //Security review this code: Namely check if the use of an 32bit int is a strong enough value for the SSO Tokem
    public class SingleSignOn
    {
        public static bool singleSignOn_Enabled;
        public HttpContext context = HttpContext.Current;
        static SingleSignOn()
        {
            loadConfiguration();
        }
        public static void loadConfiguration()
        {
            singleSignOn_Enabled = TMConfig.Current.SingleSignOn_Enabled;
        }

        public Guid authenticateUserBasedOn_SSOToken()
        {
            try
            {
                var ssoToken = new StreamReader(context.Request.InputStream).ReadToEnd();
                //var ssoToken = context.Request.Form["ssoToken"];
                return authenticateUserBasedOn_SSOToken(ssoToken);
            }
            catch (Exception ex)
            {
                ex.log();
                return Guid.Empty;
            }
        }

        public Guid authenticateUserBasedOn_SSOToken(string ssoToken)
        {
            try
            {
                if (TMConfig.Current.SingleSignOn_Enabled.isFalse())                
                    "SSO request received but TMConfig.Current.SingleSignOn_Enabled is not set".error();
                else
                {
                    var tmUser = getUserFromSSOToken(ssoToken);
                    if (tmUser.notNull())
                    {
                        var sessionID = tmUser.registerUserSession(Guid.NewGuid());
                        new TM_WebServices().tmAuthentication.sessionID = sessionID;
                        return sessionID;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.log();
            }
            return Guid.Empty;
        }
        
        public TMUser getUserFromSSOToken(string ssoToken)
        {            
            if (ssoToken.valid())
            {
                try
                {
                    var decodedToken = decodeSSOToken(ssoToken);
                    var decodedUser = decodedToken.deserialize<TMUser>(false);
                    if (decodedUser.notNull())
                        foreach (var tmUser in TM_Xml_Database.TMUsers)
                            if (decodedUser.SSOKey == tmUser.SSOKey)                            
                                return tmUser;                        
                }
                catch (Exception ex)
                {
                    ex.log();
                }
            }
            return null;
        }
    
        public string getSSOTokenForUser(string userName)
        {
            return getSSOTokenForUser(userName.tmUser());
        }        

        public string getSSOTokenForUser(TMUser tmUser)
        {
            if (tmUser.isNull())
                return null;
            if (tmUser.SSOKey.isGuid().isFalse())
                tmUser.SSOKey = Guid.NewGuid().str();
            TM_Xml_Database.Current.saveTmUserDataToDisk();
            return MachineKey.Encode(tmUser.serialize(false).asciiBytes(),MachineKeyProtection.All);
        }

        public string decodeSSOToken(string ssoToken)
        {
            if (ssoToken.isNull())
                return null;
            try
            {                
                return MachineKey.Decode(ssoToken, MachineKeyProtection.All).ascii();                
            }
            catch (Exception ex)
            {
                ex.log();
                return null;
            }
        }
    }
}
