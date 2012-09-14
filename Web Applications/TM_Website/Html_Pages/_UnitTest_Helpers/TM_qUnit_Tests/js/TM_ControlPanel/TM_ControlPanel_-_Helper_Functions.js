//Helper functions for TM_ControlPanel_* tests

TM.QUnit.ControlPanel = 
	{	
		guiLoaded 		: false,
		moduleSetUp 	: function() 
			{						
				if (TM.QUnit.ControlPanel.guiLoaded)
				{
					start();
					return;
				}
				stop();
				TM.Events.onControlPanelGuiLoaded.add_InvokeOnce(function() 
					{
						TM.QUnit.ControlPanel.guiLoaded = true;									
						start();
					});
			}
	};

	
var qunit_ControlPanel_Helper = 
	{
/*		openControlPanelGui: function() 
			{
				ok(TM.Gui.Main.homePage,"TM.Gui.Main.homePage function is defined")
				ok(TM.Gui.ShowProgressBar,"TM.Gui.ShowProgressBar function is defined")
				
				TM.Gui.Main.homePage("#Canvas");						
				TM.Events.onHomePageLinksLoaded.add(start);
			},
*/			
		logout: function()
			{
				var logoutButton = $("a:contains('Logout')");	
				if(logoutButton.length == 1)
				{		
					logoutButton.click();
					
					TM.Events.onControlPanelGuiLoaded.add_InvokeOnce(function() 
						{  							
							start() ;
						});
				}
				else
					start();
			},
		loginAs: function(username, password)
			{
			
				var loginButton = $("a:contains('Login')").click();
				//var logoutButton = $("#topRightMenu a:contains('Logout')");	
				
				equals(1,loginButton.length,"There was one login link");
				//equals(0,logoutButton.length,"There was one no logout button");
				
				
				TM.Events.onLoginDialogOpen.add_InvokeOnce(function()
					{
						$("#UsernameBox").val(username)
						$("#PasswordBox").val(password)
						$("button:contains('login')").click();										
					});		
				TM.Events.onUserDataLoaded.add_InvokeOnce(function()	
					{
						TM.Events.onLoginDialogOpen.remove();
						loadControlPanelDefaultPages();			
						start();
					});
				loginButton.click();
			},
		loginAs_Admin: function()
			{
				if (TM.Gui.CurrentUser.isAdmin())
					start();
				else
				{						
					qunit_ControlPanel_Helper.loginAs(TM.QUnit.defaultUser_Admin, TM.QUnit.defaultPassord_Admin);					
				}
			}
	}