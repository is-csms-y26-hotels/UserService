# User Service - README  

## 📌 Description  
**UserService** is the master system for managing users in the hotel booking domain.  
It handles user creation and validation while generating events for notifications.  

## 🚀 Features  
- **Create a new user**  
- **User data validation**  
- **Generate events for notifications** (`UserRegistration` topic)  

## 🔗 Service Interactions  
### Synchronous (gRPC):  
- **Gateway ↔ UserService** — Handles user creation requests  

### Asynchronous (Kafka):  
- **`UserRegistration` topic** — Sends user registration events (for NotificationService, StatisticsService)  

## 🔧 Architecture  
- **Language:** C#  
- **gRPC API** for communication with the gateway  
- **Kafka** for event-driven notifications  
- **Database:** PostgreSQL  

## 📜 Business Process: User Creation  
1. The gateway sends a gRPC request to **UserService**  
2. **UserService** validates the user data  
3. If successful, it creates the user and responds to the gateway  
4. Sends a message to the `UserRegistration` topic for NotificationService  

## 👥 Team Members  
- 🏗 **Isaev Daniil** — UserService, Gateway  

