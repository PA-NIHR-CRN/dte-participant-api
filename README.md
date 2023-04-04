# Participant API

## Table of Contents
1. [Project's Title](#projects-title)
2. [Project Description](#project-description)
3. [How to Install and Run the Project](#how-to-install-and-run-the-project)
4. [How to Deploy the Project to AWS Lambda](#how-to-deploy-the-project-to-aws-lambda)
5. [How to Use the Project](#how-to-use-the-project)

## Project's Title
**Participant API**: A C#-based RESTful API for managing and tracking participants in research studies, deployed on AWS Lambda and API Gateway.

## Project Description
Participant API is a C#-developed RESTful API designed to help researchers manage and track participants in various research studies. The primary goal of this project is to provide an efficient and easy-to-use interface for creating, updating, and deleting participant records, as well as facilitating communication and data sharing between researchers and study participants.

The API was developed using the ASP.NET Core framework and designed to run on AWS Lambda behind AWS API Gateway. This serverless architecture ensures optimal performance, scalability, and security. Challenges faced during the development process included implementing user authentication, handling sensitive data securely, and creating a flexible system that can accommodate various types of research studies. Future features to be implemented include advanced search and filtering options, integration with third-party research platforms, and real-time data analysis capabilities.

## How to Install and Run the Project
Follow these steps to set up and run the Participant API project locally:

1. Ensure you have the .NET SDK installed on your machine. If not, download and install it from the [.NET official website](https://dotnet.microsoft.com/download).

2. Clone the repository to your local machine:
```
git clone https://github.com/yourusername/participant-api.git
```

3. Navigate to the project folder:
```
cd participant-api
```

4. Restore the required dependencies and build the project:
```
dotnet restore
dotnet build
```
5. Start the development server:
```
dotnet run
```
The API should now be accessible at `http://localhost:7001`.

## How to Deploy the Project to AWS Lambda
Follow these steps to deploy the Participant API project to AWS Lambda:

1. Ensure you have the AWS CLI installed and configured on your machine. If not, download and install it from the [AWS CLI official website](https://aws.amazon.com/cli/) and follow the [configuration guide](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-configure.html).

2. Install the AWS Lambda .NET Core Global Tool:
```
dotnet tool install -g Amazon.Lambda.Tools
```
Follow the prompts to configure the deployment settings. Once the deployment is complete, you will receive the API Gateway URL for accessing your deployed API.

## How to Use the Project
To use the Participant API, you can make HTTP requests to the available endpoints using tools like [Postman](https://www.postman.com/) or [Insomnia](https://insomnia.rest/), or by integrating the API into your own applications.

The API provides endpoints for managing participants, such as creating, updating, deleting, and fetching participant records. A detailed API documentation is available at the `/swagger` endpoint


