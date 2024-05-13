# CDR Inserter Demo

Simplified CDR Inserter Demo written in .NET Core, C#. Technologies used in this project include Worker Services, Kafka, Docker, Multi-Threading, and realtime processing. Demonstrated thread synchronization by using ManualResetEvent and showed how Multithreading could help in processing large amounts of CDR in realtime.

## Author
Tea Bin Xiong

## Possible Improvements
1. Adding Outbox Pattern to the publishing event to ensure data consistency.
2. Add error handling to the consumer service in the event that the consumer is unable to consume the message.
3. Add more CDR related files, e.g., Usage Detail Record(UDR) in this program.


## Architecture Diagram
![image](./img/cdr-inserter-architecture.jpg)


## Projects
1) CDR Consumer App - Cdr.Inserter.Monitor
2) CDR Inserter Workers - Cdr.Inserter.Workers

## Infrastructure
1) Docker
2) Kafka

## Quick start guide
- Step 1: Ensure Docker Desktop is installed.
- Step 2: Navigate to the root project folder with your command line of choice, and enter:
```
docker compose up
```
- Step 3: Navigate to [http://localhost:9000](http://localhost:9000) with your browser to check if the Kafka cluster is up.
- Step 4: Place a sample CSV file in the storage folder to trigger the data ingestion.
- Step 5: You should see the CDR file content under the CDR Consumer App logs.


## Repository URL
[cdr-inserter-demo](https://github.com/teabinxiong/cdr-inserter-demo)
