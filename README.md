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


### Function App

Open the Azure Function App project and change the database name as well as the connection string to Cosmos DB

Change the TextAnalytics API initialization with your own subscription key and data center

Deploy the Function locally from Visual Studio

### Cosmos DB Client Application

Open the Cosmos DB Client application and change the databas name as well as the connection string to Cosmos DB

Run the application from Visual Studio and add some reviews from the UI