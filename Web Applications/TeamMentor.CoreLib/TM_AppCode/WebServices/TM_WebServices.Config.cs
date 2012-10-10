using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using System.Security.Permissions;	
using SecurityInnovation.TeamMentor.WebClient;
using SecurityInnovation.TeamMentor.Authentication.AuthorizationRules;
using SecurityInnovation.TeamMentor.Authentication.WebServices.AuthorizationRules;
//using Microsoft.Practices.Unity;
using O2.DotNetWrappers.ExtensionMethods;
using O2.XRules.Database.APIs;
using SecurityInnovation.TeamMentor.Authentication;
using urn.microsoft.guidanceexplorer;
//O2File:../IJavascriptProxy.cs
//O2File:../Authentication/UserRoleBaseSecurity.cs
//O2File:TM_WebServices.asmx.cs
//O2File:../FileUpload/FileUpload.cs
//O2Ref:System.Web.Services.dll 
//O2Ref:Microsoft.Practices.Unity.dll
//O2Ref:System.Xml.Linq.dll


namespace SecurityInnovation.TeamMentor.WebClient.WebServices
{ 					
	//WebServices related to: Config Methods
    public partial class TM_WebServices 
    {		
		
		[WebMethod(EnableSession = true)] public string GetTime() 						{   return "...Via Proxy:" + javascriptProxy.GetTime(); }
		[WebMethod(EnableSession = true)] public string Ping(string message)  			{   return "received ping: {0}".format(message); }
		
//        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string UseEnvironment_Moq()      		{   UnityInjection.useEnvironment_Moq(); 		return "using Moq Environment"; }
//		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string UseEnvironment_XmlDatabase()   	{   UnityInjection.useEnvironment_XmlDatabase();return "using XmlDatabase Environment"; }
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string CurrentProxyType()        		{ 	return javascriptProxy.ProxyType; }
		[WebMethod(EnableSession = true)] 											public string GetPasswordHash(string username, string password)		
																																	{	return username.createPasswordHash(password); }
		//Xml Database Specific
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string XmlDatabase_GetDatabasePath()		{	return TM_Xml_Database.Path_XmlDatabase;	}
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string XmlDatabase_GetLibraryPath()		{	return TM_Xml_Database.Path_XmlLibraries;	}		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string XmlDatabase_ReloadData()			{	
																																		guiObjectsCacheOK = false; 
																																		return  TM_Xml_Database.Current.reloadData(null); 
																																	}
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public bool XmlDatabase_ImportLibrary_fromZipFile(string pathToZipFile, string unzipPassword)
																																	{
																																		if (TM_Xml_Database.Current.xmlDB_Libraries_ImportFromZip(pathToZipFile, unzipPassword))
																																		{
																																			this.XmlDatabase_ReloadData();
																																			return true;
																																		}
																																		return false;																																		
																																	}
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string XmlDatabase_SetLibraryPath(string libraryPath)		
																																	{	guiObjectsCacheOK = false; 
																																		return  TM_Xml_Database.Current.reloadData(libraryPath); 
																																	}

		[WebMethod(EnableSession = true)] public List<Guid>     XmlDatabase_GuidanceItems_SearchTitleAndHtml(List<Guid> guidanceItemsIds, string searchText)		{	 return  TM_Xml_Database.Current.guidanceItems_SearchTitleAndHtml(guidanceItemsIds,searchText); }																																		
		[WebMethod(EnableSession = true)] public string         XmlDatabase_GetGuidanceItemXml(Guid guidanceItemId)	    {	return  TM_Xml_Database.Current.xmlDB_guidanceItemXml(guidanceItemId); }        
        [WebMethod(EnableSession = true)] public string         XmlDatabase_GetGuidanceItemPath(Guid guidanceItemId)	{	return  TM_Xml_Database.Current.xmlDB_guidanceItemPath(guidanceItemId); }                
																	
		[WebMethod(EnableSession = true)] public string         RBAC_CurrentIdentity_Name()				                {	return new UserRoleBaseSecurity().currentIdentity_Name(); }
		[WebMethod(EnableSession = true)] public bool           RBAC_CurrentIdentity_IsAuthenticated()	                {	return new UserRoleBaseSecurity().currentIdentity_IsAuthenticated(); }
		[WebMethod(EnableSession = true)] public List<string>   RBAC_CurrentPrincipal_Roles()		                    {	return new UserRoleBaseSecurity().currentPrincipal_Roles().toList(); }
		[WebMethod(EnableSession = true)] public bool           RBAC_HasRole(string role)					            {	return RBAC_CurrentPrincipal_Roles().contains(role); }
		[WebMethod(EnableSession = true)] public bool           RBAC_IsAdmin()											{	return RBAC_CurrentPrincipal_Roles().contains("Admin"); }
		[WebMethod(EnableSession = true)] public string         RBAC_SessionCookie()						            {	return HttpContext.Current.Request.Cookies["Session"].notNull() 
																												                    ? HttpContext.Current.Request.Cookies["Session"].Value : ""; }

        [WebMethod(EnableSession = true)]		                                    public Guid		SSO_AuthenticateUser(string ssoToken)            {   return new SingleSignOn().authenticateUserBasedOn_SSOToken(ssoToken); }
        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public string	SSO_GetSSOTokenForUser(string userName)          {   return new SingleSignOn().getSSOTokenForUser(userName); }
        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]			public TMUser	SSO_GetUserFromSSOToken(string ssoToken)         {   return new SingleSignOn().getUserFromSSOToken(ssoToken); }                
																												
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		GitHub_Pull_Origin()	            {	return UtilMethods.syncWithGitHub_Pull_Origin();  }
        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		GitHub_Push_Origin()	            {	return UtilMethods.syncWithGitHub_Push_Origin();  }
        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		GitHub_Push_Commit()	            {	return UtilMethods.syncWithGitHub_Commit();  }
        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		Git_Execute(string gitCommand)	{	return UtilMethods.executeGitCommand(gitCommand);  }
		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		CreateWebEditorSecret()	
																									{
																										var webEditorSecretDataFile = AppDomain.CurrentDomain.BaseDirectory.pathCombine("webEditorSecretData.config");
																										Guid.NewGuid().str().serialize(webEditorSecretDataFile);
																										return webEditorSecretDataFile.load<string>();
																										//this (below) doesn't work because the webeditor is an *.ashx and doesn't have access to the HttpContext Session object
																										/*var session = System.Web.HttpContext.Current.Session;
																										if (session["webEditorSecretData"].isNull())
																											session["webEditorSecretData"] = Guid.NewGuid().str();
																										return (string)session["webEditorSecretData"];
																										*/
																									}						
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		TMConfigFileLocation()			{	return TMConfig.Location;  }		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public TMConfig		TMConfigFile()
																					{	
																						return TMConfig.Current;  
																					}																					

		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public List<string> GetDisabledLibraries()
																					{	
																						return TMConfig.Current.Libraries_Disabled;  																					}		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public List<string> SetDisabledLibraries( List<string> disabledLibraries)
																					{
																						var config = TMConfig.Current;
																						config.Libraries_Disabled = disabledLibraries;
																						if (config.SaveTMConfig())
																							return TMConfig.Current.Libraries_Disabled;
																						return 
																							null;
																					}				

		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		Get_Libraries_Zip_Folder()
																					{
                                                                                        var librariesZipsFolder = TMConfig.Current.LibrariesUploadedFiles;                                                                                        
                                                                                        return TM_Xml_Database.Path_XmlDatabase.fullPath().pathCombine(librariesZipsFolder).fullPath();
																					}		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public List<string> Get_Libraries_Zip_Folder_Files()
																					{
                                                                                        return Get_Libraries_Zip_Folder().files().fileNames();
																					}																							
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]		public string		Set_Libraries_Zip_Folder(string folder)
																					{
																						var tmConfig = TMConfig.Current;
																						tmConfig.LibrariesUploadedFiles = folder;
																						//folder.createDir();
																						if (tmConfig.SaveTMConfig())																																										
																							return "Path set to '{0}' which currently has {1} files".format(folder.fullPath(), folder.files().size());
																						return null;
																					}

        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public Guid			GetUploadToken()
                                                                                    {
                                                                                        var uploadToken = Guid.NewGuid();
                                                                                        FileUpload.UploadTokens.Add(uploadToken);
                                                                                        return uploadToken;
																					}
        [WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public string		GetLogs()        
                                                                                    {
                                                                                        var logData = O2.Kernel.PublicDI.log.LogRedirectionTarget.prop("LogData").str() ;
                                                                                        return logData;
                                                                                    }        
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public List<KeyValue<Guid, string>>				Data_GuidanceItems_FileMappings()        
                                                                                    {
																						return TM_Xml_Database.GuidanceItems_FileMappings.ConvertDictionary();
                                                                                    }		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)] 		public List<KeyValue<Guid, TeamMentor_Article>> Data_GuidanceItems_Cached_GuidanceItems()        
                                                                                    {
																						return TM_Xml_Database.Cached_GuidanceItems.ConvertDictionary();
                                                                                    }
		
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public string		REPL_ExecuteSnippet(string snippet)        
                                                                                    {
																						return REPL.executeSnippet(snippet);
                                                                                    }
		
		//Virtual Articles
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public List<VirtualArticleAction>	VirtualArticle_GetCurrentMappings()        
                                                                                    {
																						return TM_Xml_Database.Current.getVirtualArticles().Values.toList();
                                                                                    }				
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public VirtualArticleAction			VirtualArticle_Add_Mapping_VirtualId( Guid id, Guid virtualId)
                                                                                    {
																						return TM_Xml_Database.Current.add_Mapping_VirtualId(id, virtualId);																						
                                                                                    }
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public VirtualArticleAction			VirtualArticle_Add_Mapping_Redirect (Guid id, string redirectUri)
                                                                                    {
																						return TM_Xml_Database.Current.add_Mapping_Redirect(id, redirectUri.uri());																						
                                                                                    }
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public VirtualArticleAction			VirtualArticle_Add_Mapping_ExternalArticle(Guid id, string tmServer, Guid externalId)
                                                                                    {
																						return TM_Xml_Database.Current.add_Mapping_ExternalArticle(id, tmServer, externalId);																						
                                                                                    }			
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public VirtualArticleAction			VirtualArticle_Add_Mapping_ExternalService(Guid id, string service, string data)
                                                                                    {
																						return TM_Xml_Database.Current.add_Mapping_ExternalService(id, service, data);																						
                                                                                    }			
		[WebMethod(EnableSession = true)] [Admin(SecurityAction.Demand)]        public bool							VirtualArticle_Remove_Mapping( Guid id)
                                                                                    {
																						return TM_Xml_Database.Current.remove_Mapping_VirtualId(id);																						
                                                                                    }
		[WebMethod(EnableSession = true)] [ReadArticles(SecurityAction.Demand)]     public string					VirtualArticle_Get_GuidRedirect(Guid id)
																					{
																						return TM_Xml_Database.Current.get_GuidRedirect(id);																						
                                                                                    }				
		[WebMethod(EnableSession = true)] [ReadArticles(SecurityAction.Demand)]     public TeamMentor_Article		VirtualArticle_CreateArticle_from_ExternalServiceData(string service, string serviceData)
																					{
																						return service.createArticle_from_ExternalServiceData(serviceData);																						
                                                                                    }
		
		//Article Guid Mappings
		[WebMethod(EnableSession = true)]		public Guid getGuidForMapping(string mapping)
		{
			return TM_Xml_Database.Current.xmlBD_resolveMappingToArticleGuid(mapping);
		}
		[WebMethod(EnableSession = true)]		public bool IsGuidMappedInThisServer(Guid guid)
												{
													if (GetGuidanceItemById(guid.str()).notNull())
														return true;
													if (TM_Xml_Database.Current.get_GuidRedirect(guid).valid())
														return true;
													return false;
												}
    }	
}
