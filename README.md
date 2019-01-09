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
