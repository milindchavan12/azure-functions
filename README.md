# Azure Functions
> Azure Functions is the **Function as a Service (FaaS)** i.e. Serverless offering from Microsoft Azure platform which speeds up development, reduce costs and scales automatically.

## Trigger
Azure Functions uses Event-Driven Programming Model, which means functions responses to an Event. The most common triggers are:
- Timer : Runs the function on schedule.
- Message : Listens the message on the queue.
- Http : Implements the Web API or Webhooks
- Other : Blob creation, Cosmos Db new row

## Bindings
To integrate with other services, Function provides a input and output bindings.

## Hosting Models:
- Consumption Plan (Serverless)
- App Service Plan
- Docker Container.

## Azure Functions Proxies
Azure functions proxies are the ways to intercept all incoming HTTP traffic to the function app. We can use them to proxy the functions in our functions app or completely different destinations. To implement the proxies, we need to create *proxies.json* in our functions app.

![alt text]()
