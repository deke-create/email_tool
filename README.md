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



Access the API http (e.g. Postman) client: 

Step 1 (Login):
curl --location 'http://localhost:5000/api/auth/login' \
--header 'Content-Type: application/json' \
--data '{
    "Username": "poweruser",
    "Password": "password123"
}'



example response below which includes access token (see 'data' property) needed for subsequent api request(s):

{
    "status": 0,
    "message": "Login successful",
    "data": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwb3dlcnVzZXIiLCJqdGkiOiI3MmUyMjlmNC0yZTcyLTQ1NTctOWRlNS1iOTc2ODU5Yjc5NGYiLCJleHAiOjE3NDU1NDIzNjYsImlzcyI6ImVtYWlsX3Rvb2xfYXBpIiwiYXVkIjoiZW1haWxfdG9vbF91c2VycyJ9.ckViyKrEgJmgIZlXOZ55hYePbgQ3lpStgzIAz9GFxHY"
}



Step 2 (Send Message):

[POST]: http://localhost:5000/api/email/send


curl --location 'http://localhost:5000/api/email/send' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJwb3dlcnVzZXIiLCJqdGkiOiI0ZWM4YjE2OS0yYWViLTQwNDQtYWQzYy00ZmRhNTk5MTk4MzQiLCJleHAiOjE3NDU1NDAxODcsImlzcyI6ImVtYWlsX3Rvb2xfYXBpIiwiYXVkIjoiZW1haWxfdG9vbF91c2VycyJ9.Hjvy0h5LvKHCZPrtKEY_oBxuBZrtRXpGlhPgA6MfzNM' \
--data-raw '{
  "Sender": "example@example.com",
  "Recipient": "recipient@example.com",
  "Subject": "Sample Subject",
  "Body": "This is the body of the message."
}'








