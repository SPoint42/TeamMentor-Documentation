
TM.Documentation = 
    {
            contentDiv              : "#PageContent"
        ,   open_HomePage           : function() 
                                        {
                                            TM.Documentation.open_in_ContentDiv("Pages/HomePage.htm");
                                        }
        ,   open_TableOfContents    : function() 
                                        {
                                            TM.Documentation.open_in_ContentDiv("Pages/TableOfContents.htm");
                                        }
        ,   open_in_ContentDiv      : function(page)
                                        {   
                                            $(this.contentDiv).load(page.add_TimeToUrl());
                                        }
        ,   open_SI_Website         : function()
                                        {
                                            window.open("http://www.securityinnovation.com" , "_blank");
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
                                            addLink("Table of Contents"     , this.open_TableOfContents);
                                            addLink("SecurityInnovation.com", this.open_SI_Website);
                                            

                                        }
        ,   buildGui                : function()
                                        {                                            
                                            TM.Documentation.add_LinksToMenu();
                                            //TM.Documentation.open_HomePage
                                            TM.Documentation.open_TableOfContents();

                                         
                                        }

        ,   loadData_and_BuildGuid  : function()
                                        {
                                            TM.WebServices.Data.extractFolderStructure(this.buildGui);
                                        }    

                   
    }