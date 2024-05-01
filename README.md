<h1 align="center">
  SmartCharging Project Description
</h1>
.Net Core and MongoDB are used in the Smart Charging project. First of all, the installation requirements are given below. 



# Requirements

- Visual Studio 2022 with .NET 8 Framework
- MongoDB

# Smart Charging Project Setup

This project utilizes .NET Core and MongoDB for database operations. Follow the instructions below to set up your development environment.

## MongoDB Installation


1. Go to the [MongoDB download page](https://www.mongodb.com/try/download/community) and choose the appropriate version for your operating system.

2. Follow the installation instructions for your operating system provided on the download page.

3. After installation, start the MongoDB service. On most systems, you can do this by running the `mongod` command in your terminal.

4. Open a new terminal window and type `mongo`. If MongoDB is installed correctly, you will enter the MongoDB shell.

## Create SmartCharging Database

### Open MongoDB Shell:

1. Open a terminal window and type `mongo` to enter the MongoDB shell.

2. Inside the MongoDB shell, run the following command to create a database named SmartCharging:
   ```shell
   use SmartCharging
   ```

### Verify Database Creation:

3. To verify that the database was created successfully, you can run the following command:
    ```shell
     show dbs
    ```
Note: If you have problems with Mongo DB installation or if you want to install it by following the video, you can use the link below. MongoDB and MongoDB Compass visual interface installation is explained on this link. 
- https://youtu.be/jvaBaxlTqU8?si=Xt1YFaAjHfoQ7vnk
  
After the database is installed, we can run our SmartCharging project from Visual Studio.
When we run our project, you can access the Swagger UI interface from the link below.
- http://localhost:24760/swagger/index.html
  
You can access the Swagger UI json file from the link below.
- http://localhost:24760/swagger/v1/swagger.json

xUnit and Postman were used for project RestApi tests.

## xUnit Test
We need to add the SmartCharging.Tests.csproj file in the SmartCharging.Tests folder to the SmartCharging project. For this, we need to right click on Solutions on SmartCharging in Visual Studio, select Add->Existing Project and add SmartCharging.Tests.csproj file to the project.
Once we have included xUnit testing in our project, we can run the necessary tests.
Note: Before running the tests, you should edit the dummy data in GroupTest.cs, ConnectorTest.cs and ChargeStationTest.cs files.

## Postman Test
Postman Api tests were also used during project development. The related collection was shared as a json file. By importing this file into Postman, you can test the services and examine the parameters. 
In the link below, details about importing json file to collection are shared. 
- https://docs.tink.com/entries/articles/postman-collection-for-account-check




