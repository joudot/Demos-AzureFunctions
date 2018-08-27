# Demo Azure Functions with Text Analytics to augment data stored in Cosmos DB

The intend of this project is to share a demo I built to show how to integrate Cognitive Services capabilities into an Azure Function listening to new documents added into a Cosmos DB account. Especially, the goal is to augment reviews posted by some website users with an evaluation. Is it a positive, negative or neutral review.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

To run this demo, you will need:

```
Visual Studio 2017

The Azure Functions runtime (v1)

A subscription key allowing you to call the Text Analytics API

An Azure subscription with a Cosmos DB account containing a Review collection and a leases collection
```


## Set up the demo

First, clone the repository on your machine:
```
git clone: https://github.com/joudot/Demos-AzureFunctions
```

### Function App

Open the Azure Function App solution located at: .\Demos-AzureFunctions\AzureFunctionApp\ReviewEvaluationFunctionApp

In local.settings.json, replace the AccountEndpoint and the AccountKey using the information of your Cosmos DB account

In ReviewEvaluationFunction.cs:
- Replace the database name and the collection name with existing database and collection in your Cosmos DB environment
- Create a new collection named leases in your Cosmos DB environment. It will be used by the Change Feed listener
- Replace your Subscription Key to be authorized to access the text analytics client. If you don't have any key, you can create a new "Text Analytics" resource in Azure
You will also need to pay attention to the region since it is a parameter of the client in the Function Code. If you just want to test the Cosmos DB parts, just remove the API call to the Cognitive Service.
To test that we actually process the document from the Azure function, you can hardcode the value of the review from the Function code. 

Run the ReviewEvaluationFunction locally from Visual Studio and check that there is no issue connecting to the Cosmos Db collection we want to listen to

### Cosmos DB Client Application

Open the Cosmos DB Client solution .\CosmosDBClientApplication\ReviewWebsite

In appsettings.Development.json, replace the endpoint URL, authorization key, database name and collection name by the information retrieved from your Cosmos DB collection

Run the Application and Add a review

Refresh your browser. If the function was able to process the new review, the evaluation should be tagged as good or bad
