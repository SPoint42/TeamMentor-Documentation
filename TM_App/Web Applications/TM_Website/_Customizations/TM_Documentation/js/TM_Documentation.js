
TM.Documentation = 
    {
            contentDiv              : "#PageContent"
		,	leftMenuDiv				: "#LeftMenu"
        ,   open_HomePage           : function() 
                                        {
                                            TM.Documentation.open_in_ContentDiv("Pages/HomePage.htm");
                                        }
        ,   open_TableOfContents    : function() 
                                        {
                                            TM.Documentation
											$(TM.Documentation.leftMenuDiv).load("Pages/TableOfContents.htm".add_TimeToUrl());
                                        }
        ,   open_in_ContentDiv      : function(page)
                                        {   
                                            $(this.contentDiv).load(page.add_TimeToUrl());
                                        }
        ,   open_SI_Website         : function()
                                        {
                                            window.open("http://www.securityinnovation.com" , "_blank");
                                        }
        ,   open_TM_Website         : function()
                                        {
                                            window.open("/html_pages/Gui/TeamMentor.html", "_self");
                                        }
        ,   add_LinksToMenu         : function()
                                        {
                                            var addLink = function(text,onClick)
                                                            {
                                                                $("<li>").append(   
                                                                                    $("<a>").attr("href","#")
                                                                                    .append(text)
                                                                                    .click(onClick))
                                                                         .appendTo(".nav")
                                                            }
                                            addLink("Home Page"             , this.open_HomePage);
                                            //addLink("Table of Contents"     , this.open_TableOfContents);                                            
                                            addLink("This TeamMentor Website", this.open_TM_Website);
                                            addLink("SecurityInnovation.com", this.open_SI_Website);

                                        }
        ,   buildGui                : function()
                                        {   
											
                                            TM.Documentation.add_LinksToMenu();
                                            TM.Documentation.open_HomePage();
											TM.Documentation.open_TableOfContents();                                            

											
											
                                        }
		,   cssFixes                : function()
										{
											$(".container").width("100%")
											$("#LeftMenu").width(230);
											$("#PageContent").absolute().left(400).top(60);

											$('.fill').css('background', '#000055');
											$('body').css('font-family','Helvetica, Arial, sans-serif');
										}
        ,   loadData_and_BuildGuid  : function()
                                        {
											this.cssFixes();
                                            TM.WebServices.Data.extractFolderStructure(this.buildGui);
                                        }    

                   
    }