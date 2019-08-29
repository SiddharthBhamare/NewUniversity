# NewUniversity
Simple MVC based University Management 
--------------------------------------------------------------------------------
Setup Instructions 

1 Download zip project 
2 Restore Database School database using that backup file in SQL server
3 Open NewUniversity.sln using Visual Studtio



--------------------------------------------------------------------------------
Connection String and Email required Setup

1 Go to Web.config of inside NewUniversity directory

  <mailSettings> key add your gmail credentials or any other dummy credentials for sending email
  e.g.
  <smtp deliveryMethod="Network">
        <network host="smtp.gmail.com" port="587" userName="Your dummy email id " password="email password" enableSsl="true" />
      </smtp>
2 change connection string 
	change source i.e. your sql server name 
	change your catalog i.e. database name which will you restored
  
  
  e.g. "SID-PC\SQLEXPRESS" is Server username 
 No need password if your using windows authentication in SQL 
  
  <connectionStrings>
  e.g.  <add name="SchoolEntitiesDBContext" connectionString="metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=SID-PC\SQLEXPRESS;initial catalog=School;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
  </connectionStrings>
