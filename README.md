# email_tool


Instructions to Pull and Build the Project
Clone the Repository
Open a terminal and run the following command to clone the repository:


git clone https://github.com/deke-create/email_tool.git





To Run the api (backend) App

1. Navigate to the project directory. Change to the project directory:

cd email_tool.api

2. Restore Dependencies. Use the .NET CLI to restore the required dependencies:

dotnet restore

3. Build the Project:

dotnet build

4. Run the Application:  dotnet run --project email_tool.api


________________________________________________________________


To Run the Client App

1. Navigate to the project directory. Change to the project directory:

cd email_tool.client

2. Restore Dependencies. Use the .NET CLI to restore the required dependencies:

dotnet restore

3. Build the Project:

dotnet build

4. Run the Application:  dotnet run --project email_tool.client



___________________________________________________



Access the API with http client: 

Project includes a pre-configured Postman data file which can be easily imported into Postman to test out the project's "Login" and "Send Message" endpoints:  email_tool.postman_collection.json 







