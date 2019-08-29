# NewUniversity
Simple MVC based University Management 
--------------------------------------------------------------------------------
Setup Instructions 

1 Download zip project 
2 Go to the Database Backup Folder take Resrore that backup from Sql Server 
3 Open NewUniversity.sln using Visual Studtio



--------------------------------------------------------------------------------
Send Credentials to Email -- Setup And Connection to Database 

1 Go to Web.config 
  <mailSettings> key add your gmail credentials or any other dumy credentials for sending email
  e.g.
  <smtp deliveryMethod="Network">
        <network host="smtp.gmail.com" port="587" userName="Your dummy email id " password="email password" enableSsl="true" />
      </smtp>
2 change connection string 
	changes source i.e. your sql server name 
	change your catalog i.e. database name which will you restored
  <connectionStrings>
    <add name="SchoolEntitiesDBContext" connectionString="metadata=res://*/Models.Model1.csdl|res://*/Models.Model1.ssdl|res://*/Models.Model1.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=SID-PC\SQLEXPRESS;initial catalog=School;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
  </connectionStrings>