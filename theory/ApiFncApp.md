# Web API & Fn App Interview Q&A

1. RESTful (REST) vs RESTless (SOAP)?
2. What are REST principles?
3. Caching types?
4. Error handling in Web API? try-catch, exception filters, global error handling, custom error responses.
5. What is content negotiation in Web API?
6. Anti-forgery token?
7. Cookies in ASP.NET?
8. State and session management in ASP.NET?
9. Authentication — Types, JWT, OAuth2, OpenID, Identity?
10. How do we create and validate a JWT token? Benefits
11. Authorization — ways? Role-based, claims-based, policy-based, resource-based, attribute-based.
12. Bundling and minification in .NET Core?
13. What are HTTP verbs and Http status codes?
14. How to make REST APIs more secure?
15. What are WebSockets?
16. Why is Web API required even if WCF supports RESTful services?
17. What is CORS? How to enable CORS in Web API?
18. What is gRPC? Benefits of gRPC over REST?
19. What is API versioning? How to implement it in Web API?
20. What is rate limiting and retry policy?
21. What is API gateway and AZure API management?
22. What is RabitMQ, Kafka and AZure Service Bus.
23. What  are caching strategies in Web API? In-memory caching, distributed caching, response caching, cache-control headers.
24. How to build a high traffic Web API? Load balancing, horizontal scaling, vertical scaling, database optimization, caching strategies, asynchronous processing, message queues.
25. How wpuld you ensure data consistency in a distributed system? Eventual consistency, distributed transactions, idempotency, versioning, conflict resolution strategies.
26. Design a notification system for email sending in a Web API. Use a message queue (like RabbitMQ or Kafka) to handle email requests asynchronously, implement retry policies for failed deliveries, and use a database to track the status of each email.
27. How to desing a real time collaboration tool like Google docs using Web API? Use WebSockets for real-time communication, implement a document versioning system to handle concurrent edits, and use a database to store document states and user actions.
28. How you migrate database in production? Flyway or EF Core migrations, use a staging environment to test migrations before applying them to production, and implement a rollback strategy in case of failures.
29. Api is using 80 % CPU and 90% memory. How would you troubleshoot it? Use profiling tools to identify performance bottlenecks, analyze database queries for optimization, implement caching strategies, and consider scaling the application horizontally or vertically.
30. An enpoint is throwing OutOfMemoryException. What could be wrong?
31. Memory usage spikes every hour. What could be the cause? Check for memory leaks in the application, analyze background tasks or scheduled jobs that run hourly, and review caching strategies to ensure they are not consuming excessive memory.
32. Prod server went down. How would you troubleshoot it?
33. Users can't login to the application. How would you troubleshoot it?
34. Memory leak in production.Causing daily restart. How would you troubleshoot it?
35. Deployment failed mid-way. Rollback strategy? Use a blue-green deployment strategy to minimize downtime, implement automated rollback scripts to revert to the previous stable version, and ensure that database migrations are reversible or have backup plans in place.
